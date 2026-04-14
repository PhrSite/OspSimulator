/////////////////////////////////////////////////////////////////////////////////////
//  File:   AdditionalDataSettings.cs                               20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace OspSimulator.Settings;

/// <summary>
/// This class contains settings that determine how the application will deliver additional call data
/// for the outgoing call.
/// </summary>
public class AdditionalDataSettings
{
    /// <summary>
    /// If true, then additional data will be delivered by-value in the body of the SIP INVITE request that is 
    /// sent. See Section 6 of RFC 7852.
    /// </summary>
    public bool AdditionalDataByValue { get; set; } = false;

    /// <summary>
    /// If true, then additional data will be delivered by-reference in the SIP INVITE request. 
    /// See Section 6 of RFC 7852.
    /// </summary>
    public bool AdditionalDataByReference { get; set; } = true;

    /// <summary>
    /// Constructor
    /// </summary>
    public AdditionalDataSettings()
    {
    }
}
