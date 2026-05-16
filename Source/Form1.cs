/////////////////////////////////////////////////////////////////////////////////////
//  File:   Form.cs                                                 7 Dec 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace OspSimulator;

using CameraCapture;
using Ng911Lib.Utilities;
using OspSimulator.Settings;
using SipLib.Audio.Windows;
using SipLib.Channels;
using SipLib.Core;
using SipLib.Logging;
using SipLib.Media;
using SipLib.Network;
using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.Video.Windows;

/// <summary>
/// Main Form class for the OspSimulator application. 
/// </summary>
public partial class Form1 : Form
{
    private WindowsCameraCapture? m_CameraCapture = null;
    private bool m_Capturing = false;

    private OspOutgoingCall? m_Call = null;
    private SdpOfferSettings m_SdpOfferSettings;
    private SipChannelSettings m_SipChannelSettings;
    private X509Certificate2? m_Certificate = null;
    private AdditionalDataStore? m_AdditionalDataStore = null;
    private AppSettings m_AppSettings;

    private bool m_IsLoading = false;
    private List<string> m_SupportedVideoCodecs = new List<string>() { "H264", "VP8" };
    private const string UserName = "OspSimulator";
    private Dictionary<string, List<VideoDeviceFormat>>? m_VideoDevices = null;
    private OspServer? m_OspServer = null;

    private const int DEFAULT_AUDIO_SAMPLES_PER_SEC = 16000;
    private WindowsAudioIo? m_WindowsAudioIo = null;

    /// <summary>
    /// Constructor
    /// </summary>
    public Form1()
    {
        m_AppSettings = AppSettings.GetAppSettings();

        m_SipChannelSettings = new SipChannelSettings();
        m_SipChannelSettings.LocalUser = UserName;
        m_SipChannelSettings.LocalIPv4Address = IpUtils.GetDefaultIPv4Address();
        m_SipChannelSettings.LocalIPv6Address = IpUtils.GetDefaultIPv6Address();

        MediaPortSettings mediaPortSettings = new MediaPortSettings();
        m_SdpOfferSettings = new SdpOfferSettings(AudioMediaUtils.SupportedAudioCodecs, m_SupportedVideoCodecs,
            UserName, RtpChannel.CertificateFingerprint!, new MediaPortManager(mediaPortSettings));
        m_SdpOfferSettings.OfferAudio = true;
        m_SdpOfferSettings.OfferVideo = false;
        m_SdpOfferSettings.OfferRtt = false;
        m_SdpOfferSettings.OfferMsrp = false;

        m_Certificate = m_AppSettings.CertificateSettings.GetCertificateFromFile();

        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        string strVer = System.Reflection.Assembly.GetEntryAssembly()!.GetName()!.Version!.ToString();
        Text += $" -- Version: {strVer}";

        m_IsLoading = true;

        await ShowAppSettings();
        m_IsLoading = false;
    }

