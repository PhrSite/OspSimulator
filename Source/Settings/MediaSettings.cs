/////////////////////////////////////////////////////////////////////////////////////
//  File:   MediaSettings.cs                                        20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Media;

namespace OspSimulator.Settings;

/// <summary>
/// This class contains settings that determine how the application will offer media for the outgoing call.
/// </summary>
public class MediaSettings
{
    /// <summary>
    /// If true, then audio media will be offered in the initial INVITE message.
    /// </summary>
    public bool OfferAudio { get; set; } = true;

    /// <summary>
    /// If true, then video media will be offered in the initial INVITE message.
    /// </summary>
    public bool OfferVideo { get; set; } = false;

    /// <summary>
    /// If true, then RTT media will be offered in the initial INVITE message.
    /// </summary>
    public bool OfferRtt { get; set; } = false;

    /// <summary>
    /// Not used. Perhaps in a future implementation.
    /// </summary>
    public bool OfferRttConferenceAware { get; set; } = false;

    /// <summary>
    /// If true, then MSRP media will be offered in the intial INVITE message.
    /// </summary>
    public bool OfferMsrp { get; set; } = false;

    /// <summary>
    /// If true then the application will always send MSRP text messages using the message/cpim content type.
    /// Else, it will use text/plain.
    /// </summary>
    public bool UseMsrpCpim { get; set; } = false;

    /// <summary>
    /// Specifies the type of encryption to offer for RTP type media (audio, video, RTT).
    /// </summary>
    public RtpEncryptionEnum RtpEncryption { get; set; } = RtpEncryptionEnum.None;

    /// <summary>
    /// Specifies the type of encryption to offer the for MSRP media
    /// </summary>
    public MsrpEncryptionEnum MsrpEncryption { get; set; } = MsrpEncryptionEnum.None;

    /// <summary>
    /// Contains a list of audio codecs that the application will offer for audio in the outgoing INVITE request.
    /// </summary>
    public List<string> AudioCodecs { get; set; } = new List<string>() { "PCMU", "PCMA", "G722", "G729", "AMR-WB" };

    /// <summary>
    /// Contains a list of video codecs that the application will offer for video in the outgoing INVITE request.
    /// </summary>
    public List<string> VideoCodecs { get; set; } = new List<string>() { "H264", "VP8" };

    /// <summary>
    /// If true, then the application will play an audio file instead of using the microphone input for
    /// audio.
    /// </summary>
    public bool UseRecordedAudio {  get; set; } = false;

    /// <summary>
    /// Specifies the default audio recording file for transmit audio for the OspSimulator application.
    /// </summary>
    public const string DEFAULT_AUDIO_RECORDING_FILE = "./Recordings/DefaultAudioRecording.wav";

    /// <summary>
    /// If true, then the application will use the default audio recording for audio.
    /// </summary>
    public bool UseDefaultAudioRecording { get; set; } = true;

    /// <summary>
    /// Specifies the path to the audio recording file that the user has specified. Required only if
    /// UseRecordedAudio is true and UseDefaultAudioRecording is false.
    /// </summary>
    public string AudioRecordingFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Constructor
    /// </summary>
    public MediaSettings()
    {
    }
}
