/////////////////////////////////////////////////////////////////////////////////////
//  File:   OspOutgoingCall.cs                                      7 Dec 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using CameraCapture;
using Ng911Lib.Utilities;
using OspSimulator.Settings;
using Pidf;
using SipLib.Audio.Windows;
using SipLib.Body;
using SipLib.Core;
using SipLib.Logging;
using SipLib.Media;
using SipLib.Msrp;
using SipLib.RealTimeText;
using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.Transactions;
using SipLib.Video.Windows;

using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OspSimulator;

/// <summary>
/// Delegate type for the CallEstablished event of the OspOutgoingCall class.
/// </summary>
public delegate void CallEstablishedDelegate();

/// <summary>
/// Delegate type for the CallRequestTimedOut event of the OspOutgoingCall class.
/// </summary>
public delegate void CallRequestTimedOutDelegate();

/// <summary>
/// Delegate type for the CallEnded event of the OspOutgoing class.
/// </summary>
public delegate void CallEndedDelegate();

/// <summary>
/// Delegate type for the CallStatusUpdate event of the OspOutgoingCall class.
/// </summary>
/// <param name="status">New status code to display. Will be either "Trying" or "Ringing".</param>
public delegate void CallStatusUpdateDelegate(string status);

/// <summary>
/// Delegate type for the CallCancellationComplete event of the OspOutgoingCall class.
/// </summary>
public delegate void CallCancellationCompleteDelegate();

/// <summary>
/// Delegate type for the CallRejected event of the OspOutgoingCall class.
/// </summary>
/// <param name="status">SIP status code enumeration of the SIP response that was received.</param>
/// <param name="reason">Reason phrase from the SIP response that was received.</param>
public delegate void CallRejectedDelegate(SIPResponseStatusCodesEnum status, string? reason);

/// <summary>
/// Delegate type for the CallMediaAdded event of the OspOutgoingCall class.
/// </summary>
/// <param name="media">Media type that has been added. Set to one of the static values defined by the
/// MediaTypes class.</param>
public delegate void CallMediaAddedDelegate(string media);

/// <summary>
/// Delegate type for the ReInviteFailed event of the OspOutgoingCall class.
/// </summary>
/// <param name="errorMessage">Error message explaining why the re-INVITE request failed.</param>
public delegate void ReInviteFailedDeletate(string errorMessage);

/// <summary>
/// This class manages a single outgoing NG9-1-1 call. It is primarily a SIP User Agent Client (UAC) for the call.
/// It also acts as a User Agent Server (UAS) for any requests that may originate from the called party (for example, BYE) or
/// a re-INVITE to change the characteristics of the media session.
/// </summary>
public class OspOutgoingCall
{
    private OspCallParameters m_CallParameters;
    private SipTransport? m_SipTransport = null;
    private X509Certificate2 m_Certificate;
    private OspServer m_OspServer;
    private SIPURI m_OspServerUri;

    private OspOutgoingCallStateEnum m_CallState = OspOutgoingCallStateEnum.Idle;
    private string m_LocalTag = string.Empty;
    private string m_RemoteTag = string.Empty;
    private int m_LastCSeqNum = 0;
    private SIPRequest? m_InviteRequest = null;
    private SIPResponse? m_OKResponse = null;
    private string m_CallID = string.Empty;
    private ClientInviteTransaction? m_ClientInviteTransaction = null;
    private AdditionalDataStore m_AdditionalDataStore;
    private AppSettings m_AppSettings;
    private WindowsCameraCapture? m_CameraCapture;

    private SIPContactHeader? m_RemoteContactHeader = null;
    private SdpAnswerSettings m_SdpAnswerSettings;

    /// <summary>
    /// Contains a list of media types for the call. Each string value is equal to one of the media types
    /// defined by the MediaTypes class (for example, MediaTypes.Audio or MediaTypes.Video, etc.).
    /// </summary>
    public List<string> CallMediaTypes = new List<string>();

    /// <summary>
    /// This event is fired when the call is established, i.e., the called party responded with an 200 OK
    /// response to the outgoing INVITE request.
    /// </summary>
    public event CallEstablishedDelegate? CallEstablished = null;

    /// <summary>
    /// This event is fired if the called party did not respond to an INVITE request after multiple retransmissions
    /// of the INVITE request.
    /// </summary>
    public event CallRequestTimedOutDelegate? CallRequestTimedOut = null;

    /// <summary>
    /// This event is fired if a re-INVITE request sent by this UAC to add media to the call timed out.
    /// </summary>
    public event CallRequestTimedOutDelegate? ReInviteTimedOut = null;

    /// <summary>
    /// This event is fired if a re-INVITE request sent by this UAC to add media to the call failed or
    /// was rejected.
    /// </summary>
    public event ReInviteFailedDeletate? ReInviteFailed = null;

    /// <summary>
    /// This event is fired when the called party sends a BYE request or this UAC sends a BYE or a
    /// CANCEL request to end the call.
    /// </summary>
    public event CallEndedDelegate? CallEnded = null;

    /// <summary>
    /// This event is fired when this UAC receives an interim respons such as a 100 Trying or a 180 Ringing.
    /// </summary>
    public event CallStatusUpdateDelegate? CallStatusUpdate = null;

    /// <summary>
    /// This event is fired when the CANCEL transaction initiated by this UAC is completed.
    /// </summary>
    public event CallCancellationCompleteDelegate? CallCancellationComplete = null;

    /// <summary>
    /// This event is fired if the called party rejects the call with a SIP response status code or 400 or greater.
    /// </summary>
    public event CallRejectedDelegate? CallRejected = null;

    /// <summary>
    /// This event is fired when a new media type has been added to the call either by this UAC or by
    /// the called party.
    /// </summary>
    public event CallMediaAddedDelegate? CallMediaAdded = null;

    /// <summary>
    /// This event is fired when a complete video frame is received from the called party.
    /// </summary>
    public event FrameBitmapReadyDelegate? FrameBitmapReady = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="callParameters">Contains parameters for creating a new outgoing SIP call.</param>
    /// <param name="certificate">X.509 certificate to use for SIP over TLS and HTTPS servers.</param>
    /// <param name="additionalDataStore">Object for getting location and additional data for the call.</param>
    /// <param name="appSettings">Contains various application settings that determine how the outgoing
    /// call is created and handled.</param>
    /// <param name="ospServer">HTTP/HTTPS server for processing location requests via the HELD protocol
    /// or request for additional data about the call.</param>
    /// <param name="cameraCapture">For capturing and sending video frames.</param>
    /// <param name="windowsAudioIo">For sending and receiving audio samples.</param>
    public OspOutgoingCall(OspCallParameters callParameters, X509Certificate2 certificate, AdditionalDataStore additionalDataStore,
        AppSettings appSettings, OspServer ospServer, WindowsCameraCapture? cameraCapture,
        WindowsAudioIo windowsAudioIo)
    {
        m_Certificate = certificate;
        m_CallParameters = callParameters;
        m_AdditionalDataStore = additionalDataStore;
        m_AppSettings = appSettings;
        m_OspServer = ospServer;
        m_CameraCapture = cameraCapture;
        m_WindowsAudioIo = windowsAudioIo;

        SdpOfferSettings offerSettings = m_CallParameters.OfferSettings!;
        // Set up the SdpAnswerSettings to use if a re-INVITE is received for the purpose of adding
        // media to the call.
        m_SdpAnswerSettings = new SdpAnswerSettings(offerSettings.OfferAudioCodecs, offerSettings.OfferVideoCodecs,
            Program.AppName, offerSettings.Fingerprint, offerSettings.PortManager);
        m_SdpAnswerSettings.EnableAudio = true;
        m_SdpAnswerSettings.EnableVideo = true;
        m_SdpAnswerSettings.EnableRtt = true;
        m_SdpAnswerSettings.EnableMsrp = true;

        SIPSchemesEnum scheme = m_AppSettings.NetworkSettings.UseHttps == true ? SIPSchemesEnum.https :
            SIPSchemesEnum.http;
        m_OspServerUri = new SIPURI(scheme, m_OspServer.ServerEndPoint.Address, m_OspServer.ServerEndPoint.Port);
    }

