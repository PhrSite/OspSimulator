namespace OspSimulator
{
    partial class CallForm
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
            EndCallBtn = new Button();
            PreviewVideoPb = new PictureBox();
            ReceiveVideoPb = new PictureBox();
            TextListView = new ListView();
            FromHeader = new ColumnHeader();
            MessageHeader = new ColumnHeader();
            TimeHeader = new ColumnHeader();
            label6 = new Label();
            NewMessageTb = new TextBox();
            SendBtn = new Button();
            label5 = new Label();
            TextTypeLbl = new Label();
            label1 = new Label();
            MediaTypesLbl = new Label();
            AddMediaBtn = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ToLbl = new Label();
            label7 = new Label();
            FromLbl = new Label();
            label8 = new Label();
            AudioSourceLbl = new Label();
            ((System.ComponentModel.ISupportInitialize)PreviewVideoPb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ReceiveVideoPb).BeginInit();
            SuspendLayout();
            // 
            // EndCallBtn
            // 
            EndCallBtn.AutoSize = true;
            EndCallBtn.Location = new Point(18, 267);
            EndCallBtn.Name = "EndCallBtn";
            EndCallBtn.Size = new Size(107, 41);
            EndCallBtn.TabIndex = 0;
            EndCallBtn.Text = "End Call";
            EndCallBtn.UseVisualStyleBackColor = true;
            EndCallBtn.Click += EndCallBtn_Click;
            // 
            // PreviewVideoPb
            // 
            PreviewVideoPb.BackColor = Color.Black;
            PreviewVideoPb.BorderStyle = BorderStyle.Fixed3D;
            PreviewVideoPb.Location = new Point(904, 44);
            PreviewVideoPb.Name = "PreviewVideoPb";
            PreviewVideoPb.Size = new Size(320, 240);
            PreviewVideoPb.SizeMode = PictureBoxSizeMode.StretchImage;
            PreviewVideoPb.TabIndex = 24;
            PreviewVideoPb.TabStop = false;
            // 
            // ReceiveVideoPb
            // 
            ReceiveVideoPb.BackColor = Color.Black;
            ReceiveVideoPb.BorderStyle = BorderStyle.Fixed3D;
            ReceiveVideoPb.Location = new Point(1230, 44);
            ReceiveVideoPb.Name = "ReceiveVideoPb";
            ReceiveVideoPb.Size = new Size(640, 480);
            ReceiveVideoPb.SizeMode = PictureBoxSizeMode.StretchImage;
            ReceiveVideoPb.TabIndex = 25;
            ReceiveVideoPb.TabStop = false;
            // 
            // TextListView
            // 
            TextListView.Columns.AddRange(new ColumnHeader[] { FromHeader, MessageHeader, TimeHeader });
            TextListView.GridLines = true;
            TextListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            TextListView.Location = new Point(12, 544);
            TextListView.Name = "TextListView";
            TextListView.Size = new Size(1858, 341);
            TextListView.TabIndex = 26;
            TextListView.UseCompatibleStateImageBehavior = false;
            TextListView.View = View.Details;
            // 
            // FromHeader
            // 
            FromHeader.Text = "From";
            FromHeader.Width = 200;
            // 
            // MessageHeader
            // 
            MessageHeader.Text = "Message";
            MessageHeader.Width = 1650;
            // 
            // TimeHeader
            // 
            TimeHeader.Text = "Time";
            TimeHeader.Width = 150;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 906);
            label6.Name = "label6";
            label6.Size = new Size(157, 31);
            label6.TabIndex = 27;
            label6.Text = "New Message";
            // 
            // NewMessageTb
            // 
            NewMessageTb.Location = new Point(175, 903);
            NewMessageTb.Name = "NewMessageTb";
            NewMessageTb.Size = new Size(1567, 38);
            NewMessageTb.TabIndex = 28;
            NewMessageTb.KeyPress += NewMessageTb_KeyPress;
            // 
            // SendBtn
            // 
            SendBtn.Location = new Point(1748, 906);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(115, 40);
            SendBtn.TabIndex = 29;
            SendBtn.Text = "Send";
            SendBtn.UseVisualStyleBackColor = true;
            SendBtn.Click += SendBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 508);
            label5.Name = "label5";
            label5.Size = new Size(109, 31);
            label5.TabIndex = 30;
            label5.Text = "Text Type";
            // 
            // TextTypeLbl
            // 
            TextTypeLbl.AutoSize = true;
            TextTypeLbl.BorderStyle = BorderStyle.Fixed3D;
            TextTypeLbl.Location = new Point(131, 508);
            TextTypeLbl.Name = "TextTypeLbl";
            TextTypeLbl.Size = new Size(71, 33);
            TextTypeLbl.TabIndex = 31;
            TextTypeLbl.Text = "None";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 143);
            label1.Name = "label1";
            label1.Size = new Size(179, 31);
            label1.TabIndex = 32;
            label1.Text = "Media Available";
            // 
            // MediaTypesLbl
            // 
            MediaTypesLbl.AutoSize = true;
            MediaTypesLbl.BorderStyle = BorderStyle.Fixed3D;
            MediaTypesLbl.Location = new Point(214, 143);
            MediaTypesLbl.Name = "MediaTypesLbl";
            MediaTypesLbl.Size = new Size(71, 33);
            MediaTypesLbl.TabIndex = 33;
            MediaTypesLbl.Text = "None";
            // 
            // AddMediaBtn
            // 
            AddMediaBtn.AutoSize = true;
            AddMediaBtn.Location = new Point(515, 143);
            AddMediaBtn.Name = "AddMediaBtn";
            AddMediaBtn.Size = new Size(138, 41);
            AddMediaBtn.TabIndex = 34;
            AddMediaBtn.Text = "Add Media";
            AddMediaBtn.UseVisualStyleBackColor = true;
            AddMediaBtn.Click += AddMediaBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(904, 9);
            label2.Name = "label2";
            label2.Size = new Size(177, 31);
            label2.TabIndex = 35;
            label2.Text = "Camera Preview";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1234, 8);
            label3.Name = "label3";
            label3.Size = new Size(215, 31);
            label3.TabIndex = 36;
            label3.Text = "Called Party's Video";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 18);
            label4.Name = "label4";
            label4.Size = new Size(42, 31);
            label4.TabIndex = 37;
            label4.Text = "To:";
            // 
            // ToLbl
            // 
            ToLbl.AutoSize = true;
            ToLbl.BorderStyle = BorderStyle.Fixed3D;
            ToLbl.Location = new Point(107, 18);
            ToLbl.Name = "ToLbl";
            ToLbl.Size = new Size(78, 33);
            ToLbl.TabIndex = 38;
            ToLbl.Text = "label7";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 72);
            label7.Name = "label7";
            label7.Size = new Size(71, 31);
            label7.TabIndex = 39;
            label7.Text = "From:";
            // 
            // FromLbl
            // 
            FromLbl.AutoSize = true;
            FromLbl.BorderStyle = BorderStyle.Fixed3D;
            FromLbl.Location = new Point(107, 72);
            FromLbl.Name = "FromLbl";
            FromLbl.Size = new Size(78, 33);
            FromLbl.TabIndex = 40;
            FromLbl.Text = "label8";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 195);
            label8.Name = "label8";
            label8.Size = new Size(150, 31);
            label8.TabIndex = 41;
            label8.Text = "Audio Source";
            // 
            // AudioSourceLbl
            // 
            AudioSourceLbl.AutoSize = true;
            AudioSourceLbl.BorderStyle = BorderStyle.Fixed3D;
            AudioSourceLbl.Location = new Point(214, 195);
            AudioSourceLbl.Name = "AudioSourceLbl";
            AudioSourceLbl.Size = new Size(140, 33);
            AudioSourceLbl.TabIndex = 42;
            AudioSourceLbl.Text = "Microphone";
            // 
            // CallForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1882, 953);
            ControlBox = false;
            Controls.Add(AudioSourceLbl);
            Controls.Add(label8);
            Controls.Add(FromLbl);
            Controls.Add(label7);
            Controls.Add(ToLbl);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(AddMediaBtn);
            Controls.Add(MediaTypesLbl);
            Controls.Add(label1);
            Controls.Add(TextTypeLbl);
            Controls.Add(label5);
            Controls.Add(SendBtn);
            Controls.Add(NewMessageTb);
            Controls.Add(label6);
            Controls.Add(TextListView);
            Controls.Add(ReceiveVideoPb);
            Controls.Add(PreviewVideoPb);
            Controls.Add(EndCallBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "CallForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CallForm";
            FormClosing += CallForm_FormClosing;
            Load += CallForm_Load;
            ((System.ComponentModel.ISupportInitialize)PreviewVideoPb).EndInit();
            ((System.ComponentModel.ISupportInitialize)ReceiveVideoPb).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button EndCallBtn;
        private PictureBox PreviewVideoPb;
        private PictureBox ReceiveVideoPb;
        private ListView TextListView;
        private ColumnHeader FromHeader;
        private ColumnHeader MessageHeader;
        private ColumnHeader TimeHeader;
        private Label label6;
        private TextBox NewMessageTb;
        private Button SendBtn;
        private Label label5;
        private Label TextTypeLbl;
        private Label label1;
        private Label MediaTypesLbl;
        private Button AddMediaBtn;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label ToLbl;
        private Label label7;
        private Label FromLbl;
        private Label label8;
        private Label AudioSourceLbl;
    }
}