    /// <summary>
    /// Gets the application settings from the form controls into the m_AppSettings object and saves
    /// them to the application's configuration file.
    /// </summary>
    private void SaveSettings()
    {
        CallSettings callSettings = m_AppSettings.LastCallSettings;
        callSettings.PreferIPv6 = PreferIPv6CheckBox.Checked;
        callSettings.SipToUri = ToSipUriTb.Text;
        callSettings.UseUrnServiceSos = UseUrnCheckBox.Checked;
        callSettings.FromUser = FromNumberCombo.Text;
        callSettings.UseTelUri = UseTelUriCheck.Checked;

        NetworkSettings networkSettings = m_AppSettings.NetworkSettings;
        networkSettings.IPv4Address = IPv4Combo.Text;
        networkSettings.IPv6Address = IPv6Combo.Text;
        networkSettings.EnableIPv4 = EnableIPv4Check.Checked;
        networkSettings.EnableIPv6 = EnableIPv6Check.Checked;

        MediaSettings mediaSettings = m_AppSettings.MediaSettings;
        mediaSettings.OfferAudio = OfferAudioCheck.Checked;
        mediaSettings.OfferVideo = OfferVideoCheck.Checked;
        mediaSettings.OfferRtt = OfferRttCheck.Checked;
        mediaSettings.OfferRttConferenceAware = false;
        mediaSettings.OfferMsrp = OfferMsrpCheck.Checked;
        mediaSettings.UseMsrpCpim = OfferCpimCheck.Checked;
        mediaSettings.RtpEncryption = (RtpEncryptionEnum)RtpEncryptionCombo.SelectedIndex;
        mediaSettings.MsrpEncryption = (MsrpEncryptionEnum)MsrpEncryptionCombo.SelectedIndex;

        mediaSettings.UseRecordedAudio = UseRecordedAudioCheck.Checked;
        mediaSettings.UseDefaultAudioRecording = UseDefaultAudioCheck.Checked;
        mediaSettings.AudioRecordingFilePath = AudioFileTb.Text;

        mediaSettings.AudioCodecs.Clear();
        foreach (string strAudioCodec in OfferAudioList.Items)
            mediaSettings.AudioCodecs.Add(strAudioCodec);

        mediaSettings.VideoCodecs.Clear();
        foreach (string strVideoCodec in OfferVideoList.Items)
            mediaSettings.VideoCodecs.Add(strVideoCodec);

        LocationSettings locationSettings = m_AppSettings.LocationSettings;
        locationSettings.LocationByValue = LocationByValueCheck.Checked;
        locationSettings.LocationByReference = LocationByReferenceCheck.Checked;
        locationSettings.LocationByPresenceEvent = SipPresenceCheck.Checked;

        AdditionalDataSettings additionalData = m_AppSettings.AddtionalDataSettings;
        additionalData.AdditionalDataByValue = AddDataByValueCheck.Checked;
        additionalData.AdditionalDataByReference = AddDataByReferenceCheck.Checked;

        DeviceSettings deviceSettings = m_AppSettings.DeviceSettings;
        deviceSettings.AudioDeviceName = AudioDeviceCombo.Text;

        if (VideoDevicesCombo.SelectedIndex >= 0)
        {
            deviceSettings.VideoDevice = new VideoSourceSettings();
            deviceSettings.VideoDevice.SelectedDeviceName = VideoDevicesCombo.Text;
            if (VideoListView.CheckedIndices.Count > 0)
            {
                int index = VideoListView.CheckedIndices[0];
                deviceSettings.VideoDevice.DeviceFormat = (VideoDeviceFormat)VideoListView.Items[index].Tag!;
            }
        }

        AppSettings.SaveAppSettings(m_AppSettings);
    }

