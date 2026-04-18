/////////////////////////////////////////////////////////////////////////////////////
//  File:   HelpForm.cs                                             16 Apr 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace OspSimulator;

/// <summary>
/// This form contains a Web Browser control (WebView2) and serveral buttons that enable basic
/// navigation. The purpose of this form is to allow a Windows Forms application to display on-line
/// help web pages.
/// </summary>
public partial class HelpForm : Form
{
    private string m_strUrl;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="strUrl">HTTP URL to navigate to when this form is loaded.</param>
    public HelpForm(string strUrl)
    {
        InitializeComponent();
        m_strUrl = strUrl;
    }


    private async void HelpForm_Load(object sender, EventArgs e)
    {
        await webView21.EnsureCoreWebView2Async(null); // Initialize the engine
        webView21.CoreWebView2.Navigate(m_strUrl);
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void HomeBtn_Click(object sender, EventArgs e)
    {
        webView21.CoreWebView2.Navigate(m_strUrl);
    }

    private void NavigateBackBtn_Click(object sender, EventArgs e)
    {
        webView21.CoreWebView2.GoBack();
    }

    private void NavigateForwardBtn_Click(object sender, EventArgs e)
    {
        webView21.CoreWebView2.GoForward();
    }
}
