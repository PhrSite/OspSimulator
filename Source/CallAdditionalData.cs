/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallAdditionalData.cs                                   7 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Pidf;
using AdditionalData;
using Veds;

using System.Collections.Concurrent;

namespace OspSimulator;

/// <summary>
/// Container class for location and additional call data for a NG9-1-1 call.
/// </summary>
public class CallAdditionalData
{
    /// <summary>
    /// The calling party's phone number or SIP user name (if the call is a VoIP call) that identifies the source of the call.
    /// </summary>
    public string CallingPartyNumber { get; set; }

    /// <summary>
    /// Current location of the call if location data is available.
    /// </summary>
    public Presence? Location { get; set; } = null;

    /// <summary>
    /// Comments relating to the call or the caller. The key is a unique identifier for the comment.
    /// This dictionary will be empty if there are no comments.
    /// </summary>
    public ConcurrentDictionary<string, CommentType> Comments { get; set; } = new ConcurrentDictionary<string, CommentType>();

    /// <summary>
    /// Provider information for each data provider. The key is a unique identifier for the provider.
    /// This dictionary will be empty if there is no provider information available.
    /// </summary>
    public ConcurrentDictionary<string, ProviderInfoType> Providers { get; set; } = new ConcurrentDictionary<string, ProviderInfoType>();

    /// <summary>
    /// Contains information about the calling device.
    /// </summary>
    public DeviceInfoType? DeviceInfo { get; set; } = null;

    /// <summary>
    /// Contains information about the class and type of phone service for the call.
    /// </summary>
    public ServiceInfoType? ServiceInfo { get; set; } = null;

    /// <summary>
    /// Contains information about the telephone service subscriber.
    /// </summary>
    public SubscriberInfoType? SubscriberInfo { get; set; } = null;

    /// <summary>
    /// Contains information about a car crash from an automated crash notification system.
    /// </summary>
    public AutomatedCrashNotificationType? AutomatedCrashNotification { get; set; } = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="callingPartyNumber">The calling party's phone number or other identifier.</param>
    public CallAdditionalData(string callingPartyNumber)
    {
        CallingPartyNumber = callingPartyNumber;
    }
}
