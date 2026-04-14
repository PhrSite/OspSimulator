namespace OspSimulator
{
    partial class SelectCodecForm
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
            components = new System.ComponentModel.Container();
            SelectedListBox = new ListBox();
            NotSelectedListBox = new ListBox();
            RemoveBtn = new Button();
            AddBtn = new Button();
            ClearSelectedBtn = new Button();
            OkBtn = new Button();
            CancelBtn = new Button();
            HelpBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // SelectedListBox
            // 
            SelectedListBox.FormattingEnabled = true;
            SelectedListBox.Location = new Point(34, 65);
            SelectedListBox.Name = "SelectedListBox";
            SelectedListBox.Size = new Size(183, 190);
            SelectedListBox.TabIndex = 0;
            // 
            // NotSelectedListBox
            // 
            NotSelectedListBox.FormattingEnabled = true;
            NotSelectedListBox.Location = new Point(342, 65);
            NotSelectedListBox.Name = "NotSelectedListBox";
            NotSelectedListBox.Size = new Size(183, 190);
            NotSelectedListBox.TabIndex = 1;
            // 
            // RemoveBtn
            // 
            RemoveBtn.AutoSize = true;
            RemoveBtn.Location = new Point(241, 182);
            RemoveBtn.Name = "RemoveBtn";
            RemoveBtn.Size = new Size(79, 41);
            RemoveBtn.TabIndex = 2;
            RemoveBtn.Text = ">>>";
            toolTip1.SetToolTip(RemoveBtn, "Removes a selected codec from the Selected list and adds it to the Available list");
            RemoveBtn.UseVisualStyleBackColor = true;
            RemoveBtn.Click += RemoveBtn_Click;
            // 
            // AddBtn
            // 
            AddBtn.AutoSize = true;
            AddBtn.Location = new Point(241, 126);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(79, 41);
            AddBtn.TabIndex = 3;
            AddBtn.Text = "<<<";
            toolTip1.SetToolTip(AddBtn, "Adds a selected codec from the Available list to the Selected list");
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += AddBtn_Click;
            // 
            // ClearSelectedBtn
            // 
            ClearSelectedBtn.AutoSize = true;
            ClearSelectedBtn.Location = new Point(35, 274);
            ClearSelectedBtn.Name = "ClearSelectedBtn";
            ClearSelectedBtn.Size = new Size(182, 41);
            ClearSelectedBtn.TabIndex = 4;
            ClearSelectedBtn.Text = "Clear";
            toolTip1.SetToolTip(ClearSelectedBtn, "Clears the Selected list of codes and adds the selected codecs to the Available list");
            ClearSelectedBtn.UseVisualStyleBackColor = true;
            ClearSelectedBtn.Click += ClearSelectedBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.AutoSize = true;
            OkBtn.Location = new Point(420, 365);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(105, 41);
            OkBtn.TabIndex = 5;
            OkBtn.Text = "OK";
            toolTip1.SetToolTip(OkBtn, "Closes this dialog box. Changes to the settings will be saved");
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.AutoSize = true;
            CancelBtn.Location = new Point(280, 365);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(105, 41);
            CancelBtn.TabIndex = 6;
            CancelBtn.Text = "Cancel";
            toolTip1.SetToolTip(CancelBtn, "Closes this dialog box. Changes to the settings will not be saved");
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // HelpBtn
            // 
            HelpBtn.AutoSize = true;
            HelpBtn.Location = new Point(156, 365);
            HelpBtn.Name = "HelpBtn";
            HelpBtn.Size = new Size(105, 41);
            HelpBtn.TabIndex = 7;
            HelpBtn.Text = "Help";
            toolTip1.SetToolTip(HelpBtn, "Shows the help topic for this dialog box");
            HelpBtn.UseVisualStyleBackColor = true;
            HelpBtn.Click += HelpBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(35, 31);
            label1.Name = "label1";
            label1.Size = new Size(101, 31);
            label1.TabIndex = 8;
            label1.Text = "Selected";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(342, 31);
            label2.Name = "label2";
            label2.Size = new Size(108, 31);
            label2.TabIndex = 9;
            label2.Text = "Available";
            // 
            // SelectCodecForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(555, 429);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(HelpBtn);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(ClearSelectedBtn);
            Controls.Add(AddBtn);
            Controls.Add(RemoveBtn);
            Controls.Add(NotSelectedListBox);
            Controls.Add(SelectedListBox);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(5);
            MaximizeBox = false;
            Name = "SelectCodecForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select Codecs";
            Load += SelectCodecForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox SelectedListBox;
        private ListBox NotSelectedListBox;
        private Button RemoveBtn;
        private Button AddBtn;
        private Button ClearSelectedBtn;
        private Button OkBtn;
        private Button CancelBtn;
        private Button HelpBtn;
        private Label label1;
        private Label label2;
        private ToolTip toolTip1;
    }
}