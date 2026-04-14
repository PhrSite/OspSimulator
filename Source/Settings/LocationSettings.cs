/////////////////////////////////////////////////////////////////////////////////////
//  File:   LocationSettings.cs                                     20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace OspSimulator.Settings;

/// <summary>
/// Class for storing settings that determine how call location is delivered.
/// </summary>
public class LocationSettings
{
    /// <summary>
    /// If true, then location data will be delivered in the body of the SIP INVITE request that is sent as
    /// specified in Section 3.1 of RFC 6442.
    /// </summary>
    public bool LocationByValue { get; set; } = true;

    /// <summary>
    /// If true, then location data will delivered by-reference as specified in Section 3.2 of RFC 6442.
    /// </summary>
    public bool LocationByReference { get; set; } = false;

    /// <summary>
    /// If true, then location data is delivered by the SIP Presence Event Package as specified by Section 
    /// 3.3 of RFC 6442 and RFC 3856.
    /// </summary>
    public bool LocationByPresenceEvent { get; set; } = false;

    /// <summary>
    /// Constructor
    /// </summary>
    public LocationSettings()
    {
    }
}
