/////////////////////////////////////////////////////////////////////////////////////
//  File:   OspServer.cs                                            5 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using AdditionalData;
using Held;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Logging;
using Ng911Lib.Utilities;
using OspSimulator.Settings;
using SipLib.Core;
using SipLib.Logging;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Veds;

namespace OspSimulator;

/// <summary>
/// This class provides an HTTP or HTTPS server that provides call location data using the HELD protocol (see RFC
/// 5985 and RFC 6155) and NG9-1-1 call Additional Data (see RFC 7852).
/// <para>
/// To use this class, construct an instance of and and then call the Start() method. An application
/// must call the ShutdownAsync() method to stop and shut down this server before it exits.
/// </para>
/// </summary>
public class OspServer
{
    private WebApplication? m_WebApplication = null;
    private X509Certificate2 m_ServerCert;
    private IPEndPoint m_ServerEndPoint;
    private bool m_UseHttps;
    private AdditionalDataStore m_AdditionalDataStore;

    /// <summary>
    /// Root for the path for a HELD location request. The request format for location requests is:
    /// http://IPEndPoint/Location/8185553333. The scheme may be http or https.
    /// </summary>
    public const string LOCATION_PATH_ROOT = "Location";

    /// <summary>
    /// Root for the path for a request for additional data. The request format for additional data is:
    /// http://IPEndPoint/AdditionalData/8185553333/InfoType. InfoType is one of: Comments/CommentIdentifier,
    /// DeviceInfo, Providers/ProviderIdentifier, ServiceInfo, SubscriberInfo or AutomatedCrashNotification.
    /// </summary>
    public const string ADDITIONAL_DATA_ROOT = "AdditionalData";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="certificate">X.509 certificate to use for HTTPS. Note: This is required even if
    /// not using HTTPS.</param>
    /// <param name="networkSettings">Network configuration settings for the OspSimulator application.</param>
    /// <param name="additionalDataStore">Contains the location data and additional data for one or
    /// more calling party numbers.</param>
    public OspServer(X509Certificate2 certificate, NetworkSettings networkSettings, AdditionalDataStore additionalDataStore)
    {
        m_ServerCert = certificate;
        m_UseHttps = networkSettings.UseHttps;
        m_AdditionalDataStore = additionalDataStore;
        string strIPAddress;
        int port;
        if (networkSettings.UseIPv4ForHttp == true)
            strIPAddress = networkSettings.IPv4Address;
        else
            strIPAddress = networkSettings.IPv6Address;

        if (m_UseHttps == true)
            port = networkSettings.HttpsPortNumber;
        else
            port = networkSettings.HttpPortNumber;

        m_ServerEndPoint = new IPEndPoint(IPAddress.Parse(strIPAddress), port);
    }

    /// <summary>
    /// Gets the IPEndPoint that the server is listening on.
    /// </summary>
    public IPEndPoint ServerEndPoint
    {
        get { return m_ServerEndPoint; }
    }

    /// <summary>
    /// Creates and starts a WebApplication object that listens on an HTTP or an HTTPS endpoint for
    /// requests for location or additional data.
    /// </summary>
    public void Start()
    {
        if (m_WebApplication != null)
            return;     // Already started

        ClientCertificateMode CertMode = ClientCertificateMode.NoCertificate;

        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        // See: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-10.0&preserve-view=true
        builder.WebHost.UseKestrel(options =>
        {
            if (m_UseHttps == true)
            {
                options.Listen(m_ServerEndPoint.Address, m_ServerEndPoint.Port, listenOptions =>
                {
                    listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                    {
                        ServerCertificate = m_ServerCert,
                        ClientCertificateMode = CertMode,
                        ClientCertificateValidation = DoMutualAuthentication!
                    });
                });
            }
            else
            {   // Listen for requests using HTTP
                options.Listen(m_ServerEndPoint);
            }
        })
            .ConfigureLogging((context, logging) =>
            {   // Turn off logging because the ASP .NET CORE framework generates a lot of meaningless
                // log messages.
                logging.ClearProviders();
            });

