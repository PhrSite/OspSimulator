/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallForm.cs                                             14 Feb 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using CameraCapture;
using OspSimulator.Settings;
using SipLib.Rtp;
using SipLib.Media;

using System.Diagnostics;
using System.Text;

namespace OspSimulator;

/// <summary>
/// Form class for the form that is displayed when the outgoing call is answered.
/// </summary>
public partial class CallForm : Form
{
    private AppSettings m_AppSettings;
    private OspOutgoingCall m_Call;
    private WindowsCameraCapture? m_CameraCapture;
    private TextMessagesCollection? m_TextMessages = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="appSettings">Application settings</param>
    /// <param name="call">Call object for the call that has been answered by the called party.</param>
    /// <param name="cameraCapture">WindowsCameraCapture to use for capturing video frames from the camera
    /// for previewing.</param>
    public CallForm(AppSettings appSettings, OspOutgoingCall call, WindowsCameraCapture? cameraCapture)
    {
        m_AppSettings = appSettings;
        m_Call = call;
        m_CameraCapture = cameraCapture;
        InitializeComponent();

        if (m_Call.CallHasRtt == true)
            m_TextMessages = m_Call.RttMessages;
        else if (m_Call.CallHasMsrp == true)
            m_TextMessages = m_Call.MsrpMessages;
        else
            m_TextMessages = null;
    }

    private void EndCallBtn_Click(object sender, EventArgs e)
    {
        m_Call.EndCall();
    }

    private void CallForm_Load(object sender, EventArgs e)
    {
        if (m_CameraCapture != null)
        {
            m_CameraCapture.FrameBitmapReady += OnFrameBitmapReadyForPreview;
            m_Call.FrameBitmapReady += OnReceivedFrameBitmapReady;
        }

        InitializeTextMessages();

        DisplayMediaTypes();
        DisplayTextType();

        ToLbl.Text = m_AppSettings.LastCallSettings.SipToUri.ToString();
        FromLbl.Text = m_AppSettings.LastCallSettings.FromUser;
        if (m_AppSettings.MediaSettings.UseRecordedAudio == false)
            AudioSourceLbl.Text = "Microphone";
        else
            AudioSourceLbl.Text = "Recording";

        m_Call.CallEnded += OnCallEnded;
        m_Call.CallMediaAdded += OnCallMediaAdded;
        m_Call.ReInviteTimedOut += OnReInviteTimedOut;
        m_Call.ReInviteFailed += OnReInviteFailed;

        RtpChannel? audioRtpChannel = m_Call.GetAudioRtpChannel();
        if (audioRtpChannel != null)
            audioRtpChannel.ReceiveStatisticsReady += OnAudioReceiveStatisticsReady;

    }

    private void OnReInviteFailed(string errorMessage)
    {
        BeginInvoke(() =>
        {
            MessageBox.Show($"Re-INVITE request failed. Reason = {errorMessage}", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        });
    }

    private void OnReInviteTimedOut()
    {
        BeginInvoke(() =>
        {
            MessageBox.Show("Re-INVITE request timed out", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        });
    }

    private void InitializeTextMessages()
    {
        if (m_TextMessages != null)
        {
            int index = 0;
            foreach (TextMessage message in m_TextMessages.Messages)
            {
                TextMessage textMessage = m_TextMessages.Messages[index++];
                ListViewItem item = BuildListViewItem(textMessage);
                TextListView.Items.Add(item);
                item.Tag = textMessage;
            }

            m_TextMessages.MessageAdded += OnMessageAdded;
            m_TextMessages.MessageUpdated += OnMessageUpdated;
        }
    }

    private void DisplayTextType()
    {
        if (m_Call.CallHasRtt == true)
            TextTypeLbl.Text = "RTT";
        else if (m_Call.CallHasMsrp == true)
            TextTypeLbl.Text = "MSRP";
        else
            TextTypeLbl.Text = "None";
    }

    private void DisplayMediaTypes()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < m_Call.CallMediaTypes.Count; i++)
        {
            if (i >= 1)
                stringBuilder.Append(", ");

            stringBuilder.Append(MediaTypeToDisplayString(m_Call.CallMediaTypes[i]));
        }

        MediaTypesLbl.Text = stringBuilder.ToString();
    }

