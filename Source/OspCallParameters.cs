/////////////////////////////////////////////////////////////////////////////////////
//  File:   OspCallParameters.cs                                    8 Dec 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Channels;
using SipLib.Sdp;

namespace OspSimulator;

/// <summary>
/// This class contains parameters for creating an OspOutgoing call.
/// </summary>
public class OspCallParameters
{
    /// <summary>
    /// This object is a SIPURI that has the host portion resolved to an IP address and port number. This will be
    /// the same as the ToSipUri if the ToSipUri host portion is an IPEndPoint.
    /// </summary>
    public SIPURI? ResolvedSipUri { get; set; }

    /// <summary>
    /// This is the parsed SIP URI from the To SIP URI input field provided by the user in the application's main form.
    /// The host portion of this SIPURI may or may not be an IPEndPoint.
    /// </summary>
    public SIPURI? ToSipUri {  get; set; }

    /// <summary>
    /// Specifies the SIPURI to use in the request line of the outgoing INVITE request.
    /// </summary>
    public SIPURI? RequestSipUri { get; set; }
    
    /// <summary>
    /// Contains parameters for building a SIPChannel derived class.
    /// </summary>
    public SipChannelSettings? ChannelSettings { get; set; }

    /// <summary>
    /// Setting to use for building the SDP to send with the outgoing INVITE request.
    /// </summary>
    public SdpOfferSettings? OfferSettings { get; set; }

    /// <summary>
    /// This contains the user part From SIP header SIPURI that was entered by the user in the application's main
    /// form.
    /// </summary>
    public string? From { get; set; }
}
