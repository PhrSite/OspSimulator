/////////////////////////////////////////////////////////////////////////////////////
//  File:   SettingsForm.cs                                         28 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using OspSimulator.Settings;
using PsapSimulator.Settings;
using SipLib.Logging;
using SipLib.Media;
using System.Security.Cryptography.X509Certificates;

namespace OspSimulator;

/// <summary>
/// Form class for the advanced settings OspSimulator application.
/// </summary>
public partial class SettingsForm : Form
{
    private AppSettings m_AppSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="appSettings">Current application configuration settings.</param>
    public SettingsForm(AppSettings appSettings)
    {
        m_AppSettings = appSettings;
        InitializeComponent();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (VerifySettings() == false)
            return;     // Error message already displayed

        NetworkSettings Ns = m_AppSettings.NetworkSettings;
        Ns.UseMutualAuthentication = MutualAuthenticationCheck.Checked;
        Ns.UseHttps = UseHttpsCheck.Checked;
        Ns.UseIPv4ForHttp = UseIPv4ForHttpCheck.Checked;
        Ns.HttpPortNumber = int.Parse(HttpPortTb.Text);
        Ns.HttpsPortNumber = int.Parse(HttpsPortTb.Text);
        Ns.LocalSipPortNumber = int.Parse(LocalSipPortTb.Text);
        Ns.LocalSipsPortNumber = int.Parse(LocalSipsPortTb.Text);
        Ns.MediaPorts.AudioPorts = GetMediaPortRange(AudioRow);
        Ns.MediaPorts.VideoPorts = GetMediaPortRange(VideoRow);
        Ns.MediaPorts.RttPorts = GetMediaPortRange(RttRow);
        Ns.MediaPorts.MsrpPorts = GetMediaPortRange(MsrpRow);

        CertificateSettings certSettings = m_AppSettings.CertificateSettings;
        certSettings.UseDefaultCertificate = DefaultCertCb.Checked;
        certSettings.CertificateFilePath = CertFileTb.Text;
        certSettings.CertificatePassword = CertPasswordTb.Text;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        // Display the current settings.
        NetworkSettings networkSettings = m_AppSettings.NetworkSettings;
        MutualAuthenticationCheck.Checked = networkSettings.UseMutualAuthentication;
        UseHttpsCheck.Checked = networkSettings.UseHttps;
        UseIPv4ForHttpCheck.Checked = networkSettings.UseIPv4ForHttp;
        HttpPortTb.Text = networkSettings.HttpPortNumber.ToString();
        HttpsPortTb.Text = networkSettings.HttpsPortNumber.ToString();
        LocalSipPortTb.Text = networkSettings.LocalSipPortNumber.ToString();
        LocalSipsPortTb.Text = networkSettings.LocalSipsPortNumber.ToString();

        MediaPortSettings Mps = m_AppSettings.NetworkSettings.MediaPorts;
        PortsGridView.Rows.Add("Audio", Mps.AudioPorts.StartPort, Mps.AudioPorts.Count);
        PortsGridView.Rows.Add("Video", Mps.VideoPorts.StartPort, Mps.VideoPorts.Count);
        PortsGridView.Rows.Add("RTT", Mps.RttPorts.StartPort, Mps.RttPorts.Count);
        PortsGridView.Rows.Add("MSRP", Mps.MsrpPorts.StartPort, Mps.MsrpPorts.Count);

        CertificateSettings certSettings = m_AppSettings.CertificateSettings;
        DefaultCertCb.Checked = certSettings.UseDefaultCertificate;
        CertFileTb.Text = certSettings.CertificateFilePath;
        CertPasswordTb.Text = certSettings.CertificatePassword;
    }

    private const int MinPort = 5000;
    private const int MaxPort = short.MaxValue;

