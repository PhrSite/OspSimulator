namespace OspSimulator
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tabControl1 = new TabControl();
            NetworkPage = new TabPage();
            groupBox1 = new GroupBox();
            PortsGridView = new DataGridView();
            MediaType = new DataGridViewTextBoxColumn();
            StartPort = new DataGridViewTextBoxColumn();
            PortCount = new DataGridViewTextBoxColumn();
            LocalSipsPortTb = new TextBox();
            label4 = new Label();
            LocalSipPortTb = new TextBox();
            label3 = new Label();
            HttpsPortTb = new TextBox();
            label2 = new Label();
            HttpPortTb = new TextBox();
            label1 = new Label();
            UseIPv4ForHttpCheck = new CheckBox();
            UseHttpsCheck = new CheckBox();
            MutualAuthenticationCheck = new CheckBox();
            CertificatePage = new TabPage();
            groupBox2 = new GroupBox();
            CertPasswordTb = new TextBox();
            label9 = new Label();
            CertFileBrowseBtn = new Button();
            CertFileTb = new TextBox();
            label8 = new Label();
            DefaultCertCb = new CheckBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            OkBtn = new Button();
            CancelBtn = new Button();
            HelpBtn = new Button();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            NetworkPage.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortsGridView).BeginInit();
            CertificatePage.SuspendLayout();
            groupBox2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(tabControl1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 88.88889F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1111107F));
            tableLayoutPanel1.Size = new Size(1117, 546);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(NetworkPage);
            tabControl1.Controls.Add(CertificatePage);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1111, 479);
            tabControl1.TabIndex = 0;
            // 
            // NetworkPage
            // 
            NetworkPage.Controls.Add(groupBox1);
            NetworkPage.Controls.Add(LocalSipsPortTb);
            NetworkPage.Controls.Add(label4);
            NetworkPage.Controls.Add(LocalSipPortTb);
            NetworkPage.Controls.Add(label3);
            NetworkPage.Controls.Add(HttpsPortTb);
            NetworkPage.Controls.Add(label2);
            NetworkPage.Controls.Add(HttpPortTb);
            NetworkPage.Controls.Add(label1);
            NetworkPage.Controls.Add(UseIPv4ForHttpCheck);
            NetworkPage.Controls.Add(UseHttpsCheck);
            NetworkPage.Controls.Add(MutualAuthenticationCheck);
            NetworkPage.Location = new Point(4, 40);
            NetworkPage.Name = "NetworkPage";
            NetworkPage.Padding = new Padding(3);
            NetworkPage.Size = new Size(1103, 435);
            NetworkPage.TabIndex = 0;
            NetworkPage.Text = "Network";
            NetworkPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(PortsGridView);
            groupBox1.Location = new Point(400, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(550, 267);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            groupBox1.Text = "Media Ports";
            // 
            // PortsGridView
            // 
            PortsGridView.AllowUserToAddRows = false;
            PortsGridView.AllowUserToDeleteRows = false;
            PortsGridView.AllowUserToResizeColumns = false;
            PortsGridView.AllowUserToResizeRows = false;
            PortsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PortsGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            PortsGridView.BackgroundColor = SystemColors.Control;
            PortsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PortsGridView.Columns.AddRange(new DataGridViewColumn[] { MediaType, StartPort, PortCount });
            PortsGridView.Location = new Point(34, 47);
            PortsGridView.MultiSelect = false;
            PortsGridView.Name = "PortsGridView";
            PortsGridView.RowHeadersVisible = false;
            PortsGridView.RowHeadersWidth = 51;
            PortsGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            PortsGridView.ScrollBars = ScrollBars.None;
            PortsGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            PortsGridView.Size = new Size(495, 183);
            PortsGridView.TabIndex = 8;
            // 
            // MediaType
            // 
            MediaType.HeaderText = "Media Type";
            MediaType.MinimumWidth = 6;
            MediaType.Name = "MediaType";
            MediaType.ReadOnly = true;
            MediaType.Resizable = DataGridViewTriState.False;
            MediaType.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // StartPort
            // 
            StartPort.HeaderText = "Start Port";
            StartPort.MinimumWidth = 6;
            StartPort.Name = "StartPort";
            StartPort.Resizable = DataGridViewTriState.False;
            StartPort.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PortCount
            // 
            PortCount.HeaderText = "Port Count";
            PortCount.MinimumWidth = 6;
            PortCount.Name = "PortCount";
            PortCount.Resizable = DataGridViewTriState.False;
            PortCount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // LocalSipsPortTb
            // 
            LocalSipsPortTb.Location = new Point(205, 368);
            LocalSipsPortTb.Name = "LocalSipsPortTb";
            LocalSipsPortTb.Size = new Size(125, 38);
            LocalSipsPortTb.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(19, 368);
            label4.Name = "label4";
            label4.Size = new Size(163, 31);
            label4.TabIndex = 13;
            label4.Text = "Local SIPS Port";
            // 
            // LocalSipPortTb
            // 
            LocalSipPortTb.Location = new Point(205, 313);
            LocalSipPortTb.Name = "LocalSipPortTb";
            LocalSipPortTb.Size = new Size(125, 38);
            LocalSipPortTb.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 313);
            label3.Name = "label3";
            label3.Size = new Size(151, 31);
            label3.TabIndex = 11;
            label3.Text = "Local SIP Port";
            // 
            // HttpsPortTb
            // 
            HttpsPortTb.Location = new Point(153, 256);
            HttpsPortTb.Name = "HttpsPortTb";
            HttpsPortTb.Size = new Size(125, 38);
            HttpsPortTb.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 256);
            label2.Name = "label2";
            label2.Size = new Size(126, 31);
            label2.TabIndex = 9;
            label2.Text = "HTTPS Port";
            // 
            // HttpPortTb
            // 
            HttpPortTb.Location = new Point(153, 194);
            HttpPortTb.Name = "HttpPortTb";
            HttpPortTb.Size = new Size(125, 38);
            HttpPortTb.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 194);
            label1.Name = "label1";
            label1.Size = new Size(114, 31);
            label1.TabIndex = 7;
            label1.Text = "HTTP Port";
            // 
            // UseIPv4ForHttpCheck
            // 
            UseIPv4ForHttpCheck.AutoSize = true;
            UseIPv4ForHttpCheck.Location = new Point(19, 132);
            UseIPv4ForHttpCheck.Name = "UseIPv4ForHttpCheck";
            UseIPv4ForHttpCheck.Size = new Size(243, 35);
            UseIPv4ForHttpCheck.TabIndex = 3;
            UseIPv4ForHttpCheck.Text = "Use IPv4 For HTTP(s)";
            UseIPv4ForHttpCheck.UseVisualStyleBackColor = true;
            // 
            // UseHttpsCheck
            // 
            UseHttpsCheck.AutoSize = true;
            UseHttpsCheck.Location = new Point(19, 79);
            UseHttpsCheck.Name = "UseHttpsCheck";
            UseHttpsCheck.Size = new Size(145, 35);
            UseHttpsCheck.TabIndex = 2;
            UseHttpsCheck.Text = "Use HTTPS";
            UseHttpsCheck.UseVisualStyleBackColor = true;
            // 
            // MutualAuthenticationCheck
            // 
            MutualAuthenticationCheck.AutoSize = true;
            MutualAuthenticationCheck.Location = new Point(20, 27);
            MutualAuthenticationCheck.Name = "MutualAuthenticationCheck";
            MutualAuthenticationCheck.Size = new Size(310, 35);
            MutualAuthenticationCheck.TabIndex = 1;
            MutualAuthenticationCheck.Text = "Use Mutual Authentication";
            MutualAuthenticationCheck.UseVisualStyleBackColor = true;
            // 
            // CertificatePage
            // 
            CertificatePage.Controls.Add(groupBox2);
            CertificatePage.Location = new Point(4, 40);
            CertificatePage.Name = "CertificatePage";
            CertificatePage.Size = new Size(1103, 435);
            CertificatePage.TabIndex = 2;
            CertificatePage.Text = "Certificate";
            CertificatePage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(CertPasswordTb);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(CertFileBrowseBtn);
            groupBox2.Controls.Add(CertFileTb);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(DefaultCertCb);
            groupBox2.Location = new Point(19, 38);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(819, 241);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "X.509 Certificate";
            // 
            // CertPasswordTb
            // 
            CertPasswordTb.Location = new Point(213, 156);
            CertPasswordTb.Name = "CertPasswordTb";
            CertPasswordTb.PasswordChar = '*';
            CertPasswordTb.Size = new Size(466, 38);
            CertPasswordTb.TabIndex = 4;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(18, 159);
            label9.Name = "label9";
            label9.Size = new Size(110, 31);
            label9.TabIndex = 4;
            label9.Text = "Password";
            // 
            // CertFileBrowseBtn
            // 
            CertFileBrowseBtn.AutoSize = true;
            CertFileBrowseBtn.Location = new Point(690, 107);
            CertFileBrowseBtn.Name = "CertFileBrowseBtn";
            CertFileBrowseBtn.Size = new Size(107, 42);
            CertFileBrowseBtn.TabIndex = 3;
            CertFileBrowseBtn.Text = "Browse";
            CertFileBrowseBtn.UseVisualStyleBackColor = true;
            CertFileBrowseBtn.Click += CertFileBrowseBtn_Click;
            // 
            // CertFileTb
            // 
            CertFileTb.Location = new Point(213, 105);
            CertFileTb.Name = "CertFileTb";
            CertFileTb.Size = new Size(466, 38);
            CertFileTb.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(18, 108);
            label8.Name = "label8";
            label8.Size = new Size(159, 31);
            label8.TabIndex = 1;
            label8.Text = "Certificate File";
            // 
            // DefaultCertCb
            // 
            DefaultCertCb.AutoSize = true;
            DefaultCertCb.Location = new Point(18, 54);
            DefaultCertCb.Name = "DefaultCertCb";
            DefaultCertCb.Size = new Size(264, 35);
            DefaultCertCb.TabIndex = 1;
            DefaultCertCb.Text = "Use Default Certificate";
            DefaultCertCb.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(OkBtn);
            flowLayoutPanel1.Controls.Add(CancelBtn);
            flowLayoutPanel1.Controls.Add(HelpBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(3, 488);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1111, 55);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // OkBtn
            // 
            OkBtn.AutoSize = true;
            OkBtn.Location = new Point(1003, 3);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(105, 41);
            OkBtn.TabIndex = 7;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.AutoSize = true;
            CancelBtn.Location = new Point(903, 3);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 41);
            CancelBtn.TabIndex = 6;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // HelpBtn
            // 
            HelpBtn.AutoSize = true;
            HelpBtn.Location = new Point(792, 3);
            HelpBtn.Name = "HelpBtn";
            HelpBtn.Size = new Size(105, 41);
            HelpBtn.TabIndex = 5;
            HelpBtn.Text = "Help";
            HelpBtn.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1117, 546);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OspSimulator Advanced Settings";
            Load += SettingsForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            NetworkPage.ResumeLayout(false);
            NetworkPage.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PortsGridView).EndInit();
            CertificatePage.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TabControl tabControl1;
        private TabPage NetworkPage;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button OkBtn;
        private Button CancelBtn;
        private Button HelpBtn;
        private CheckBox MutualAuthenticationCheck;
        private CheckBox UseHttpsCheck;
        private CheckBox UseIPv4ForHttpCheck;
        private TextBox HttpsPortTb;
        private Label label2;
        private TextBox HttpPortTb;
        private Label label1;
        private TextBox LocalSipPortTb;
        private Label label3;
        private TextBox LocalSipsPortTb;
        private Label label4;
        private DataGridView PortsGridView;
        private DataGridViewTextBoxColumn MediaType;
        private DataGridViewTextBoxColumn StartPort;
        private DataGridViewTextBoxColumn PortCount;
        private TabPage CertificatePage;
        private GroupBox groupBox2;
        private TextBox CertPasswordTb;
        private Label label9;
        private Button CertFileBrowseBtn;
        private TextBox CertFileTb;
        private Label label8;
        private CheckBox DefaultCertCb;
        private GroupBox groupBox1;
    }
}