    private void OnCallMediaAdded(string media)
    {
        Invoke(() => HandleCallMediaAdded(media));
    }

    private void HandleCallMediaAdded(string newMedia)
    {
        DisplayMediaTypes();
        DisplayTextType();

        switch (newMedia)
        {
            case MediaTypes.Audio:
                // Nothing needs to be done
                break;
            case MediaTypes.Video:
                // Nothing needs to be done
                break;
            case MediaTypes.RTT:
                m_TextMessages = m_Call.RttMessages;
                InitializeTextMessages();
                break;
            case MediaTypes.MSRP:
                m_TextMessages = m_Call.MsrpMessages;
                InitializeTextMessages();
                break;
        } // end switch
    }

    private string MediaTypeToDisplayString(string mediaType)
    {
        string strMedia = "Unknown";
        switch (mediaType)
        {
            case "audio":
                strMedia = "Audio";
                break;
            case "video":
                strMedia = "Video";
                break;
            case "message":
                strMedia = "MSRP";
                break;
            case "text":
                strMedia = "RTT";
                break;
        }

        return strMedia;
    }

    private void OnAudioReceiveStatisticsReady(RtpReceiveStatistics receiveStatistics, RtpChannel rtpChannel)
    {
        // For debug only
        RtpReceiveStatistics Rrs = receiveStatistics;
        string strTime = DateTime.Now.ToString("HH:mm:ss");
        Debug.WriteLine($"Time = {strTime}: MOS = {Rrs.Mos.MOS:F2}, Smoothed Jitter = {Rrs.SmoothedJitter.Average} ms, " +
            $"Max Jitter = {Rrs.SmoothedJitter.Maximum} ms, Dropped Packets = {Rrs.DroppedPackets}");
    }

    private void OnMessageUpdated(int index)
    {
        // For debug only
        if (index < 0)
            return;

        BeginInvoke(() =>
        {
            if (m_TextMessages == null)
                return;

            if (index < m_TextMessages.Messages.Count)
            {
                ListViewItem? item = TextListView.Items[index];
                if (item != null)
                {
                    item.SubItems[1].Text = m_TextMessages.Messages[index].Message;
                }
            }
        });
    }

    private void OnMessageAdded(int index)
    {
        BeginInvoke(() =>
        {
            if (m_TextMessages == null)
                return;

            TextMessage textMessage = m_TextMessages.Messages[index];
            ListViewItem item = BuildListViewItem(textMessage);
            item.Tag = textMessage;
            TextListView.Items.Add(item);
            TextListView.EnsureVisible(TextListView.Items.Count - 1);
        });
    }

    private ListViewItem BuildListViewItem(TextMessage textMessage)
    {
        ListViewItem item = new ListViewItem(textMessage.From);

        if (textMessage.Source == TextSourceEnum.Received)
            item.BackColor = Color.Beige;
        else
            item.BackColor = Color.LightCyan;

        item.SubItems.Add(textMessage.Message);
        item.SubItems.Add(textMessage.Time.ToString("HH:mm:ss"));

        return item;
    }

    private void OnCallEnded()
    {
        try
        {
            BeginInvoke(() => Close());
        }
        catch
        {
        }
    }

    private void OnReceivedFrameBitmapReady(Bitmap bitmap)
    {
        BeginInvoke(() =>
        {
            ReceiveVideoPb.Image?.Dispose();
            ReceiveVideoPb.Image = bitmap;
        });
    }

    private void CallForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (m_CameraCapture != null)
        {
            m_CameraCapture.FrameBitmapReady -= OnFrameBitmapReadyForPreview;
            m_Call.FrameBitmapReady -= OnReceivedFrameBitmapReady;
        }

