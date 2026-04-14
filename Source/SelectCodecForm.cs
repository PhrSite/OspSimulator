/////////////////////////////////////////////////////////////////////////////////////
//  File:   SelectCodecForm.cs                                      31 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////


namespace OspSimulator;

/// <summary>
/// This form class allows the user to select which codecs to offer for audio or video media.
/// </summary>
public partial class SelectCodecForm : Form
{
    private List<string> m_SelectedList;
    private List<string> m_SupportedList;
    private string m_FormTitle;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="selectedList">Contains the list of currently selected codecs</param>
    /// <param name="supportedList">Contains the list of supported codecs</param>
    /// <param name="title">Title to display in the form control bar</param>
    public SelectCodecForm(List<string> selectedList, List<string> supportedList, string title)
    {
        m_SelectedList = selectedList;
        m_SupportedList = supportedList;
        m_FormTitle = title;
        InitializeComponent();
    }

    private void SelectCodecForm_Load(object sender, EventArgs e)
    {
        Text = m_FormTitle;
        foreach (string strNotSelected in m_SupportedList)
        {
            if (m_SelectedList.Contains(strNotSelected) == false)
                NotSelectedListBox.Items.Add(strNotSelected);
        }

        foreach (string strSelected in m_SelectedList)
            SelectedListBox.Items.Add(strSelected);
    }

    private void ClearSelectedBtn_Click(object sender, EventArgs e)
    {
        List<string> removeList = new List<string>();
        foreach (string strSelected in SelectedListBox.Items)
        {
            removeList.Add(strSelected);
            NotSelectedListBox.Items.Add(strSelected);
        }

        foreach (string strRemove in removeList)
            SelectedListBox.Items.Remove(strRemove);

    }

    private void AddBtn_Click(object sender, EventArgs e)
    {
        string? str = NotSelectedListBox.SelectedItem?.ToString();
        if (str == null)
        {
            MessageBox.Show("Please select a codec from the Available list.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        SelectedListBox.Items.Add(str);
        NotSelectedListBox.Items.Remove(str);
    }

    private void RemoveBtn_Click(object sender, EventArgs e)
    {
        string? str = SelectedListBox.SelectedItem?.ToString();
        if (str == null)
        {
            MessageBox.Show("Please select a codec from the Selected list", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        SelectedListBox.Items.Remove(str);
        NotSelectedListBox.Items.Add(str);
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {

    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private List<string> m_SelectedCodecs = new List<string>();

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (SelectedListBox.Items.Count == 0)
        {
            MessageBox.Show("At least one codec must be selected", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        m_SelectedCodecs.Clear();
        foreach (string str in SelectedListBox.Items)
            m_SelectedCodecs.Add(str);

        DialogResult= DialogResult.OK;
        Close();
    }

    /// <summary>
    /// Gets the list of selected codecs. Call this method if ShowDialog() returns DialogResult.OK.
    /// </summary>
    /// <returns>The list will contain at least one codec name if ShowDialog() returned DialogResult.OK</returns>
    public List<string> GetSelectedCodecs()
    {
        return m_SelectedCodecs;
    }
}
