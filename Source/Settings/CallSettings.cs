/////////////////////////////////////////////////////////////////////////////////////
//  File: CallSettings.cs                                       20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace OspSimulator.Settings;

/// <summary>
/// Contains the settings that were used for the most recent call request that was sent.
/// </summary>
public class CallSettings
{
    /// <summary>
    /// Contains the last "To" SIP URI entered by the user. The To SIP URI is used to call the remote party.
    /// </summary>
    public string SipToUri { get; set; } = string.Empty;

    /// <summary>
    /// Contains the last From Number entered by the user. The From Number is the user part of the local SIP URI
    /// in the From header of the outgoing INVITE request. This corresponds to the Calling Party Number.
    /// </summary>
    public string FromUser { get; set; } = string.Empty;

    /// <summary>
    /// If true, then the application uses a tel URI in the SIP From header of the outgoing call. The default
    /// setting is false.
    /// </summary>
    public bool UseTelUri { get; set; } = false;

    /// <summary>
    /// If true, then the request URI of the outgoing INVITE request shall be set to urn:service:sos. If false then the
    /// request URI of the outgoing INVITE request will be set to the To SIP URI.
    /// </summary>
    public bool UseUrnServiceSos { get; set; } = false;

    /// <summary>
    /// This setting is applied if the application needs to do a DNS host name lookup for the remote host and the DNS
    /// server returns an IPv6 address for the host.
    /// </summary>
    public bool PreferIPv6 { get; set; } = false;

    /// <summary>
    /// Constructor
    /// </summary>
    public CallSettings()
    {
    }
}