    private bool VerifySettings()
    {
        SIPURI? sipUri = null;
        if (string.IsNullOrEmpty(ToSipUriTb.Text) == true || SIPURI.TryParse(ToSipUriTb.Text, out sipUri) == false)
        {
            MessageBox.Show("The To SIP URI is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ToSipUriTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(FromNumberCombo.Text) == true || FromNumberCombo.Text !=
            SIPEscape.EscapeSpecialCharacters(FromNumberCombo.Text))
        {
            MessageBox.Show("The From Number must be specified and it must not contain any special " +
                "characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FromNumberCombo.Focus();
            return false;
        }

        if (EnableIPv4Check.Checked == false && EnableIPv6Check.Checked == false)
        {
            MessageBox.Show("At least one IP address (IPv4 or IPv6) must be enabled and selected", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (EnableIPv4Check.Checked == true && string.IsNullOrEmpty(IPv4Combo.Text) == true)
        {
            MessageBox.Show("IPv4 is enabled so an IPv4 address must be selected.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPv4Combo.Focus();
            return false;
        }

        if (EnableIPv6Check.Checked == true && string.IsNullOrEmpty(IPv6Combo.Text) == true)
        {
            MessageBox.Show("IPv6 is enabled so an IPv6 address must be selected.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPv6Combo.Focus();
            return false;
        }

        if (m_AppSettings.NetworkSettings.UseIPv4ForHttp == true && string.IsNullOrEmpty(IPv4Combo.Text) == true)
        {
            MessageBox.Show("IPv4 is selected for HTTP so an IPv4 address must be selected. See Advanced Settings",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPv4Combo.Focus();
            return false;
        }

        if (m_AppSettings.NetworkSettings.UseIPv4ForHttp == false && string.IsNullOrEmpty(IPv6Combo.Text) == true)
        {
            MessageBox.Show("IPv6 is selected for HTTP so an IPv6 address must be selected. See Advanced Settings",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPv6Combo.Focus();
            return false;
        }

        if (OfferAudioCheck.Checked == true)
        {
            if (OfferAudioList.Items.Count == 0)
            {
                MessageBox.Show("Audio is enabled but no audio codecs are selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OfferAudioCheck.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(AudioDeviceCombo.Text) == true)
            {
                MessageBox.Show("Audio is enabled but no Audio Device is selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OfferAudioCheck.Focus();
                return false;
            }

            if (UseRecordedAudioCheck.Checked == true)
            {
                if (UseDefaultAudioCheck.Checked == false)
                {
                    if (string.IsNullOrEmpty(AudioFileTb.Text) == true)
                    {
                        MessageBox.Show("The Audio Recording File must be specified.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        AudioFileTb.Focus();
                        return false;
                    }
                }
            }
        }

        if (OfferVideoCheck.Checked == true)
        {
            if (OfferVideoList.Items.Count == 0)
            {
                MessageBox.Show("Video is enabled but no video codecs are selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OfferVideoCheck.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(VideoDevicesCombo.Text) == true)
            {
                MessageBox.Show("Video is enabled but no Video Device is selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OfferVideoCheck.Focus();
                return false;
            }

            if (VideoListView.CheckedIndices.Count == 0)
            {
                MessageBox.Show("Video is enabled but no Video Format is selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OfferVideoCheck.Focus();
                return false;
            }
        }

        if (OfferRttCheck.Checked == true && OfferMsrpCheck.Checked == true)
        {
            MessageBox.Show("Only one type of text media may be selected.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        return true;
    }

    private async Task ShowAppSettings()
    {
        IPv4Combo.Items.Clear();
        IPv6Combo.Items.Clear();
        SetIpAddress(IpUtils.GetIPv4Addresses(), IPv4Combo, m_AppSettings.NetworkSettings.IPv4Address);
        SetIpAddress(IpUtils.GetIPv6Addresses(), IPv6Combo, m_AppSettings.NetworkSettings.IPv6Address);
        EnableIPv4Check.Checked = m_AppSettings.NetworkSettings.EnableIPv4;
        EnableIPv6Check.Checked = m_AppSettings.NetworkSettings.EnableIPv6;

        PreferIPv6CheckBox.Checked = m_AppSettings.LastCallSettings.PreferIPv6;

        m_AdditionalDataStore = new AdditionalDataStore();
        m_AdditionalDataStore.Initialize();

        ToSipUriTb.Text = m_AppSettings.LastCallSettings.SipToUri;

        FromNumberCombo.Items.Clear();
        // FromNumberCombo.Items.Add(DefaultFromNumber);
        List<string> numbers = m_AdditionalDataStore.GetCallingPartyNumbers();
        foreach (string number in numbers)
            FromNumberCombo.Items.Add(number);

        FromNumberCombo.Text = m_AppSettings.LastCallSettings.FromUser;
        UseTelUriCheck.Checked = m_AppSettings.LastCallSettings.UseTelUri;

        UseUrnCheckBox.Checked = m_AppSettings.LastCallSettings.UseUrnServiceSos;
        PreferIPv6CheckBox.Checked = m_AppSettings.LastCallSettings.PreferIPv6;

        MediaSettings mediaSettings = m_AppSettings.MediaSettings;
        OfferAudioCheck.Checked = mediaSettings.OfferAudio;
        OfferVideoCheck.Checked = mediaSettings.OfferVideo;
        OfferRttCheck.Checked = mediaSettings.OfferRtt;
        OfferMsrpCheck.Checked = mediaSettings.OfferMsrp;
        OfferCpimCheck.Checked = mediaSettings.UseMsrpCpim;

        RtpEncryptionCombo.SelectedIndex = (int)mediaSettings.RtpEncryption;
        MsrpEncryptionCombo.SelectedIndex = (int)mediaSettings.MsrpEncryption;

        OfferAudioList.Items.Clear();
        foreach (string strAudioCodec in mediaSettings.AudioCodecs)
            OfferAudioList.Items.Add(strAudioCodec);

        OfferVideoList.Items.Clear();
        foreach (string strVideoCodec in mediaSettings.VideoCodecs)
            OfferVideoList.Items.Add(strVideoCodec);

        UseRecordedAudioCheck.Checked = mediaSettings.UseRecordedAudio;
        UseDefaultAudioCheck.Checked = mediaSettings.UseDefaultAudioRecording;
        AudioFileTb.Text = mediaSettings.AudioRecordingFilePath;

        AdditionalDataSettings additionalData = m_AppSettings.AddtionalDataSettings;
        LocationSettings locationSettings = m_AppSettings.LocationSettings;
        LocationByValueCheck.Checked = locationSettings.LocationByValue;
        LocationByReferenceCheck.Checked = locationSettings.LocationByReference;
        SipPresenceCheck.Checked = locationSettings.LocationByPresenceEvent;
        AddDataByValueCheck.Checked = additionalData.AdditionalDataByValue;
        AddDataByReferenceCheck.Checked = additionalData.AdditionalDataByReference;

        List<string> AudioDevices = WindowsAudioIo.GetAudioDeviceNames();
        foreach (string audioDevice in AudioDevices)
        {
            AudioDeviceCombo.Items.Add(audioDevice);
        }
        if (AudioDevices.Count > 0)
        {
            if (string.IsNullOrEmpty(m_AppSettings.DeviceSettings.AudioDeviceName) == false)
            {
                int AudioIndex = AudioDeviceCombo.FindString(m_AppSettings.DeviceSettings.AudioDeviceName);
                if (AudioIndex >= 0)
                    AudioDeviceCombo.SelectedIndex = AudioIndex;
                else
                    AudioDeviceCombo.SelectedIndex = 0;
            }
            else
                AudioDeviceCombo.SelectedIndex = 0;
        }

        int ColWidth = VideoListView.ClientRectangle.Width / 4 - 6;
        for (int i = 0; i < VideoListView.Columns.Count; i++)
            VideoListView.Columns[i].Width = ColWidth;

        m_VideoDevices = await VideoDeviceEnumerator.GetVideoFrameSources();
        SetVideoDeviceSelections();
    }

    private void SetVideoDeviceSelections()
    {
        if (m_VideoDevices == null || m_VideoDevices.Keys.Count == 0)
            return;     // No video capture devices available

        foreach (string deviceName in m_VideoDevices.Keys)
            VideoDevicesCombo.Items.Add(deviceName);

        VideoSourceSettings? Vss = m_AppSettings.DeviceSettings.VideoDevice;
        string? SelectedDevice;
        if (Vss == null || Vss.SelectedDeviceName == null)
        {   // No video settings yet, so pick the first available device
            VideoDevicesCombo.SelectedIndex = 0;
            SelectedDevice = VideoDevicesCombo.Text;
            LoadVideoFormats(m_VideoDevices[SelectedDevice]);
        }
        else
        {
            SelectedDevice = Vss.SelectedDeviceName;
            int index = VideoDevicesCombo.FindString(SelectedDevice);
            if (index < 0)
                // The previously selected device device was not found, default to the first device
                index = 0;

            VideoDevicesCombo.SelectedIndex = index;
            SelectedDevice = VideoDevicesCombo.Text;
            LoadVideoFormats(m_VideoDevices[SelectedDevice]);
            SelectVideoFormat(Vss.DeviceFormat);
        }
    }

    private void SelectVideoFormat(VideoDeviceFormat format)
    {
        int foundIndex = -1;
        for (int i = 0; i < VideoListView.Items.Count; i++)
        {
            VideoDeviceFormat? Vdf = (VideoDeviceFormat?)VideoListView.Items[i].Tag;
            if (Vdf == null)
                continue;

            if (Vdf.SubType == format.SubType && Vdf.Width == format.Width && Vdf.Height == format.Height &&
                Vdf.Framerate == format.Framerate)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex >= 0)
            VideoListView.Items[foundIndex].Checked = true;
        else
        {   // Nothing found, pick the first format
            if (VideoListView.Items.Count > 0)
                VideoListView.Items[0].Checked = true;
        }
    }

    private void LoadVideoFormats(List<VideoDeviceFormat> formats)
    {
        VideoListView.Items.Clear();
        foreach (VideoDeviceFormat format in formats)
        {
            // Limit the video resolution to VGA for capture so that the application can run reliably on older versions
            // of Windows with low performance hardware on low bandwidth networks when media encryption is enabled.
            if (format.Width <= 640)
            {
                ListViewItem Lvi = new ListViewItem(format.SubType);
                Lvi.SubItems.Add(format.Width.ToString());
                Lvi.SubItems.Add(format.Height.ToString());
                Lvi.SubItems.Add(format.Framerate.ToString());
                Lvi.Tag = format;
                VideoListView.Items.Add(Lvi);
            }
        }
    }

    private void SetIpAddress(List<IPAddress> ips, ComboBox combo, string? setting)
    {
        if (ips.Count == 0)
            return;

        foreach (IPAddress ip in ips)
        {
            combo.Items.Add(ip.ToString());
        }

        if (string.IsNullOrEmpty(setting) == false)
        {
            int index = combo.FindString(setting);
            if (index >= 0)
                combo.SelectedIndex = index;
            else
                combo.SelectedIndex = 0;
        }
        else
            combo.SelectedIndex = 0;
    }

    private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private async void CallBtn_Click(object sender, EventArgs e)
    {
        CallStatusLabel.Text = "";
        if (m_Call == null)
        {   // Start a new call
            if (m_OspServer == null || m_AdditionalDataStore == null)
            {
                MessageBox.Show("The server is not running. Click on the Start Server button to start the server",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (VerifySettings() == false)
                return;

            SaveSettings();

            OspCallParameters? callParameters = await GetCurrentSettings();
            if (callParameters == null)
                return; // Error message already displayed

            VideoSourceSettings? videoSourceSettings = m_AppSettings.DeviceSettings.VideoDevice;
            try
            {
                if (videoSourceSettings != null && string.IsNullOrEmpty(videoSourceSettings.SelectedDeviceName) == false)
                {
                    m_CameraCapture = new WindowsCameraCapture(videoSourceSettings);
                    m_Capturing = await m_CameraCapture.StartCapture();
                    if (m_Capturing == false)
                    {
                        m_AppSettings.MediaSettings.OfferVideo = false;
                        m_CameraCapture = null;
                        callParameters.OfferSettings!.OfferVideo = false;
                        SipLogger.LogError($"Failed to start the camera capture for video sevice name = {videoSourceSettings.SelectedDeviceName}.");
                    }
                }
                else
                {
                    m_AppSettings.MediaSettings.OfferVideo = false;
                    callParameters.OfferSettings!.OfferVideo = false;
                }
            }
            catch (Exception ex)
            {
                SipLogger.LogError(ex, "Failed to create the camera capture.");
                MessageBox.Show("An exception occured when creating the video capture device", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_WindowsAudioIo = new WindowsAudioIo(DEFAULT_AUDIO_SAMPLES_PER_SEC, m_AppSettings.DeviceSettings.AudioDeviceName);
                m_WindowsAudioIo.StartAudio();
            }
            catch
            {
                MessageBox.Show($"Failed to start the audio device called: {m_AppSettings.DeviceSettings.AudioDeviceName}. " +
                    "Verify that the selected audio capture device is connected and enabled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_Call = new OspOutgoingCall(callParameters, m_Certificate!, m_AdditionalDataStore, m_AppSettings,
                m_OspServer, m_CameraCapture, m_WindowsAudioIo);
            m_Call.CallEstablished += OnCallEstablished;
            m_Call.CallRequestTimedOut += OnCallRequestTimedOut;
            m_Call.CallEnded += OnCallEnded;
            m_Call.CallStatusUpdate += OnCallStatusUpdate;
            m_Call.CallCancellationComplete += OnCallCancellationComplete;
            m_Call.CallRejected += OnCallRejected;

            m_Call.StartCall();
            CallBtn.Text = "Calling...";
        }
        else
        {   // End the current call
            await StopCameraCapture();
            m_Call.EndCall();
            CallBtn.Text = "Start Call";
        }
    }

    private async Task StopCameraCapture()
    {
        if (m_CameraCapture != null)
        {
            await m_CameraCapture.StopCapture();
            m_CameraCapture = null;
            m_Capturing = false;
        }
    }

    private void OnCallRejected(SIPResponseStatusCodesEnum status, string? reason)
    {
        BeginInvoke(() =>
        {
            CallBtn.Text = "Start Call";
            CallStatusLabel.Text = string.IsNullOrEmpty(reason) == false ? reason : "Rejected reason unknown";
            TerminateCall();
        });
    }

    private bool m_CallCancelled = false;

    private void OnCallCancellationComplete()
    {
        BeginInvoke(() =>
        {
            m_CallCancelled = true;
            CallBtn.Text = "Start Call";
            TerminateCall();
            CallStatusLabel.Text = "Cancelled";
        });
    }

    private void OnCallStatusUpdate(string status)
    {
        BeginInvoke(() =>
        {
            CallStatusLabel.Text = status;
        });
    }

    private void OnCallEnded()
    {
        BeginInvoke(() =>
        {
            TerminateCall();
            CallBtn.Text = "Start Call";
            if (m_CallCancelled == true)
            {
                m_CallCancelled = false;
                CallStatusLabel.Text = "Cancelled";
            }
            else
                CallStatusLabel.Text = "";
        });
    }

    private void TerminateCall()
    {
        if (m_Call == null)
            return;

        m_Call.Shutdown();
        m_Call.CallEstablished -= OnCallEstablished;
        m_Call.CallEnded -= OnCallEnded;
        m_Call.CallStatusUpdate -= OnCallStatusUpdate;
        m_Call.CallCancellationComplete -= OnCallCancellationComplete;
        m_Call = null;

        if (m_WindowsAudioIo != null)
        {
            m_WindowsAudioIo.StopAudio();
            m_WindowsAudioIo = null;
        }

        _ = StopCameraCapture();
    }

    private void OnCallRequestTimedOut()
    {
        BeginInvoke(() =>
        {
            CallBtn.Text = "Start Call";
            CallStatusLabel.Text = "Timed Out";
            TerminateCall();
        });
    }

    private void OnCallEstablished()
    {
        BeginInvoke(async () =>
        {
            CallStatusLabel.Text = "On-Line";
            CallBtn.Text = "Hang Up";

            if (m_Call == null)
                return;

            m_Call.CallEnded -= OnCallEnded;
            CallForm callForm = new CallForm(m_AppSettings, m_Call, m_CameraCapture);
            callForm.ShowDialog();
            TerminateCall();
            CallStatusLabel.Text = string.Empty;
            CallBtn.Text = "Start Call";
        });
    }

    private async Task<OspCallParameters?> GetCurrentSettings()
    {
        OspCallParameters callParameters = new OspCallParameters();
        m_SipChannelSettings = new SipChannelSettings();
        m_SipChannelSettings.LocalUser = UserName;
        NetworkSettings networkSettings = m_AppSettings.NetworkSettings;
        if (networkSettings.EnableIPv4 == true)
            m_SipChannelSettings.LocalIPv4Address = IPAddress.Parse(networkSettings.IPv4Address);
        else
            m_SipChannelSettings.LocalIPv4Address = null;

        if (networkSettings.EnableIPv6 == true)
            m_SipChannelSettings.LocalIPv6Address = IPAddress.Parse(networkSettings.IPv6Address);
        else
            m_SipChannelSettings.LocalIPv6Address = null;

        m_SipChannelSettings.LocalSipPort = networkSettings.LocalSipPortNumber;
        m_SipChannelSettings.LocalSipsPort = networkSettings.LocalSipsPortNumber;
        m_SipChannelSettings.UseMutualAuthentication = networkSettings.UseMutualAuthentication;

        callParameters.ChannelSettings = m_SipChannelSettings;

        if (m_SipChannelSettings.LocalIPv4Address == null && m_SipChannelSettings.LocalIPv6Address == null)
        {
            MessageBox.Show("Cannot make a call because this PC does not have an IP address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        if (m_Certificate == null)
        {
            MessageBox.Show("No X.509 certificate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        // Check the From entry is a valid user part of a SIP URI
        string? strFrom = FromNumberCombo.Text;
        if (string.IsNullOrEmpty(strFrom) == true || strFrom != SIPEscape.EscapeSpecialCharacters(strFrom))
        {
            MessageBox.Show("The From Number must be a valid SIP user part that contains no whitespace or " +
                "punctuation, special characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FromNumberCombo.Focus();
            return null;
        }

        SIPURI? toSipUri;
        if (string.IsNullOrEmpty(ToSipUriTb.Text) == true || SIPURI.TryParse(ToSipUriTb.Text, out toSipUri) == false ||
            toSipUri is null)
        {
            MessageBox.Show("The To SIP URI must be a valid SIP URI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ToSipUriTb.Focus();
            return null;
        }

        SIPURI? resolvedSipUri = null;
        try
        {
            resolvedSipUri = await SipDnsClient.ResolveSipServerAsync(toSipUri, m_SipChannelSettings != null ? true : false,
                PreferIPv6CheckBox.Checked, false);
        }
        catch { }

        if (resolvedSipUri is null)
        {
            MessageBox.Show("Failed to determine the SIP server's IP address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ToSipUriTb.Focus();
            return null;
        }

        IPAddress resolvedIpAddress = resolvedSipUri.ToSIPEndPoint()!.GetIPEndPoint().Address;
        if (m_SipChannelSettings!.LocalIPv6Address == null && resolvedIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
        {
            MessageBox.Show("Cannot reach the SIP server because the SIP server's IP address is IPv6 but this PC does not have an IPv6 address",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ToSipUriTb.Focus();
            return null;
        }

        if (m_SipChannelSettings!.LocalIPv4Address == null && resolvedIpAddress.AddressFamily == AddressFamily.InterNetwork)
        {
            MessageBox.Show("Cannot reach the SIP server because the SIP server's IP address is IPv4 but this PC does not have an IPv4 address",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ToSipUriTb.Focus();
            return null;
        }

        callParameters.ResolvedSipUri = resolvedSipUri;
        callParameters.ToSipUri = toSipUri;

        MediaSettings mediaSettings = m_AppSettings.MediaSettings;
        m_SdpOfferSettings = new SdpOfferSettings(mediaSettings.AudioCodecs, mediaSettings.VideoCodecs,
            Program.AppName, RtpChannel.CertificateFingerprint!, new MediaPortManager(networkSettings.MediaPorts));
        m_SdpOfferSettings.OfferAudio = OfferAudioCheck.Checked;
        m_SdpOfferSettings.OfferVideo = OfferVideoCheck.Checked;
        m_SdpOfferSettings.OfferRtt = OfferRttCheck.Checked;
        m_SdpOfferSettings.OfferMsrp = OfferMsrpCheck.Checked;
        m_SdpOfferSettings.RtpEncryptionType = (RtpEncryptionEnum)RtpEncryptionCombo.SelectedIndex;
        if (MsrpEncryptionCombo.SelectedIndex == 0)
            m_SdpOfferSettings.UseTlsForMsrp = false;
        else
            m_SdpOfferSettings.UseTlsForMsrp = true;

        callParameters.OfferSettings = m_SdpOfferSettings;
        callParameters.From = FromNumberCombo.Text;

        if (UseUrnCheckBox.Checked == true)
            callParameters.RequestSipUri = SIPURI.ParseSIPURI("urn:service:sos");
        else
            callParameters.RequestSipUri = toSipUri;

        return callParameters;
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        if (m_Call != null)
        {
            MessageBox.Show("Call in progress. The call must be terminated before closing the application", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        SaveSettings();
        Close();
    }

    private async void StartServerBtn_Click(object sender, EventArgs e)
    {
        if (m_Call != null)
            return;     // Don't let the user start or stop the server if a call has been started

        if (m_OspServer == null)
        {
            if (VerifySettings() == false)
                return;     // Error messages displayed.

            SaveSettings();

            if (m_Certificate == null)
            {
                MessageBox.Show("An X.509 certificate is not available. Check the configuration " +
                    "settings in Advanced Settings and try again.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            m_AdditionalDataStore = new AdditionalDataStore();
            m_AdditionalDataStore.Initialize();
            m_OspServer = new OspServer(m_Certificate, m_AppSettings.NetworkSettings, m_AdditionalDataStore);
            m_OspServer.Start();
            StartServerBtn.Text = "Stop Server";
            EnableServerSettings(false);
        }
        else
        {
            m_AdditionalDataStore = null;
            await m_OspServer.ShutdownAsync();
            m_OspServer = null;
            StartServerBtn.Text = "Start Server";
            EnableServerSettings(true);
        }
    }

    private void EnableServerSettings(bool enable)
    {
        SettingsBtn.Enabled = enable;
        EnableIPv4Check.Enabled = enable;
        IPv4Combo.Enabled = enable;
        EnableIPv6Check.Enabled = enable;
        IPv6Combo.Enabled = enable;
        PreferIPv6CheckBox.Enabled = enable;
    }

    private void SettingsBtn_Click(object sender, EventArgs e)
    {
        string strSettings = JsonHelper.SerializeToString(m_AppSettings);
        AppSettings settingsCopy = JsonHelper.DeserializeFromString<AppSettings>(strSettings);
        SettingsForm settingsForm = new SettingsForm(settingsCopy);
        DialogResult result = settingsForm.ShowDialog();
        if (result == DialogResult.OK)
            m_AppSettings = settingsCopy;
    }

    private void VideoDevicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;

        string SelectedDevice = VideoDevicesCombo.Text;
        LoadVideoFormats(m_VideoDevices![SelectedDevice]);
        VideoListView.Items[0].Checked = true;       // Just pick the first available format
    }

    /// <summary>
    /// Fired just before the checked state of an item actually changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void VideoListView_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        if (m_IsLoading == true)
            return;

        if (e.NewValue == CheckState.Checked)
        {   // Un-check any currently checked items
            foreach (ListViewItem checkedItem in VideoListView.CheckedItems)
                checkedItem.Checked = false;
        }
    }

    private void ChangeAudioBtn_Click(object sender, EventArgs e)
    {
        SelectCodecForm form = new SelectCodecForm(m_AppSettings.MediaSettings.AudioCodecs,
            AudioMediaUtils.SupportedAudioCodecs, "Audio Codecs");
        DialogResult dialogResult = form.ShowDialog();
        if (dialogResult == DialogResult.OK)
        {
            m_AppSettings.MediaSettings.AudioCodecs = form.GetSelectedCodecs();
            OfferAudioList.Items.Clear();
            foreach (string audioCodec in m_AppSettings.MediaSettings.AudioCodecs)
                OfferAudioList.Items.Add(audioCodec);
        }
    }

    private void ChangeVideoBtn_Click(object sender, EventArgs e)
    {
        SelectCodecForm form = new SelectCodecForm(m_AppSettings.MediaSettings.VideoCodecs,
            m_SupportedVideoCodecs, "Video Codecs");
        DialogResult dialogResult = form.ShowDialog();
        if (dialogResult == DialogResult.OK)
        {
            m_AppSettings.MediaSettings.VideoCodecs = form.GetSelectedCodecs();
            OfferVideoList.Items.Clear();
            foreach (string videoCodec in m_AppSettings.MediaSettings.VideoCodecs)
                OfferVideoList.Items.Add(videoCodec);
        }
    }

    private void AudioFileBrowseBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "WAV files (*.wav)|*.wav";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            AudioFileTb.Text = ofd.FileName;
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {
        HelpUtils.ShowHelp("MainWindow.html");
    }
}