    private bool VerifySettings()
    {
        if (VerifyTextBoxNumericEntry(HttpPortTb, MinPort, MaxPort, "HTTP Port") == false)
            return false;

        if (VerifyTextBoxNumericEntry(HttpsPortTb, MinPort, MaxPort, "HTTPS Port") == false)
            return false;

        if (VerifyTextBoxNumericEntry(LocalSipPortTb, MinPort, MaxPort, "Local SIP Port") == false)
            return false;

        if (VerifyTextBoxNumericEntry(LocalSipsPortTb, MinPort, MaxPort, "Local SIPS Port") == false)
            return false;

        int HttpPort = int.Parse(HttpPortTb.Text);
        int HttpsPort = int.Parse(HttpsPortTb.Text);
        int LocalSipPort = int.Parse(LocalSipPortTb.Text);
        int LocalSipsPort = int.Parse(LocalSipsPortTb.Text);

        if (HttpPort == LocalSipPort || HttpPort == LocalSipsPort)
        {
            MessageBox.Show("The HTTP Port settings may not be equal to the Local SIP Port or the " +
                "Local SIPS Port setting.");
            HttpPortTb.Focus();
            return false;
        }

        if (HttpsPort == LocalSipPort || HttpsPort == LocalSipsPort)
        {
            MessageBox.Show("The HTTPS Port settings may not be equal to the Local SIP Port or the " +
                "Local SIPS Port setting.");
            HttpPortTb.Focus();
            return false;
        }

        // Note: The HTTP Port and the HTTPS port may be equal because either HTTP or HTTPS is used.
        // Note: The Local SIP Port and the Local SIPS port may be equal because either SIP or SIPS is used.

        // Check the media port settings
        if (ValidateMediaPortSettings() == false)
            return false;

        // Check for port duplication between HTTP, HTTPS, SIP and SIPS with the media port ranges.
        List<PortRange> ports = new List<PortRange>();
        for (int i = AudioRow; i <= MsrpRow; i++)
            ports.Add(GetMediaPortRange(i));

        if (PortIsInMediaRange(HttpPort, ports) == true)
        {
            MessageBox.Show("The HTTP Port is in media port ranges.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HttpPortTb.Focus();
            return false;
        }

        if (PortIsInMediaRange(HttpsPort, ports) == true)
        {
            MessageBox.Show("The HTTPS Port is in media port ranges.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HttpsPortTb.Focus();
            return false;
        }

        if (PortIsInMediaRange(LocalSipPort, ports) == true)
        {
            MessageBox.Show("The Local SIP Port is in media port ranges.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HttpPortTb.Focus();
            return false;
        }

        if (PortIsInMediaRange(LocalSipPort, ports) == true)
        {
            MessageBox.Show("The Local SIPS Port is in media port ranges.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HttpPortTb.Focus();
            return false;
        }

        // Certificate page
        if (DefaultCertCb.Checked == false)
        {
            if (string.IsNullOrEmpty(CertFileTb.Text) == true)
            {
                MessageBox.Show("The Certificate File must be specified.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CertFileTb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(CertPasswordTb.Text) == true)
            {
                MessageBox.Show("The Password must be specified.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CertPasswordTb.Focus();
                return false;
            }

            // Try loading the certificate to make sure that is valid.
            bool certificateError = false;
            try
            {
                X509Certificate2 certificate = X509CertificateLoader.LoadPkcs12FromFile(CertFileTb.Text,
                    CertPasswordTb.Text);
            }
            catch (Exception)
            {
                certificateError = true;
            }

            if (certificateError == true)
            {
                MessageBox.Show("Error loading the X.509 certificate from the file. Make sure that " +
                    "the *.PFX file is present and valid and that the password is correct.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        return true;
    }

    private bool PortIsInMediaRange(int port, List<PortRange> mediaPorts)
    {
        for (int i = AudioRow; i < MsrpRow; i++)
        {
            PortRange portRange = mediaPorts[i];
            if (port >= portRange.StartPort && port <= (portRange.StartPort + portRange.Count - 1))
                return true;
        }

        return false;
    }

    private const int AudioRow = 0;
    private const int VideoRow = 1;
    private const int RttRow = 2;
    private const int MsrpRow = 3;
    private const int StartPortCell = 1;
    private const int CountCell = 2;

    private readonly string[] MediaTypes = { "Audio", "Video", "RTT", "MSRP" };

    private bool ValidateMediaPortSettings()
    {
        int i;
        for (i = AudioRow; i <= MsrpRow; i++)
        {
            if (ValidateMediaRange(i) == false)
                return false;
        }

        List<PortRange> ports = new List<PortRange>();
        for (i = AudioRow; i <= MsrpRow; i++)
            ports.Add(GetMediaPortRange(i));

        // Test for port range overlaps. Don't care about MSRP because it uses TCP and the other media
        // types use UDP so port range overlaps are not a problem.
        for (i = AudioRow; i < MsrpRow; i++)
        {
            int CurrentStart = ports[i].StartPort;
            int CurrentEnd = CurrentStart + ports[i].Count - 1;
            int NextStart;
            int NextEnd;
            for (int j = i + 1; j < MsrpRow; j++)
            {
                NextStart = ports[j].StartPort;
                NextEnd = NextStart + ports[j].Count - 1;

                if (CurrentStart >= NextStart && CurrentStart <= NextEnd || (CurrentEnd >= NextStart &&
                    CurrentEnd <= NextEnd))
                {
                    MessageBox.Show($"The port range for {MediaTypes[i]} overlaps the port range for {MediaTypes[j]}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (NextStart >= CurrentStart && NextStart <= CurrentEnd || (NextEnd >= CurrentStart &&
                    NextEnd <= CurrentEnd))
                {
                    MessageBox.Show($"The port range for {MediaTypes[i]} overlaps the port range for {MediaTypes[j]}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        return true;
    }

    private PortRange GetMediaPortRange(int MediaIndex)
    {
        PortRange range = new PortRange();
        range.StartPort = int.Parse(PortsGridView.Rows[MediaIndex].Cells[StartPortCell].Value!.ToString()!);
        range.Count = int.Parse(PortsGridView.Rows[MediaIndex].Cells[CountCell].Value!.ToString()!);
        return range;
    }

    private bool ValidateMediaRange(int Row)
    {
        int StartPort = 0, Count = 0;

        object StartVal = PortsGridView.Rows[Row].Cells[StartPortCell].Value!;
        object CountVal = PortsGridView.Rows[Row].Cells[CountCell].Value!;

        if (StartVal == null)
        {
            MessageBox.Show($"The Start Port for {MediaTypes[Row]} must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (CountVal == null)
        {
            MessageBox.Show($"The Count for {MediaTypes[Row]} must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (int.TryParse(StartVal.ToString(), out StartPort) == false || StartPort <= 1024 || StartPort > 65535)
        {
            MessageBox.Show($"The Start Port for {MediaTypes[Row]} must be an integer value greater than 1024 " +
                "and less than or equal to 65535", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (int.TryParse(CountVal.ToString(), out Count) == false || Count <= 0)
        {
            MessageBox.Show($"The Count for {MediaTypes[Row]} must be an integer value greater than 0", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        return true;
    }


    private bool VerifyTextBoxNumericEntry(TextBox textBox, int MinValue, int MaxValue, string settingName)
    {
        int intValue = 0;
        string strErrorMessage = $"The {settingName} must be an integer value {MinValue} and {MaxValue}";

        if (string.IsNullOrEmpty(textBox.Text) == true || int.TryParse(textBox.Text, out intValue) == false ||
            intValue < MinValue || intValue > MaxValue)
        {
            MessageBox.Show(strErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBox.Focus();
            return false;
        }
        else
            return true;
    }

    private void CertFileBrowseBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "PFX files (*.pfx)|*.pfx";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            CertFileTb.Text = ofd.FileName;
    }
}