    private SIPURI? m_FromSipUri = null;
    private Sdp? m_OfferSdp = null;
    private Sdp? m_AnsweredSdp = null;

    private IPEndPoint? m_LocalIpEndPoint = null;
    private IPAddress? m_LocalIpAddress = null;
    private IPEndPoint? m_RemoteIpEndPoint = null;

    /// <summary>
    /// Starts a new outgoing call.
    /// </summary>
    /// <returns>Returns true if successful or false if there are errors in the call parameters provided
    /// to this object.</returns>
    public bool StartCall()
    {
        if (m_CallParameters.ResolvedSipUri is null || m_CallParameters.ChannelSettings is null ||
            m_CallParameters.ToSipUri is null || m_CallParameters.RequestSipUri is null ||
            m_CallParameters.OfferSettings is null || string.IsNullOrEmpty(m_CallParameters.From) == true)
            return false;

        if (m_SipTransport != null)
            return true;    // Already started

        m_SipTransport = SipTransport.CreateFromRemoteSipUri(m_CallParameters.ResolvedSipUri, m_CallParameters.ChannelSettings, 
            m_Certificate);
        m_SipTransport.SipRequestReceived += OnSipRequestReceived;
        m_SipTransport.Start();

        // Build the INVITE request
        if (m_AppSettings.LastCallSettings.UseTelUri == false)
        {
            m_FromSipUri = m_SipTransport.SipChannel.SIPChannelContactURI.CopyOf();
            m_FromSipUri.User = m_CallParameters.From;
        }
        else
            m_FromSipUri = SIPURI.ParseSIPURI($"tel:+1{m_AppSettings.LastCallSettings.FromUser}");
            
        m_LocalIpEndPoint = m_SipTransport.SipChannel.SIPChannelContactURI.ToSIPEndPoint()!.GetIPEndPoint();
        m_LocalIpAddress = m_LocalIpEndPoint.Address;
        m_OfferSdp = Sdp.BuildOfferSdp(m_LocalIpAddress, m_CallParameters.OfferSettings, m_Certificate);

        SIPRequest invite = SIPRequest.CreateRequest(SIPMethodsEnum.INVITE, m_CallParameters.RequestSipUri,
            m_CallParameters.ToSipUri, m_CallParameters.ToSipUri.User, m_FromSipUri, m_CallParameters.From,
            m_SipTransport.SipChannel.SIPChannelContactURI);

        // Add the Call-Info headers for the NG9-1-1 emergency-CallId and emergency-IncidentId
        string ElementId = $"osp.{Program.AppName}";
        string EmergencyCallIdentifier = SipUtils.BuildEmergencyIdUrn("callid", ElementId);
        string EmergencyIncidentIdentifier = SipUtils.BuildEmergencyIdUrn("incidentid", ElementId);
        SipUtils.AddEmergencyIdUrnCallInfoHeader(invite, EmergencyCallIdentifier, "emergency-CallId");
        SipUtils.AddEmergencyIdUrnCallInfoHeader(invite, EmergencyIncidentIdentifier, "emergency-IncidentId");

        SipBodyBuilder bodyBuilder = new SipBodyBuilder();
        bodyBuilder.AddContent(SipLib.Body.ContentTypes.Sdp, m_OfferSdp.ToString(), null, null);

        // Get the additional data and location data for the call
        CallAdditionalData? callAddData = m_AdditionalDataStore.GetCallAdditionalData(m_CallParameters.From);
        if (callAddData != null)
        {
            AddLocationData(invite, bodyBuilder, callAddData.Location, m_LocalIpEndPoint, m_CallParameters.From);
            AddAdditionalData(invite, bodyBuilder, callAddData, m_LocalIpEndPoint, m_CallParameters.From);
        }

        bodyBuilder.AttachMessageBody(invite);

        m_CallState = OspOutgoingCallStateEnum.Calling;
        m_InviteRequest = invite;
        m_LocalTag = invite.Header.From!.FromTag!;
        m_LastCSeqNum = invite.Header.CSeq;
        m_CallID = invite.Header.CallId;

        m_ClientInviteTransaction = m_SipTransport.StartClientInvite(invite, m_CallParameters.ResolvedSipUri.ToSIPEndPoint()!.GetIPEndPoint(),
            OnInviteTransactionComplete, OnInviteResponseReceived);

        return true;
    }

    private void AddAdditionalData(SIPRequest invite, SipBodyBuilder bodyBuilder, CallAdditionalData additionalData,
        IPEndPoint localIPEndPoint, string strFrom)
    {
        AdditionalDataSettings settings = m_AppSettings.AddtionalDataSettings;
        if (settings.AdditionalDataByValue == true)
        {
            // ServiceInfo
            if (additionalData.ServiceInfo != null)
                AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.ServiceInfo), 
                    $"ServiceInfo_{strFrom}@{localIPEndPoint}", PurposeTypes.ServiceInfo, SipLib.Body.ContentTypes.ServiceInfo);

