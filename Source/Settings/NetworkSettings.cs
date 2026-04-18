/////////////////////////////////////////////////////////////////////////////////////
//  File:   NetworkSettings.cs                                      20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace OspSimulator.Settings;

using SipLib.Media;

/// <summary>
/// Network settings for the OspServer class and the SIP interface
/// </summary>
public class NetworkSettings
{
    /// <summary>
    /// If true, then the application will use the IPv4 transport protocol
    /// </summary>
    public bool EnableIPv4 { get; set; } = true;

    /// <summary>
    /// If true, then the application will use the IPv6 transport protocol
    /// </summary>
    public bool EnableIPv6 { get; set; } = true;

    /// <summary>
    /// The default will be the last selected IP address for the IPv4 network if it’s available, or the
    /// first IP address in the list of available IP addresses for the IPv4 network.
    /// </summary>
    public string IPv4Address { get; set; } = string.Empty;

    /// <summary>
    /// The default will be the last selected IP address for the IPv6 network if it’s available, or the
    /// first IP address in the list of available IP addresses for the IPv6 network.
    /// </summary>
    public string IPv6Address { get; set; } = string.Empty;

    /// <summary>
    /// Applies to SIP over TLS. If true, then the application shall offer its X.509 certificate when 
    /// it attempts to connect to the remote host using SIP over TLS.
    /// </summary>
    public bool UseMutualAuthentication { get; set; } = false;

    /// <summary>
    /// If true, then the application shall listen on HTTPS for its HELD and additional data interfaces. 
    /// If false, then the application will listen on HTTP. 
    /// </summary>
    public bool UseHttps { get; set; } = false;

    /// <summary>
    /// If true, then use IPv4 for the HTTP interfaces, else use IPv6 for the HTTP interfaces.
    /// </summary>
    public bool UseIPv4ForHttp { get; set; } = true;

    /// <summary>
    /// Specifies the TCP port number to use for HTTP interfaces.
    /// </summary>
    public int HttpPortNumber { get; set; } = 11000;

    /// <summary>
    /// Specifies the TCP port number to use for HTTPS interfaces.
    /// </summary>
    public int HttpsPortNumber { get; set; } = 11001;

    /// <summary>
    /// SIP port for UDP and TCP.
    /// </summary>
    public int LocalSipPortNumber { get; set; } = 5060;

    /// <summary>
    /// SIP port for Transport Layer Security (TLS).
    /// </summary>
    public int LocalSipsPortNumber { get; set; } = 5061;

    /// <summary>
    /// Contains the port ranges for each type of media
    /// </summary>
    public MediaPortSettings MediaPorts { get; set; } = new MediaPortSettings();

    /// <summary>
    /// Constructor
    /// </summary>
    public NetworkSettings()
    {
        MediaPorts.AudioPorts.Count = 100;
        MediaPorts.VideoPorts.Count = 100;
        MediaPorts.RttPorts.Count = 100;
        MediaPorts.MsrpPorts.Count = 100;
    }

}