        m_WebApplication = builder.Build();

        m_WebApplication.Use(RequestHandler);
        SipLogger.LogInformation("Starting OspServer now.");
        m_WebApplication.StartAsync(CancellationToken.None);
    }

    /// <summary>
    /// Shuts down the WebApplication. This method must be called in order to free up the network
    /// resources used by the WebApplication.
    /// </summary>
    /// <returns></returns>
    public async Task ShutdownAsync()
    {
        if (m_WebApplication == null)
            return;

        SipLogger.LogInformation("Shutting down OspServer now.");
        await m_WebApplication.StopAsync();
        await m_WebApplication.DisposeAsync();
        m_WebApplication = null;
    }

    private async Task RequestHandler(HttpContext context, RequestDelegate next)
    {
        IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;
        string strRemoteIpAddress = remoteIpAddress == null ? "Unknown" : remoteIpAddress.ToString();

        string strPath = context.Request.Path;
        if (string.IsNullOrEmpty(strPath) == true)
        {
            context.Response.StatusCode = 400;      // Bad Request
            SipLogger.LogError($"No request path provided from {strRemoteIpAddress}");
            return;
        }

        strPath = strPath.ToLower();
        if (strPath.IndexOf(LOCATION_PATH_ROOT.ToLower()) >= 0)
        {
            if (context.Request.Method == "POST")
                await HandleLocationPostRequest(context, strPath, strRemoteIpAddress);
            else if (context.Request.Method == "GET")
                // For debug only. The HELD protocol uses only the POST method.
                await HandleLocationGetRequest(context, strPath, strRemoteIpAddress);
            return;
        }
        else if (strPath.IndexOf(ADDITIONAL_DATA_ROOT.ToLower()) >= 0)
        {
            await AdditionalDataRequestHandler(context, context.Request.Path);
            return;
        }

        await next(context);
    }

    /// <summary>
    /// Handles a HELD POST request for location data.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPath"></param>
    /// <param name="strRemoteIpAddress"></param>
    /// <returns></returns>
    private async Task HandleLocationPostRequest(HttpContext context, string strPath, string strRemoteIpAddress)
    {
        if (context.Request.Method != "POST")
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            SipLogger.LogError($"Unsupported request method = {context.Request.Method} from {strRemoteIpAddress}");
            return;
        }

        string? strCallerID = GetCallingPartyNumber(strPath);
        if (string.IsNullOrEmpty(strCallerID) == true)    
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            SipLogger.LogError($"Unable to find the calling party number in the request from {strRemoteIpAddress}");
            return;
        }

        // Read the body of the POST request
        long? ContentLength = context.Request.ContentLength;
        if (ContentLength == null || ContentLength == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            SipLogger.LogError($"No body in the location POST request from {strRemoteIpAddress}");
            return;
        }

        byte[] BodyBytes = new byte[(int)ContentLength];
        int BytesRead = 0;
        //BytesRead = await context.Request.Body.ReadAsync(BodyBytes, 0, (int)ContentLength);
        Memory<byte> mem = new Memory<byte>(BodyBytes);
        BytesRead = await context.Request.Body.ReadAsync(mem);

        if (BytesRead <= 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            SipLogger.LogError($"Unable to read the locationRequest object from {strRemoteIpAddress}");
            return; 
        }

        string strLocationRequest = Encoding.UTF8.GetString(BodyBytes);
        LocationRequest? locationRequest = XmlHelper.DeserializeFromString<LocationRequest>(strLocationRequest);

        if (locationRequest == null)
        {
            await WriteHeldError("xmlError", "The locationRequest XML is not valid", context.Response);
            SipLogger.LogError($"Unable to deserialize the locationRequest object from {strRemoteIpAddress}");
            return;
        }

        string? uri = locationRequest.device?.uri;
        string? callingPartyNumber = null;
        if (uri == null)
        {   // Try to get the callingPartyNumber from the request path
            string[] pathFields = strPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (pathFields.Length != 2)
            {
                await WriteHeldError("xmlError", "The locationRequest device or device.uri is null " +
                    "and the path does not contain a calling party number.", context.Response);
                SipLogger.LogError($"Null device or device.uri or no calling party number in the request path in a locationRequest from {strRemoteIpAddress}");
                return;
            }

            callingPartyNumber = pathFields[1];
        }
        else
        {
            SIPURI? sipUri = null;
            callingPartyNumber = null;
            if (SIPURI.TryParse(uri, out sipUri) == false || sipUri is null)
            {
                await WriteHeldError("xmlError", "Invalid device uri provided", context.Response);
                SipLogger.LogError($"Invalid URI in a locationRequest from {strRemoteIpAddress}");
                return;
            }

            callingPartyNumber = sipUri.User;
        }

        if (callingPartyNumber == null)
        {
            await WriteHeldError("requestError", "No calling party number provided", context.Response);
            return;
        }

        CallAdditionalData? callAdditionalData = m_AdditionalDataStore.GetCallAdditionalData(callingPartyNumber);
        if (callAdditionalData == null || callAdditionalData.Location == null)
        {
            await WriteHeldError("locationUknown", $"No location for: {callingPartyNumber}", context.Response);
            return;
        }

        LocationResponse locationResponse = new LocationResponse();
        locationResponse.presence = callAdditionalData.Location;
        await WriteHeldResponse(XmlHelper.SerializeToString(locationResponse), context.Response);
    }

    /// <summary>
    /// Handles a GET request for a HELD location request. Note: HELD does not use the GET method. This
    /// method is for testing only. Returns a HELD locationResponse XML object to the requestor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPath"></param>
    /// <param name="strRemoteIpAddress"></param>
    /// <returns></returns>
    private async Task HandleLocationGetRequest(HttpContext context, string strPath, string strRemoteIpAddress)
    {
        string? callingPartyNumber = GetCallingPartyNumber(strPath);
        if (string.IsNullOrEmpty(callingPartyNumber) == true)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            SipLogger.LogError($"Unable to find the calling party number in the request from {strRemoteIpAddress}");
            return;
        }

        CallAdditionalData? callAdditionalData = m_AdditionalDataStore.GetCallAdditionalData(callingPartyNumber);
        if (callAdditionalData == null || callAdditionalData.Location == null)
        {
            await WriteHeldError("locationUknown", $"No location for: {callingPartyNumber}", context.Response);
            return;
        }

        LocationResponse locationResponse = new LocationResponse();
        locationResponse.presence = callAdditionalData.Location;
        await WriteHeldResponse(XmlHelper.SerializeToString(locationResponse), context.Response);
    }

    private async Task WriteHeldError(string strHeldErrorCode, string strHeldErrorMessage, HttpResponse response)
    {
        HeldError heldError = new HeldError();
        heldError.code = strHeldErrorCode;
        heldError.message = new HeldErrorMessage();
        heldError.message.Value = strHeldErrorMessage;
        heldError.message.lang = "en";
        await WriteHeldResponse(XmlHelper.SerializeToString(heldError), response);
    }

    private async Task WriteHeldResponse(string strResponse, HttpResponse response)
    {
        byte[] ResponseBytes = Encoding.UTF8.GetBytes(strResponse);
        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = ContentTypes.Held;
        response.ContentLength = ResponseBytes.Length;
        await response.BodyWriter.WriteAsync(ResponseBytes);
        await response.CompleteAsync();
    }

    private string? GetCallingPartyNumber(string strPath)
    {
        int index = strPath.LastIndexOf("/");
        if (index < 0)
            return null;

        if ((index + 1) >= strPath.Length)
            return null;

        return strPath.Substring(index + 1);
    }

    /// <summary>
    /// The path for an additional data request is /AdditionalData/CallingPartyNumber/InfoType
    /// For example: CallingPartyNumber = 8055551234, InfoType = SubscriberInfo.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPath"></param>
    /// <returns></returns>
    private async Task AdditionalDataRequestHandler(HttpContext context, string strPath)
    {
        // Parse the request path
        string[] fields = strPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (fields.Length < 3)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        string strCallingPartyNumber = fields[1];
        string strInfoType = fields[2];

        CallAdditionalData? callAdditionalData = m_AdditionalDataStore.GetCallAdditionalData(strCallingPartyNumber);
        if (callAdditionalData == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        switch (strInfoType)
        {
            case "Comments":
                await HandleComments(fields, callAdditionalData, context.Response);
                break;
            case "DeviceInfo":
                await HandleDeviceInfo(callAdditionalData.DeviceInfo, context.Response);
                break;
            case "Providers":
                await HandleProviders(fields, callAdditionalData, context.Response);
                break;
            case "ServiceInfo":
                await HandleServiceInfo(callAdditionalData.ServiceInfo, context.Response);
                break;
            case "SubscriberInfo":
                await HandleSubscriberInfo(callAdditionalData.SubscriberInfo, context.Response);
                break;
            case "AutomatedCrashNotification":
                await HandleAutomatedCrashNotification(callAdditionalData.AutomatedCrashNotification, context.Response);
                break;
            default:
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                break;
        }
    }

    private async Task HandleComments(string[] pathFields, CallAdditionalData callAdditionalData, 
        HttpResponse response)
    {
        if (pathFields.Length != 4)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        if (callAdditionalData.Comments.Count == 0)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        CommentType? comment = null;
        if (callAdditionalData.Comments.TryGetValue(pathFields[3], out comment) == false ||
            comment is null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(comment, ContentTypes.Comment, response);
    }

    private async Task HandleProviders(string[] pathFields, CallAdditionalData callAdditionalData,
        HttpResponse response)
    {
        if (pathFields.Length != 4)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        ProviderInfoType? provider = null;
        if (callAdditionalData.Providers.TryGetValue(pathFields[3], out provider) == false ||
            provider is null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(provider, ContentTypes.ProviderInfo, response);
    }

    private async Task HandleDeviceInfo(DeviceInfoType? device, HttpResponse response)
    {
        if (device == null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(device, ContentTypes.DeviceInfo, response);
    }

    private async Task HandleServiceInfo(ServiceInfoType? service, HttpResponse response)
    {
        if (service == null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(service, ContentTypes.ServiceInfo, response);
    }

    private async Task HandleAutomatedCrashNotification(AutomatedCrashNotificationType? AcnType, HttpResponse response)
    {
        if (AcnType == null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(AcnType, ContentTypes.Veds, response);
    }

    private async Task HandleSubscriberInfo(SubscriberInfoType? subscriber, HttpResponse response)
    {
        if (subscriber == null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        await WriteAdditionalDataResponse(subscriber, ContentTypes.SubscriberInfo, response);
    }

    private async Task WriteAdditionalDataResponse(object Obj, string strContentType, HttpResponse response)
    {
        byte[] bytes = XmlHelper.SerializeToByteArray(Obj);
        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = strContentType;
        response.ContentLength = bytes.Length;
        await response.BodyWriter.WriteAsync(bytes);
    }

    /// <summary>
    /// Disables connection based client certificate validation so the middleware can handle it instead.
    /// Or, custom validation can be handled here.
    /// </summary>
    /// <param name="certificate">The certificate used to authenticate the remote party.</param>
    /// <param name="chain">The chain of certificate authorities associated with the remote certificate.
    /// </param>
    /// <param name="errors">One or more errors associated with the remote certificate.</param>
    /// <returns>A Boolean value that determines whether the specified certificate is accepted for authentication.</returns>
    private bool DoMutualAuthentication(X509Certificate2 certificate, X509Chain chain, SslPolicyErrors
        errors)
    {
        return true;    // Accepts all client certificates
    }

}