            // SubscriberInfo
            if (additionalData.SubscriberInfo != null)
                AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.SubscriberInfo),
                    $"SubscriberInfo_{strFrom}@{localIPEndPoint}", PurposeTypes.SubscriberInfo, SipLib.Body.ContentTypes.SubscriberInfo);

            // DeviceInfo
            if (additionalData.DeviceInfo != null)
                AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.DeviceInfo),
                    $"DeviceInfo_{strFrom}@{localIPEndPoint}", PurposeTypes.DeviceInfo, SipLib.Body.ContentTypes.DeviceInfo);

            // ProviderInfo
            if (additionalData.Providers.Count > 0)
            {
                foreach (string strProvider in additionalData.Providers.Keys)
                {
                    AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.Providers[strProvider]),
                        $"{strProvider}_{strFrom}@{localIPEndPoint}", PurposeTypes.ProviderInfo, SipLib.Body.ContentTypes.ProviderInfo);
                }
            }

            // Comments
            if (additionalData.Comments.Count > 0)
            {
                foreach (string strComment in additionalData.Comments.Keys)
                {
                    AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.Comments[strComment]),
                        $"{strComment}_{strFrom}@{localIPEndPoint}", PurposeTypes.Comment, SipLib.Body.ContentTypes.Comment);
                }
            }

            // Automated Crash Notification information (AACN)
            if (additionalData.AutomatedCrashNotification != null)
                AddAdditionalDataByValue(invite, bodyBuilder, XmlHelper.SerializeToString(additionalData.AutomatedCrashNotification),
                    $"AACN_{strFrom}@{localIPEndPoint}", PurposeTypes.Veds, Ng911Lib.Utilities.ContentTypes.Veds);
        }

        if (settings.AdditionalDataByReference == true)
        {
            string strAddDataUriBase = $"{m_OspServerUri.ToString()}/{OspServer.ADDITIONAL_DATA_ROOT}/{strFrom}/";

            // ServiceInfo
            if (additionalData.ServiceInfo != null)
                AddAdditionalDataByReference(invite, strAddDataUriBase + "ServiceInfo", PurposeTypes.ServiceInfo);

            // SubscriberInfo
            if (additionalData.SubscriberInfo != null)
                AddAdditionalDataByReference(invite, strAddDataUriBase + "SubscriberInfo", PurposeTypes.SubscriberInfo);

            // DeviceInfo
            if (additionalData.DeviceInfo != null)
                AddAdditionalDataByReference(invite, strAddDataUriBase + "DeviceInfo", PurposeTypes.DeviceInfo);

            // ProviderInfo
            if (additionalData.Providers.Count > 0)
            {
                foreach (string provider in additionalData.Providers.Keys)
                    AddAdditionalDataByReference(invite, $"{strAddDataUriBase}Providers/{provider}", PurposeTypes.ProviderInfo);
            }

            // Comments
            if (additionalData.Comments.Count > 0)
            {
                foreach (string comment in additionalData.Comments.Keys)
                    AddAdditionalDataByReference(invite, $"{strAddDataUriBase}Comments/{comment}", PurposeTypes.Comment);
            }

            // Automated Crash Notification information (AACN)
            if (additionalData.AutomatedCrashNotification != null)
                AddAdditionalDataByReference(invite, $"{strAddDataUriBase}AutomatedCrashNotification", PurposeTypes.Veds);
        }
    }

    private void AddAdditionalDataByReference(SIPRequest invite, string strUri, string purpose)
    {
        SIPCallInfoHeader callInfo = new SIPCallInfoHeader(SIPURI.ParseSIPURI(strUri), purpose);
        invite.Header.CallInfo.Add(callInfo);
    }

    private void AddAdditionalDataByValue(SIPRequest invite, SipBodyBuilder bodyBuilder, string
        contents, string contentId, string purpose, string contentType)
    {
        SIPURI cidUri = SIPURI.ParseSIPURI("cid:" + contentId);
        SIPCallInfoHeader callInfo = new SIPCallInfoHeader(cidUri, purpose);
        invite.Header.CallInfo.Add(callInfo);
        bodyBuilder.AddContent(contentType, contents, contentId, null);
    }

    private void AddLocationData(SIPRequest invite, SipBodyBuilder bodyBuilder, Presence? presence,
        IPEndPoint localIpEndPoint, string strFrom)
    {
        if (presence == null)
            return;     // No PIDF-LO location data is available for the call

        string strPresence = XmlHelper.SerializePidfToString(presence);

        LocationSettings locationSettings = m_AppSettings.LocationSettings;
        string contentId = $"{strFrom}@{localIpEndPoint.ToString()}";
        if (locationSettings.LocationByValue == true)
        {
            bodyBuilder.AddContent(SipLib.Body.ContentTypes.Pidf, strPresence, contentId, null);
            // Add a cid Geolocation header
            SIPURI cidUri = SIPURI.ParseSIPURI("cid:" + contentId);
            SIPGeolocationHeader geoHeader = new SIPGeolocationHeader(cidUri);
            invite.Header.Geolocation.Add(geoHeader);
        }

        if (locationSettings.LocationByReference == true)
        {
            string strHeldUri = $"{m_OspServerUri.ToString()}/{OspServer.LOCATION_PATH_ROOT}/{strFrom}";
            SIPGeolocationHeader heldHeader = new SIPGeolocationHeader(SIPURI.ParseSIPURI(strHeldUri));
            invite.Header.Geolocation.Add(heldHeader);
        }

        if (locationSettings.LocationByPresenceEvent == true)
        {
            SIPURI sipUri = m_SipTransport!.SipChannel.SIPChannelContactURI.CopyOf();
            sipUri.User = strFrom;
            SIPGeolocationHeader presHeader = new SIPGeolocationHeader(sipUri);
            invite.Header.Geolocation.Add(presHeader);
        }
    }

    private List<string> m_OfferNewMediaTypes = new List<string>();

    /// <summary>
    /// Sends a re-INVITE request to the called party to add new media to the call.
    /// </summary>
    /// <param name="newMediaTypesToAdd">Must contain a list of new media types to add to the call. Each
    /// string value must be equal to one of the media types defined by the MediaTypes class. The caller
    /// must ensure that each media type is not already available for the call.</param>
    public void SendReInviteToAddMedia(List<string> newMediaTypesToAdd)
    {
        if (m_SipTransport == null || m_InviteRequest == null || m_OKResponse == null ||
            m_CallParameters == null || m_CallParameters.OfferSettings == null || m_OfferSdp == null ||
            m_LocalIpAddress is null || m_RemoteIpEndPoint == null)
            return;

        m_OfferNewMediaTypes = newMediaTypesToAdd;

        int LastCSeqNumber = m_LastCSeqNum;
        SIPRequest reInvite = SipUtils.BuildInDialogRequest(SIPMethodsEnum.INVITE, m_SipTransport.SipChannel,
            false, m_InviteRequest!, m_LocalTag, m_RemoteTag, m_OKResponse, ref LastCSeqNumber);
        m_LastCSeqNum = LastCSeqNumber;

        // Must increment the version of the SDP session when modifying the session
        if (m_OfferSdp.Origin != null)
            m_OfferSdp.Origin.Version += 1;

        SdpOfferSettings offerSettings = m_CallParameters.OfferSettings;
        // Build a new SDP to offer based on the original SDP
        foreach (string mediaType in newMediaTypesToAdd)
        {
            switch (mediaType)
            {
                case MediaTypes.Audio:
                    MediaDescription audioMediaDescription = SdpUtils.CreateAudioMediaDescription(offerSettings.
                        PortManager.NextAudioPort, offerSettings.OfferAudioCodecs, offerSettings.RtpEncryptionType,
                        offerSettings.Fingerprint);
                    m_OfferSdp.Media.Add(audioMediaDescription);
                    break;
                case MediaTypes.Video:
                    MediaDescription videoMediaDescription = SdpUtils.CreateVideoMediaDescription(offerSettings.
                        PortManager.NextVideoPort, offerSettings.OfferVideoCodecs, offerSettings.RtpEncryptionType,
                        offerSettings.Fingerprint);
                    m_OfferSdp.Media.Add(videoMediaDescription);
                    break;
                case MediaTypes.RTT:
                    MediaDescription rttMediaDescription = SdpUtils.CreateRttMediaDescription(offerSettings.PortManager.NextRttPort);
                    if (offerSettings.RtpEncryptionType == RtpEncryptionEnum.SdesSrtp)
                        SdpUtils.AddSdesSrtpEncryption(rttMediaDescription);
                    else if (offerSettings.RtpEncryptionType == RtpEncryptionEnum.DtlsSrtp)
                        SdpUtils.AddDtlsSrtp(rttMediaDescription, offerSettings.Fingerprint);
                    m_OfferSdp.Media.Add(rttMediaDescription);
                    break;
                case MediaTypes.MSRP:
                    MediaDescription msrpMediaDescription = SdpUtils.CreateMsrpMediaDescription(m_LocalIpAddress, offerSettings.
                        PortManager.NextMsrpPort, offerSettings.UseTlsForMsrp, offerSettings.MsrpSetupType, m_Certificate, offerSettings.UserName);
                    m_OfferSdp.Media.Add(msrpMediaDescription);
                    break;
            } // end swith mediaType
        } // end foreach

        SipBodyBuilder bodyBuilder = new SipBodyBuilder();
        bodyBuilder.AddContent(SipLib.Body.ContentTypes.Sdp, m_OfferSdp.ToString(), null, null);
        bodyBuilder.AttachMessageBody(reInvite);

        m_SipTransport.StartClientInvite(reInvite, m_RemoteIpEndPoint!, ReInviteToAddMediaTransactionComplete, null);
    }

    private void ReInviteToAddMediaTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (sipResponse == null)
        {
            ReInviteTimedOut?.Invoke();
            return;
        }

        if (sipResponse.Status != SIPResponseStatusCodesEnum.Ok)
        {
            ReInviteFailed?.Invoke($"Re-INVITE was rejected with a status code of {sipResponse.StatusCode}");
            return;
        }

        Sdp? offeredSdp = sipRequest.GetSdpContents();
        Sdp? answeredSdp = sipResponse.GetSdpContents();
        if (offeredSdp == null)
        {
            ReInviteFailed?.Invoke("Failed to get the offered SDP for the re-INVITE request");
            return;
        }

        if (answeredSdp == null)
        {
            ReInviteFailed?.Invoke("Failed to get the answered SDP for the re-INVITE response");
            return;
        }

        MediaDescription? offeredMd = null;
        MediaDescription? answeredMd = null;

        foreach (string mediaType in m_OfferNewMediaTypes)
        {
            offeredMd = offeredSdp.GetMediaType(mediaType);
            answeredMd = answeredSdp.GetMediaType(mediaType);

            if (answeredMd == null)
            {   // Protocol error. Possible but not likely so just log an error message
                SipLogger.LogError("Failed to get the answered MediaDescription from the response to " +
                    $"a re-INVITE to IP address: {remoteEndPoint.Address.ToString()}");
                continue; 
            }

            if (offeredMd == null)
            {   // Protocol error. Possible but not likely so just log an error message
                SipLogger.LogError("Failed to get the offered MediaDescription from the " +
                    $"a re-INVITE request to IP address: {remoteEndPoint.Address.ToString()}");
                continue; 
            }

            if (answeredMd.Port == 0)
            {   // The remote endpoint rejected this media type
                continue;
            }

            CallMediaTypes.Add(mediaType);
            switch (mediaType)
            {
                case MediaTypes.Audio:
                    SetupAudioMedia(offeredSdp, offeredMd, answeredSdp, answeredMd);
                    break;
                case MediaTypes.Video:
                    SetupVideoMedia(offeredSdp, offeredMd, answeredSdp, answeredMd);
                    break;
                case MediaTypes.RTT:
                    SetupRttMedia(offeredSdp, offeredMd, answeredSdp, answeredMd);
                    break;
                case MediaTypes.MSRP:
                    SetupMsrpMedia(offeredSdp, offeredMd, answeredSdp, answeredMd, false);
                    break;
            } // end switch

            // Notify the application that a new media type has been added to the call
            CallMediaAdded?.Invoke(mediaType);
        } // end foreach
    }

    /// <summary>
    /// Ends the call by sending a BYE request if the call is currently on-line or by sending a
    /// CANCEL request it the called party has not answered the call yet.
    /// </summary>
    public void EndCall()
    {
        if (m_CallState == OspOutgoingCallStateEnum.OnLine)
        {   // Send a BYE request
            IPEndPoint remoteEndPoint = m_RemoteContactHeader!.ContactURI!.ToSIPEndPoint()!.GetIPEndPoint();
            SIPRequest ByeRequest = SipUtils.BuildByeRequest(m_InviteRequest!, m_SipTransport!.SipChannel,
                remoteEndPoint, false, m_LastCSeqNum, m_OKResponse!);
            m_SipTransport.StartClientNonInviteTransaction(ByeRequest, remoteEndPoint, ByeTransactionComplete, 1000);

        }
        else if (m_CallState == OspOutgoingCallStateEnum.Calling)
        {
            m_ClientInviteTransaction?.CancelInvite();
            CallEnded?.Invoke();
        }
    }

    private void OnInviteTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        m_ClientInviteTransaction = null;

        if (sipResponse == null)
        {
            if (Transaction.TerminationReason == TransactionTerminationReasonEnum.CancelledByClient)
                CallCancellationComplete?.Invoke();
            else
                CallRequestTimedOut?.Invoke();
        }
        else
        {
            if (sipResponse.Status == SIPResponseStatusCodesEnum.Ok)
            {
                m_OKResponse = sipResponse;
                m_RemoteContactHeader = m_OKResponse.Header.Contact?[0];
                m_RemoteTag = sipResponse.Header.To!.ToTag!;
                m_CallState = OspOutgoingCallStateEnum.OnLine;
                m_RemoteIpEndPoint = m_OKResponse?.Header.Contact?[0]?.ContactURI?.ToSIPEndPoint()?.GetIPEndPoint();
                m_AnsweredSdp = sipResponse.GetSdpContents();

                // Setup the media streams for the call.
                SetupCallMedia();
                foreach (RtpChannel rtpChannel in m_RtpChannels)
                    CallMediaTypes.Add(rtpChannel.MediaType);

                if (m_MsrpConnection != null)
                    CallMediaTypes.Add(MediaTypes.MSRP);

                CallEstablished?.Invoke();
            }
            else
            {   // The call was rejected for some reason or the INVITE request was terminated by the user.
                CallRejected?.Invoke(sipResponse.Status, sipResponse.ReasonPhrase);
            }
        }
    }

    private void SetupCallMedia()
    {
        Sdp? answeredSdp = m_OKResponse?.GetSdpContents();
        if (answeredSdp == null)
        {   // Protocol error. Not likely so just log an error message
            SipLogger.LogError("Failed to get the answered SDP from the OK response for the INVITE " +
                $"to: {m_RemoteIpEndPoint?.ToString()}");
            return;
        }

        Sdp? offeredSdp = m_InviteRequest?.GetSdpContents();
        if (offeredSdp == null)
        {   // Protocol error. Not likely so just log an error message
            SipLogger.LogError("Failed to get the offered SDP from the INVITE request to: " +
                $"{m_RemoteIpEndPoint?.ToString()}");
            return; 
        }

        MediaSettings mediaSettings = m_AppSettings.MediaSettings;

        foreach (MediaDescription answeredMediaDescription in answeredSdp.Media)
        {
            MediaDescription? offeredMediaDescription = offeredSdp.GetMediaType(answeredMediaDescription.MediaType);
            if (offeredMediaDescription == null)
            {   // Protocol error. Not likely so just log an error message
                SipLogger.LogError("Failed to find the offered MediaDescription from the INVITE request " +
                    $"to: {m_RemoteIpEndPoint?.ToString()} for media type = {answeredMediaDescription.MediaType}");
                continue;
            }

            if (answeredMediaDescription.Port == 0)
            {   // This media type was rejected by the called party
                continue;
            }

            switch (answeredMediaDescription.MediaType)
            {
                case MediaTypes.Audio:
                    SetupAudioMedia(offeredSdp, offeredMediaDescription, answeredSdp, answeredMediaDescription);
                    break;
                case MediaTypes.Video:
                    SetupVideoMedia(offeredSdp, offeredMediaDescription, answeredSdp, answeredMediaDescription);
                    break;
                case MediaTypes.RTT:
                    SetupRttMedia(offeredSdp, offeredMediaDescription, answeredSdp, answeredMediaDescription);
                    break;
                case MediaTypes.MSRP:
                    SetupMsrpMedia(offeredSdp, offeredMediaDescription, answeredSdp, answeredMediaDescription, false);
                    break;
            } // end switch
        } // end foreach
    }

    private List<RtpChannel> m_RtpChannels = new List<RtpChannel>();
    private AudioSource? m_AudioSource = null;
    private WindowsAudioIo m_WindowsAudioIo;
    private AudioDestination? m_AudioDestination = null;

    /// <summary>
    /// Gets the RtpChannel for audio media. The application should only call this method if the
    /// call has been answered.
    /// </summary>
    /// <returns>Returns the RtpChannel for audio media or null if the call has no audio media.</returns>
    public RtpChannel? GetAudioRtpChannel()
    {
        RtpChannel? rtpChannel = null;
        foreach (RtpChannel channel in m_RtpChannels)
        {
            if (channel.MediaType == MediaTypes.Audio)
            {
                rtpChannel = channel;
                break; 
            }
        }

        return rtpChannel;
    }

    private void SetupAudioMedia(Sdp offeredSdp, MediaDescription offeredMediaDescription,
        Sdp answeredSdp, MediaDescription answeredMediaDescription)
    {
        (RtpChannel? rtpChannel, string? Error) = RtpChannel.CreateFromSdp(false, offeredSdp,
            offeredMediaDescription, answeredSdp, answeredMediaDescription, true, Program.AppName);
        if (rtpChannel == null)
        {
            SipLogger.LogError($"Failed to create the RtpChannel for audio. Reason = {Error}");
            return; 
        }

        m_RtpChannels.Add(rtpChannel);

        IAudioEncoder? encoder = AudioMediaUtils.GetAudioEncoder(answeredMediaDescription);
        if (encoder == null)
        {
            SipLogger.LogError("Failed to create the audio encoder");
            return;
        }

        IAudioDecoder? decoder = AudioMediaUtils.GetAudioDecoder(answeredMediaDescription);
        if (decoder == null)
        {
            SipLogger.LogError("Failed to create the audio decoder");
            return;
        }

        m_AudioSource = new AudioSource(answeredMediaDescription, encoder, rtpChannel);
        SetAudioSampleSource(m_AudioSource);

        m_AudioDestination = new AudioDestination(answeredMediaDescription, decoder, rtpChannel,
            null, m_WindowsAudioIo.SampleRate);
        m_AudioDestination.SetDestinationHandler(m_WindowsAudioIo.AudioOutSamplesReady);
        
        rtpChannel.StartListening();
    }

    private void SetAudioSampleSource(AudioSource audioSource)
    {
        MediaSettings mediaSettings = m_AppSettings.MediaSettings;
        if (mediaSettings.UseRecordedAudio == false)
            audioSource.SetAudioSampleSource(m_WindowsAudioIo);
        else
        {
            AudioSampleData audioSampleData;
            if (mediaSettings.UseDefaultAudioRecording == true)
                audioSampleData = WindowsAudioUtils.ReadWaveFile(MediaSettings.DEFAULT_AUDIO_RECORDING_FILE);
            else
            {
                try
                {
                    audioSampleData = WindowsAudioUtils.ReadWaveFile(mediaSettings.AudioRecordingFilePath);
                }
                catch
                {
                    SipLogger.LogError($"Failed to read the wave file called: {mediaSettings.AudioRecordingFilePath}");
                    audioSampleData = WindowsAudioUtils.ReadWaveFile(MediaSettings.DEFAULT_AUDIO_RECORDING_FILE);
                }
            }
                
            FileAudioSource fileAudioSource = new FileAudioSource(audioSampleData, null!);
            audioSource.SetAudioSampleSource(fileAudioSource);
            fileAudioSource.Start();
        }
    }

    private VideoSender? m_VideoSender = null;

    /// <summary>
    /// For processing video that is being received from the called party.
    /// </summary>
    private VideoReceiver? m_VideoReceiver = null;

    private void SetupVideoMedia(Sdp offeredSdp, MediaDescription offeredMediaDescription,
        Sdp answeredSdp, MediaDescription answeredMediaDescription)
    {
        (RtpChannel? rtpChannel, string? Error) = RtpChannel.CreateFromSdp(false, offeredSdp,
            offeredMediaDescription, answeredSdp, answeredMediaDescription, false, Program.AppName);
        if (rtpChannel == null)
        {
            SipLogger.LogError($"Failed to create the RtpChannel for video. Reason = {Error}");
            return;
        }

        m_RtpChannels.Add(rtpChannel);

        m_VideoReceiver = new VideoReceiver(answeredMediaDescription, rtpChannel);
        m_VideoReceiver.FrameReady += OnFrameBitmapReady;
        m_VideoSender = new VideoSender(answeredMediaDescription, rtpChannel, m_AppSettings!.DeviceSettings!.VideoDevice!.
            DeviceFormat.Framerate);

        // If video is available, the m_CameraCapture object will not be null
        if (m_CameraCapture != null)
            m_CameraCapture.FrameReady += OnCameraCaptureFrameReady;

        rtpChannel.StartListening();
    }

    private bool m_HasMsrpMedia = false;
    private bool m_HasRttMedia = false;
    private RttReceiver? m_RttReceiver = null;
    private RttSender? m_RttSender = null;

    /// <summary>
    /// Contains text messages sent and received via the Real Time Text (RTT) protocol.
    /// </summary>
    public TextMessagesCollection RttMessages = new TextMessagesCollection(TextTypeEnum.RTT);

    /// <summary>
    /// Contains text messages sent and received via the Message Session Relay Protocol (MSRP).
    /// </summary>
    public TextMessagesCollection MsrpMessages = new TextMessagesCollection(TextTypeEnum.MSRP);

    /// <summary>
    /// Returns true if the call has Real Time Text (RTT) media.
    /// </summary>
    public bool CallHasRtt
    {
        get { return m_HasRttMedia; }
    }

    /// <summary>
    /// Returns true if the call has Message Session Relay Protocol (MSRP) media.
    /// </summary>
    public bool CallHasMsrp
    {
        get { return m_HasMsrpMedia; }
    }

    private void SetupRttMedia(Sdp offeredSdp, MediaDescription offeredMediaDescription,
        Sdp answeredSdp, MediaDescription answeredMediaDescription)
    {
        if (m_HasMsrpMedia == true)
            return;     // Allow only one type of text media for a call

        m_HasRttMedia = true;

        (RtpChannel? rtpChannel, string? Error) = RtpChannel.CreateFromSdp(false, offeredSdp,
            offeredMediaDescription, answeredSdp, answeredMediaDescription, false, Program.AppName);
        if (rtpChannel == null)
        {
            SipLogger.LogError($"Failed to create the RtpChannel for RTT. Reason = {Error}");
            return;
        }

        m_RtpChannels.Add(rtpChannel);

        RttParameters? rttParameters = RttParameters.FromMediaDescription(answeredMediaDescription);
        if (rttParameters == null)
        {
            SipLogger.LogError($"Failed to create a RttParameters object from the answered MediaDescription.");
            return;
        }

        string? source = m_InviteRequest?.Header.To?.ToURI?.User;
        if (string.IsNullOrEmpty(source) == true)
            source = "Called Party";

        m_RttReceiver = new RttReceiver(rttParameters, rtpChannel, source);
        m_RttReceiver.RttCharactersReceived += OnRttCharactersReceived;

        m_RttSender = new RttSender(rttParameters, rtpChannel.Send);
        m_RttSender.Start();
        rtpChannel.StartListening();
    }

    private void OnRttCharactersReceived(string RxChars, string Source)
    {
        RttMessages.AddReceivedMessage(Source, RxChars);
    }

    public void SendRttCharacters(string rttCharacters)
    {
        if (string.IsNullOrEmpty(rttCharacters) == true)
            return;

        if (m_RttSender != null)
        {
            m_RttSender.SendMessage(rttCharacters);
            RttMessages.AddSentMessage("Me", rttCharacters);
        }
    }

    private MsrpConnection? m_MsrpConnection = null;

    private void SetupMsrpMedia(Sdp offeredSdp, MediaDescription offeredMediaDescription,
        Sdp answeredSdp, MediaDescription answeredMediaDescription, bool isIncoming)
    {
        if (m_HasRttMedia == true)
            return;     // Only allow one type of text

        m_HasMsrpMedia = true;

        (MsrpConnection? msrpConnection, string? msrpError) = MsrpConnection.CreateFromSdp(offeredMediaDescription,
            answeredMediaDescription, isIncoming, m_Certificate!);
        if (msrpConnection != null)
        {
            m_MsrpConnection = msrpConnection;
            // Hook the MsrpConnection's events
            msrpConnection.MsrpMessageReceived += OnMsrpMessageReceived;
            msrpConnection.Start();
        }
        else
            SipLogger.LogError($"Failed to create the MsrpConnection. Reason = {msrpError}");
    }

    private void OnMsrpMessageReceived(string ContentType, byte[] Contents, string from)
    {
        string strContents = Encoding.UTF8.GetString(Contents);

        if (ContentType == "text/plain")
        {
            string? strUser = m_CallParameters.ToSipUri?.User;
            if (string.IsNullOrEmpty(strUser) == true)
                strUser = "Called Party";
            MsrpMessages.AddReceivedMessage(strUser, strContents);
        }
        else if (ContentType.ToLower() == "message/cpim")
        {
            CpimMessage? cpim = CpimMessage.ParseCpimBytes(Contents);
            if (cpim != null)
            {
                string? From = cpim.From?.URI?.User;
                if (From == null) From = "Called Party";
                if (cpim.Body != null)
                    strContents = Encoding.UTF8.GetString(cpim.Body);
                else
                    strContents = "Unknown";

                MsrpMessages.AddReceivedMessage(From, strContents);
            }
        }
    }

    public void SendMsrpText(string strMsrpText)
    {
        if (m_MsrpConnection == null)
            return;

        if (m_AppSettings.MediaSettings.UseMsrpCpim == true)
        {
            CpimMessage cpimMessage = new CpimMessage();
            SIPURI fromUri = m_SipTransport!.SipChannel.SIPChannelContactURI.CopyOf();
            fromUri.User = m_CallParameters.From;
            cpimMessage.From = new SIPUserField(m_CallParameters.From, fromUri, null);
            SIPURI toUri = m_CallParameters.ToSipUri!.CopyOf();
            cpimMessage.To.Add(new SIPUserField(toUri.User, toUri, null));
            cpimMessage.ContentType = "text/plain";
            cpimMessage.Body = Encoding.UTF8.GetBytes(strMsrpText);
            m_MsrpConnection.SendMsrpMessage("message/cpim", cpimMessage.ToByteArray());
        }
        else
            m_MsrpConnection.SendMsrpMessage("text/plain", Encoding.UTF8.GetBytes(strMsrpText));
        
        MsrpMessages.AddSentMessage("Me", strMsrpText);
    }

    private void OnCameraCaptureFrameReady(int Width, int Height, int fps, byte[] bytes, FFmpeg.AutoGen.AVPixelFormat pixelFormat)
    {
        if (m_VideoSender != null)
            m_VideoSender.SendVideoFrame(Width, Height, fps, bytes, pixelFormat);
    }

    private void OnFrameBitmapReady(Bitmap bitmap)
    {
        FrameBitmapReady?.Invoke(bitmap);
    }

    private void OnInviteResponseReceived(SIPResponse Response, IPEndPoint RemoteEndPoint, SipTransactionBase Transaction)
    {
        if (Response.Status == SIPResponseStatusCodesEnum.Trying)
            CallStatusUpdate?.Invoke("Trying");
        else if (Response.Status == SIPResponseStatusCodesEnum.Ringing)
            CallStatusUpdate?.Invoke("Ringing");
        else
        {

        }
    }

    private void OnSipRequestReceived(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        if (m_SipTransport == null)
            return;

        IPEndPoint remoteIpEndPoint = remoteEndPoint.GetIPEndPoint();
        if (sipRequest.Method == SIPMethodsEnum.BYE)
        {
            SIPResponse sipResponse = SipUtils.BuildOkToByeOrCancel(sipRequest, remoteEndPoint);
            sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint,
                ByeTransactionComplete, sipResponse);
            
        }
        else if (sipRequest.Method == SIPMethodsEnum.INVITE)
        {
            if (SipUtils.IsInDialog(sipRequest) == true && sipRequest.Header.To!.ToTag == m_LocalTag &&
                sipRequest.Header.From!.FromTag == m_RemoteTag && sipRequest.Header.CallId == m_CallID)
                // Its an in-dialog request that matches the current call.
                ProcessReInvite(sipRequest, remoteEndPoint, sipTransportManager);
            else
            {
                SIPResponse response;
                if (sipRequest.Header.CallId == m_CallID)
                    // Request is for the same Call-ID but the INVITE request is no in-dialog
                    response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist,
                        "Dialog Does Not Exist", sipTransportManager.SipChannel, Program.AppName);
                else
                    // The incoming call service is not supported.
                    response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.
                        ServiceUnavailable, "Service Not Available", sipTransportManager.SipChannel, Program.AppName);

                sipTransportManager.StartServerInviteTransaction(sipRequest, remoteIpEndPoint, null, response);
            }
        }
        else if (sipRequest.Method == SIPMethodsEnum.SUBSCRIBE)
        {   // Handle a SUBSCRIBE request for the SIP Presence Event package.
            if (string.IsNullOrEmpty(sipRequest.Header.Event) == true)
            {   // Error: No SUBSCRIBE/NOTIFY event package specified
                SIPResponse badRequest = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BadRequest,
                    "No Event Specified", sipTransportManager.SipChannel, Program.AppName);
                sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint, null, badRequest);
                return;
            }

            if (sipRequest.Header.Event == "presence")
                ProcessPresenceEventSubscribe(sipRequest, remoteEndPoint, sipTransportManager);
            else
            {   // Error: Unknown event package
                SIPResponse badEvent = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BadEvent,
                    "Bad Event", sipTransportManager.SipChannel, Program.AppName);
                sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint, null, badEvent);
            }
        }
    }

    // Handles a re-INVITE request from the called party.
    private void ProcessReInvite(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        if (m_OfferSdp == null || m_AnsweredSdp == null)
        {
            SipLogger.LogError("Call SDP media sessions not valid");
            return;
        }

        Sdp? offeredSdp = sipRequest.GetSdpContents();

        if (offeredSdp == null)
        {   // Must have an SDP body for an incoming re-INVITE
            SIPResponse response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BadRequest, "No SDP",
                sipTransportManager.SipChannel, Program.AppName);
            sipTransportManager.StartServerInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, response);
            return;
        }

        if (offeredSdp.Media.Count < m_OfferSdp!.Media.Count)
        {   // Error: On a re-INVITE, the offered media count must be greater than or equal than the media count
            // of the existing call
            SIPResponse response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BadRequest, "Bad Request - Media Count Mismatch",
                sipTransportManager.SipChannel, Program.AppName);
            sipTransportManager.StartServerInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, response);
            return;
        }

        m_RemoteContactHeader = sipRequest.Header.Contact?[0];
        m_RemoteIpEndPoint = m_RemoteContactHeader?.ContactURI?.ToSIPEndPoint()?.GetIPEndPoint();

        (Sdp? answerSdp, List<string> modifiedMediaList) = BuildReInviteAnswerSdp(offeredSdp, sipTransportManager);
        if (answerSdp == null)
        {
            SipLogger.LogError("Failed to build the answer SDP");
            return;
        }

        SIPResponse OkResponse = SipUtils.BuildOkToInvite(sipRequest, sipTransportManager.SipChannel, answerSdp.ToString(),
            SipLib.Body.ContentTypes.Sdp);
        OkResponse.Header.To!.ToTag = m_LocalTag;    // Fix the local tag
        sipTransportManager.StartServerInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);

        // Handle MsrpConnection and RtpChannel objects for the call that either need to be modified 
        // or created and added to the call.
        foreach (string mediaType in modifiedMediaList)
        {
            MediaDescription? answerMd = answerSdp.GetMediaType(mediaType);
            MediaDescription? offeredMd = offeredSdp.GetMediaType(mediaType);
            if (answerMd == null)
            {
                SipLogger.LogError($"Failed to get the answer MediaDescription for media type = {mediaType}");
                return;
            }

            if (offeredMd == null)
            {
                SipLogger.LogError($"Failed to get the offered MediaDescription for media type = {mediaType}");
                return;
            }

            if (mediaType == MediaTypes.MSRP)
            {
                bool MsrpAdded = m_MsrpConnection != null ? true : false;
                SetupMsrpConnectionForReInvite(offeredSdp, offeredMd, answerSdp, answerMd);
                if (MsrpAdded == true)
                {
                    CallMediaTypes.Add(mediaType);
                    CallMediaAdded?.Invoke(mediaType);
                }
            }
            else
            {
                RtpChannel? rtpChannel = GetRtpChannelForMediaType(mediaType);
                bool mediaAdded = rtpChannel == null ? true : false;
                CreateNewRtpChannelForReInvite(offeredSdp, offeredMd, answerSdp, answerMd, mediaAdded);
                if (mediaAdded == true)
                {
                    CallMediaTypes.Add(mediaType);
                    CallMediaAdded?.Invoke(mediaType);
                }
            }

            m_OfferSdp = answerSdp;
            m_AnsweredSdp = offeredSdp;
        }

    }

    private (Sdp?, List<string>) BuildReInviteAnswerSdp(Sdp OfferedSdp, SipTransport sipTransportManager)
    {
        List<string> modifiedMediaList = new List<string>();
        IPAddress address = sipTransportManager.SipChannel.SIPChannelEndPoint.Address!;

        if (m_OfferSdp == null || m_AnsweredSdp == null)
            return (null, modifiedMediaList);

        Sdp answerSdp = new Sdp(sipTransportManager.SipChannel.SIPChannelEndPoint.Address!, m_SdpAnswerSettings.UserName);

        // Compare the new offered SDP to the last provided SDP from the remote endpoint
        foreach (MediaDescription offeredMd in OfferedSdp.Media)
        {
            MediaDescription? lastMd = m_AnsweredSdp.GetMediaType(offeredMd.MediaType);
            if (lastMd != null)
            {   // This media type is part of the current call

                MediaDescription? localMd = m_OfferSdp.GetMediaType(offeredMd.MediaType);
                if (localMd == null)
                {
                    // TODO: log this error
                    return (null, modifiedMediaList);
                }

                if (MediaDescription.AreEqual(m_AnsweredSdp, lastMd, OfferedSdp, offeredMd) == true)
                {   // No changes to the media session are being offered. Answer with the last offered
                    // (i.e. the local) SDP media description.
                    answerSdp.Media.Add(localMd);
                }
                else
                {   // Changes to the media session are being offered
                    modifiedMediaList.Add(offeredMd.MediaType);
                    answerSdp.Media.Add(BuildReInviteAnswerMediaDescription(offeredMd, address));
                }
            }
            else
            {   // This media type is being added to the current call
                modifiedMediaList.Add(offeredMd.MediaType);
                answerSdp.Media.Add(BuildReInviteAnswerMediaDescription(offeredMd, address));
            }
        }

        return (answerSdp, modifiedMediaList);
    }

    private MediaDescription BuildReInviteAnswerMediaDescription(MediaDescription offeredMd, IPAddress address)
    {
        MediaDescription answerMd = new MediaDescription(offeredMd.MediaType, 0, offeredMd.PayloadTypes);
        switch (offeredMd.MediaType)
        {
            case MediaTypes.Audio:
                answerMd = Sdp.BuildAudioAnswerMediaDescription(offeredMd, m_SdpAnswerSettings, 
                    GetLocalRtpPortForMediaType(offeredMd.MediaType));
                break;
            case MediaTypes.Video:
                answerMd = Sdp.BuildVideoAnswerMediaDescription(offeredMd, m_SdpAnswerSettings,
                    GetLocalRtpPortForMediaType(offeredMd.MediaType));
                break;
            case MediaTypes.RTT:
                answerMd = Sdp.BuildRttAnswerMediaDescription(offeredMd, m_SdpAnswerSettings,
                    GetLocalRtpPortForMediaType(offeredMd.MediaType));
                break;
            case MediaTypes.MSRP:
                answerMd = Sdp.BuildMsrpAnswerMediaDescription(offeredMd, address, m_SdpAnswerSettings, null);
                break;
        } // end swith

        return answerMd;
    }

    private void SetupMsrpConnectionForReInvite(Sdp offeredSdp, MediaDescription OfferedMd, Sdp answeredSdp, MediaDescription Answeredmd)
    {
        if (m_MsrpConnection != null)
        {   // The existing MsrpConnection is being modified by the incoming re-INVITE request. This
            // requires tearing down the existing connection and creating a new one.
            m_MsrpConnection.MsrpMessageReceived -= OnMsrpMessageReceived;
            m_MsrpConnection.Shutdown();
            m_MsrpConnection = null;
        }
        else
        {
            m_HasMsrpMedia = true;
            CallMediaAdded?.Invoke(MediaTypes.MSRP);
        }

        SetupMsrpMedia(offeredSdp, OfferedMd, answeredSdp, Answeredmd, true);
    }

    private void CreateNewRtpChannelForReInvite(Sdp OfferedSdp, MediaDescription OfferedMd, Sdp AnsweredSdp,
        MediaDescription AnsweredMd, bool Add)
    {
        (RtpChannel? rtpChannel, string? Error) = RtpChannel.CreateFromSdp(true, OfferedSdp, OfferedMd, AnsweredSdp,
            AnsweredMd, true, null);
        if (rtpChannel == null)
        {
            SipLogger.LogError($"Failed to create an RtpChannel for a re-INVITE. Call-ID = {m_CallID}, " +
                $"MediaType = {OfferedMd.MediaType}, Error = {Error}");
            return;
        }

        if (Add == true)
        {
            m_RtpChannels.Add(rtpChannel);
        }
        else
        {   // Replace the existing RtpChannel.
            for (int i = 0; i < m_RtpChannels.Count; i++)
            {
                if (m_RtpChannels[i].MediaType == AnsweredMd.MediaType)
                {
                    m_RtpChannels[i].Shutdown();
                    //call.UnHookRtpEvents(call.RtpChannels[i]);
                    m_RtpChannels[i] = rtpChannel;
                }
            }
        }

        switch (AnsweredMd.MediaType)
        {
            case MediaTypes.Audio:
                SetupAudioForIncomingReInvite(rtpChannel, AnsweredMd);
                break;
            case MediaTypes.Video:
                SetupVideoForIncomingReInvite(rtpChannel, AnsweredMd);
                break;
            case MediaTypes.RTT:
                SetupRttForIncomingReInvite(rtpChannel, AnsweredMd);
                break;
        }

        rtpChannel.StartListening();
    }

    private void SetupRttForIncomingReInvite(RtpChannel rtpChannel, MediaDescription AnsweredMd)
    {
        if (m_RttReceiver != null)
        {
            m_RttReceiver.RttCharactersReceived -= OnRttCharactersReceived;
            m_RttReceiver = null;
        }

        if (m_RttSender != null)
        {
            m_RttSender.Stop();
            m_RttSender = null;
        }

        m_HasRttMedia = true;
        RttParameters? rttParameters = RttParameters.FromMediaDescription(AnsweredMd);
        if (rttParameters == null)
        {
            SipLogger.LogError($"Failed to create a RttParameters object from the answered MediaDescription.");
            return;
        }

        string? source = m_InviteRequest?.Header.To?.ToURI?.User;
        if (string.IsNullOrEmpty(source) == true)
            source = "Called Party";

        m_RttReceiver = new RttReceiver(rttParameters, rtpChannel, source);
        m_RttReceiver.RttCharactersReceived += OnRttCharactersReceived;

        m_RttSender = new RttSender(rttParameters, rtpChannel.Send);
        m_RttSender.Start();
    }

    private void SetupVideoForIncomingReInvite(RtpChannel rtpChannel, MediaDescription AnsweredMd)
    {
        if (m_VideoReceiver != null)
        {
            m_VideoReceiver.Shutdown();
            m_VideoReceiver.FrameReady -= OnFrameBitmapReady;
            m_VideoReceiver = null;
        }

        if (m_VideoSender != null)
        {
            m_VideoSender.Shutdown();
            m_VideoSender = null;
        }

        m_VideoReceiver = new VideoReceiver(AnsweredMd, rtpChannel);
        m_VideoReceiver.FrameReady += OnFrameBitmapReady;
        m_VideoSender = new VideoSender(AnsweredMd, rtpChannel, m_AppSettings!.DeviceSettings!.VideoDevice!.
            DeviceFormat.Framerate);
       
        // If video is available, the m_CameraCapture object will not be null
        if (m_CameraCapture != null)
            m_CameraCapture!.FrameReady += OnCameraCaptureFrameReady;

    }

    private void SetupAudioForIncomingReInvite(RtpChannel rtpChannel, MediaDescription AnsweredMd)
    {
        if (m_AudioSource != null)
            m_AudioSource.Stop();

        if (m_AudioDestination != null)
            m_AudioDestination.SetDestinationHandler(null);

        IAudioEncoder? encoder = AudioMediaUtils.GetAudioEncoder(AnsweredMd);
        if (encoder == null)
        {
            SipLogger.LogError("Failed to get the audio encoder for an incoming re-INVITE");
            return;
        }

        IAudioDecoder? decoder = AudioMediaUtils.GetAudioDecoder(AnsweredMd);
        if (decoder == null)
        {
            SipLogger.LogError("Failed to get the audio decoder for an incoming re-INVITE");
            return;
        }

        m_AudioSource = new AudioSource(AnsweredMd, encoder, rtpChannel);
        SetAudioSampleSource(m_AudioSource);
        m_AudioSource.Start();

        m_AudioDestination = new AudioDestination(AnsweredMd, decoder, rtpChannel,
            null, m_WindowsAudioIo.SampleRate);
        m_AudioDestination.SetDestinationHandler(m_WindowsAudioIo.AudioOutSamplesReady);
    }

    /// <summary>
    /// Gets the RtpChannel for a specified media type
    /// </summary>
    /// <param name="mediaType">Media type to search for</param>
    /// <returns>Returns the RtpChannel if found or null if there is no RtpChannel for the specified media type.</returns>
    private RtpChannel? GetRtpChannelForMediaType(string mediaType)
    {
        foreach (RtpChannel rtpChannel in m_RtpChannels)
        {
            if (rtpChannel.MediaType == mediaType)
                return rtpChannel;
        }

        return null;
    }

    /// <summary>
    /// Gets the local port used by a RtpChannel for a specified media type. 
    /// </summary>
    /// <param name="mediaType">Media type to search for</param>
    /// <returns>Returns the local port if the media type exists for the call or 0 if it does not</returns>
    private int GetLocalRtpPortForMediaType(string mediaType)
    {
        RtpChannel? rtpChannel = GetRtpChannelForMediaType(mediaType);
        if (rtpChannel != null)
            return rtpChannel.LocalPort;
        else
            return 0;
    }

    /// <summary>
    /// Gets the local MsrpUri if there is MSRP media for this call or null if there is no MSRP.
    /// </summary>
    /// <returns></returns>
    private MsrpUri? GetLocalMsrpUri()
    {
        return m_MsrpConnection?.LocalMsrpUri;
    }


    private void ProcessPresenceEventSubscribe(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        IPEndPoint remoteIpEndPoint = remoteEndPoint.GetIPEndPoint();
        string? CallingPartyNumber = sipRequest.URI?.User;
        if (string.IsNullOrEmpty(CallingPartyNumber) == true)
        {
            SIPResponse notFound = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.NotFound,
                "Not Found", sipTransportManager.SipChannel, Program.AppName);
            sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint, null, notFound);
            return;
        }

        CallAdditionalData? additionalData = m_AdditionalDataStore.GetCallAdditionalData(CallingPartyNumber);
        if (additionalData == null || additionalData.Location == null)
        {   // Location data not found for the calling party number
            SIPResponse notFound = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.NotFound,
                "Not Found", sipTransportManager.SipChannel, Program.AppName);
            sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint, null, notFound);
            return;
        }

        SIPResponse OkResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok,
            "OK", sipTransportManager.SipChannel, Program.AppName);
        OkResponse.Header.To!.ToTag = CallProperties.CreateNewTag();
        OkResponse.Header.Expires = sipRequest.Header.Expires;
        sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteIpEndPoint, null, OkResponse);

        // Build a NOTIFY request to send the location.
        SIPRequest notify = SIPRequest.CreateBasicRequest(SIPMethodsEnum.NOTIFY,
            sipRequest.Header.Contact![0].ContactURI!, sipRequest.Header.Contact![0].ContactURI!,
            sipRequest.Header.Contact!.FirstOrDefault()!.ContactName, sipTransportManager.SipChannel.SIPChannelContactURI,
            null);

        // The NOTIFY request must be in-dialog with the dialog that the SUBSCRIBE request created
        notify.Header.CallId = sipRequest.Header.CallId;
        notify.Header.To!.ToTag = sipRequest.Header.From!.FromTag;
        notify.Header.From!.FromTag = OkResponse.Header.To!.ToTag;

        notify.Header.Event = sipRequest.Header.Event;
        notify.Header.Expires = OkResponse.Header.Expires;
        notify.Header.SubscriptionState = "active";

        // Attach the PIDF-LO presence object to the NOTIFY request.
        SipBodyBuilder bodyBuilder = new SipBodyBuilder();
        bodyBuilder.AddContent(SipLib.Body.ContentTypes.Pidf, XmlHelper.SerializePidfToString(additionalData.Location),
            null, null);
        bodyBuilder.AttachMessageBody(notify);

        sipTransportManager.StartClientNonInviteTransaction(notify, remoteIpEndPoint, NotifyTransactionComplete,
            1000);
    }

    private void NotifyTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (sipResponse == null)
            SipLogger.LogError($"NOTIFY request timed out from IP address = {remoteEndPoint.Address.ToString()}");
    }

    private void ByeTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        // Assume success
        CallEnded?.Invoke();
    }

    /// <summary>
    /// Shuts down this SIP UAC. Do not attempt to use this object after calling this function.
    /// </summary>
    public void Shutdown()
    {
        if (m_SipTransport != null)
        {
            m_SipTransport.Shutdown();
            m_SipTransport.SipRequestReceived -= OnSipRequestReceived;
            m_SipTransport = null;
        }

        // Shut down the media sources and destinations.
        foreach (RtpChannel rtpChannel in m_RtpChannels)
        {
            rtpChannel.Shutdown();
        }

        m_RtpChannels.Clear();

        if (m_AudioSource != null)
        {
            m_AudioSource.Stop();
            m_AudioSource = null;
        }

        if (m_AudioDestination != null)
        {
            // Nothing needs to be done
        }

        if (m_VideoSender != null)
        {
            m_VideoSender.Shutdown();
            m_VideoSender = null;
        }

        if (m_VideoReceiver != null)
        {
            m_VideoReceiver.Shutdown();
            m_VideoReceiver.FrameReady -= OnFrameBitmapReady;
            m_VideoReceiver = null;
        }

        if (m_RttSender != null)
        {
            m_RttSender.Stop();
            m_RttSender = null;
        }

        if (m_RttReceiver != null)
        {
            m_RttReceiver.RttCharactersReceived -= OnRttCharactersReceived;
            m_RttReceiver = null;
        }

        if (m_MsrpConnection != null)
        {
            m_MsrpConnection.MsrpMessageReceived -= OnMsrpMessageReceived;
            m_MsrpConnection.Shutdown();
            m_MsrpConnection = null;
        }
    }
}
