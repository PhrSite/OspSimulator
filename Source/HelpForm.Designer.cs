namespace OspSimulator
{
    partial class HelpForm
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
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            flowLayoutPanel1 = new FlowLayoutPanel();
            CloseBtn = new Button();
            NavigateBackBtn = new Button();
            NavigateForwardBtn = new Button();
            HomeBtn = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(webView21, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tableLayoutPanel1.Size = new Size(1098, 478);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(3, 3);
            webView21.Name = "webView21";
            webView21.Size = new Size(1092, 410);
            webView21.TabIndex = 1;
            webView21.ZoomFactor = 1D;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(CloseBtn);
            flowLayoutPanel1.Controls.Add(NavigateBackBtn);
            flowLayoutPanel1.Controls.Add(NavigateForwardBtn);
            flowLayoutPanel1.Controls.Add(HomeBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 419);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1092, 56);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // CloseBtn
            // 
            CloseBtn.AutoSize = true;
            CloseBtn.Dock = DockStyle.Left;
            CloseBtn.Location = new Point(3, 3);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(105, 41);
            CloseBtn.TabIndex = 0;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // NavigateBackBtn
            // 
            NavigateBackBtn.AutoSize = true;
            NavigateBackBtn.Location = new Point(114, 3);
            NavigateBackBtn.Name = "NavigateBackBtn";
            NavigateBackBtn.Size = new Size(169, 41);
            NavigateBackBtn.TabIndex = 1;
            NavigateBackBtn.Text = "Navigate Back";
            NavigateBackBtn.UseVisualStyleBackColor = true;
            NavigateBackBtn.Click += NavigateBackBtn_Click;
            // 
            // NavigateForwardBtn
            // 
            NavigateForwardBtn.AutoSize = true;
            NavigateForwardBtn.Location = new Point(289, 3);
            NavigateForwardBtn.Name = "NavigateForwardBtn";
            NavigateForwardBtn.Size = new Size(205, 41);
            NavigateForwardBtn.TabIndex = 2;
            NavigateForwardBtn.Text = "Navigate Forward";
            NavigateForwardBtn.UseVisualStyleBackColor = true;
            NavigateForwardBtn.Click += NavigateForwardBtn_Click;
            // 
            // HomeBtn
            // 
            HomeBtn.AutoSize = true;
            HomeBtn.Location = new Point(500, 3);
            HomeBtn.Name = "HomeBtn";
            HomeBtn.Size = new Size(94, 41);
            HomeBtn.TabIndex = 3;
            HomeBtn.Text = "Home";
            HomeBtn.UseVisualStyleBackColor = true;
            HomeBtn.Click += HomeBtn_Click;
            // 
            // HelpForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1098, 478);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "HelpForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Help Form";
            WindowState = FormWindowState.Maximized;
            Load += HelpForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button CloseBtn;
        private Button NavigateBackBtn;
        private Button NavigateForwardBtn;
        private Button HomeBtn;
    }
}