        if (m_TextMessages != null)
        {
            m_TextMessages.MessageAdded -= OnMessageAdded;
            m_TextMessages.MessageUpdated -= OnMessageUpdated;
            m_TextMessages = null;
        }

        RtpChannel? audioRtpChannel = m_Call.GetAudioRtpChannel();
        if (audioRtpChannel != null)
            audioRtpChannel.ReceiveStatisticsReady -= OnAudioReceiveStatisticsReady;

        m_Call.CallMediaAdded -= OnCallMediaAdded;
        m_Call.ReInviteTimedOut -= OnReInviteTimedOut;
        m_Call.ReInviteFailed -= OnReInviteFailed;
    }

    private void OnFrameBitmapReadyForPreview(Bitmap bitmap)
    {
        BeginInvoke(() =>
        {
            try
            {
                PreviewVideoPb.Image?.Dispose();
                PreviewVideoPb.Image = bitmap;
            }
            catch
            {
            }
        });
    }

    private void SendBtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NewMessageTb.Text) == true)
            return;

        string strMessage = new string(NewMessageTb.Text);
        NewMessageTb.Text = string.Empty;
        if (m_Call.CallHasMsrp == true)
        {
            // TODO: Implement sending CPIM messages
            m_Call.SendMsrpText(strMessage);
        }
        else if (m_Call.CallHasRtt == true)
            m_Call.SendRttCharacters(strMessage);

    }

    private void NewMessageTb_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (m_Call.CallHasMsrp == true)
        {
            if (e.KeyChar == '\r')
            {
                SendBtn_Click(this, new EventArgs());
            }
        }
        else if (m_Call.CallHasRtt == true)
        {
            m_Call.SendRttCharacters(e.KeyChar.ToString());
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                NewMessageTb.Text = string.Empty;
                m_TextMessages?.ClearLastSource();   // Force the next characters into a new row
            }
        }
    }

    private void AddMediaBtn_Click(object sender, EventArgs e)
    {
        // Figure out which media can be added to the call.
        List<string> availableMedia = new List<string>()
        {
            MediaTypes.Audio, MediaTypes.Video, MediaTypes.RTT, MediaTypes.MSRP
        };

        foreach (string callMediaType in m_Call.CallMediaTypes)
            availableMedia.Remove(callMediaType);

        // Only one type of text media can be added to the call.
        if (m_Call.CallHasMsrp == true)
            availableMedia.Remove(MediaTypes.RTT);

        if (m_Call.CallHasRtt == true)
            availableMedia.Remove(MediaTypes.MSRP);

        if (availableMedia.Count == 0)
        {
            MessageBox.Show("The call has all available media types. No new media can be added to the call.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Build a list of display media names to offer the user to select from
        List<string> displayMediaTypes = new List<string>();
        foreach (string mediaType in availableMedia)
            displayMediaTypes.Add(MediaTypeToDisplayString(mediaType));

        AddNewMediaForm newMediaForm = new AddNewMediaForm(displayMediaTypes);
        DialogResult result = newMediaForm.ShowDialog();
        if (result != DialogResult.OK)
            return;

        List<string> selectedMediaTypes = newMediaForm.GetSelectedMedia();
        if (selectedMediaTypes.Count == 0)
            return;

        List<string> mediaTypesToAdd = new List<string>();
        foreach (string strSelected in selectedMediaTypes)
            mediaTypesToAdd.Add(MediaDisplayTypeToMediaType(strSelected));

        m_Call.SendReInviteToAddMedia(mediaTypesToAdd);
    }

    private string MediaDisplayTypeToMediaType(string displayType)
    {
        string strMedia = "Unknown";
        switch (displayType)
        {
            case "Audio":
                strMedia = MediaTypes.Audio;
                break;
            case "Video":
                strMedia = MediaTypes.Video;
                break;
            case "RTT":
                strMedia = MediaTypes.RTT;
                break;
            case "MSRP":
                strMedia = MediaTypes.MSRP;
                break;
        }

        return strMedia;
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {
        HelpUtils.ShowHelp("CallForm.html");
    }
}
