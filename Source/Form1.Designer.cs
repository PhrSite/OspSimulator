namespace OspSimulator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CallBtn = new Button();
            CloseBtn = new Button();
            PreferIPv6CheckBox = new CheckBox();
            CallStatusLabel = new Label();
            StartServerBtn = new Button();
            SettingsBtn = new Button();
            IPv6Combo = new ComboBox();
            IPv4Combo = new ComboBox();
            EnableIPv6Check = new CheckBox();
            EnableIPv4Check = new CheckBox();
            OfferMsrpCheck = new CheckBox();
            OfferRttCheck = new CheckBox();
            OfferVideoCheck = new CheckBox();
            OfferAudioCheck = new CheckBox();
            groupBox1 = new GroupBox();
            MsrpEncryptionCombo = new ComboBox();
            RtpEncryptionCombo = new ComboBox();
            label10 = new Label();
            label7 = new Label();
            groupBox7 = new GroupBox();
            AudioFileBrowseBtn = new Button();
            AudioFileTb = new TextBox();
            label3 = new Label();
            UseDefaultAudioCheck = new CheckBox();
            UseRecordedAudioCheck = new CheckBox();
            AudioDeviceCombo = new ComboBox();
            label25 = new Label();
            groupBox8 = new GroupBox();
            VideoDevicesCombo = new ComboBox();
            label26 = new Label();
            VideoListView = new ListView();
            SubTypeHeader = new ColumnHeader();
            WidthHeader = new ColumnHeader();
            HeightHeader = new ColumnHeader();
            FpsHeader = new ColumnHeader();
            groupBox2 = new GroupBox();
            OfferCpimCheck = new CheckBox();
            AddDataByReferenceCheck = new CheckBox();
            AddDataByValueCheck = new CheckBox();
            SipPresenceCheck = new CheckBox();
            LocationByReferenceCheck = new CheckBox();
            LocationByValueCheck = new CheckBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            ChangeVideoBtn = new Button();
            ChangeAudioBtn = new Button();
            OfferVideoList = new ListBox();
            label6 = new Label();
            label5 = new Label();
            OfferAudioList = new ListBox();
            FromNumberCombo = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            ToSipUriTb = new TextBox();
            UseUrnCheckBox = new CheckBox();
            button1 = new Button();
            UseTelUriCheck = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // CallBtn
            // 
            CallBtn.AutoSize = true;
            CallBtn.Location = new Point(456, 887);
            CallBtn.Name = "CallBtn";
            CallBtn.Size = new Size(182, 41);
            CallBtn.TabIndex = 2;
            CallBtn.Text = "Start Call";
            CallBtn.UseVisualStyleBackColor = true;
            CallBtn.Click += CallBtn_Click;
            // 
            // CloseBtn
            // 
            CloseBtn.AutoSize = true;
            CloseBtn.Location = new Point(1586, 891);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(113, 44);
            CloseBtn.TabIndex = 7;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // PreferIPv6CheckBox
            // 
            PreferIPv6CheckBox.AutoSize = true;
            PreferIPv6CheckBox.Location = new Point(708, 100);
            PreferIPv6CheckBox.Name = "PreferIPv6CheckBox";
            PreferIPv6CheckBox.Size = new Size(144, 35);
            PreferIPv6CheckBox.TabIndex = 8;
            PreferIPv6CheckBox.Text = "Prefer IPv6";
            PreferIPv6CheckBox.UseVisualStyleBackColor = true;
            // 
            // CallStatusLabel
            // 
            CallStatusLabel.BorderStyle = BorderStyle.FixedSingle;
            CallStatusLabel.Location = new Point(664, 891);
            CallStatusLabel.Name = "CallStatusLabel";
            CallStatusLabel.Size = new Size(670, 37);
            CallStatusLabel.TabIndex = 9;
            // 
            // StartServerBtn
            // 
            StartServerBtn.Location = new Point(16, 885);
            StartServerBtn.Name = "StartServerBtn";
            StartServerBtn.Size = new Size(183, 43);
            StartServerBtn.TabIndex = 10;
            StartServerBtn.Text = "Start Server";
            StartServerBtn.UseVisualStyleBackColor = true;
            StartServerBtn.Click += StartServerBtn_Click;
            // 
            // SettingsBtn
            // 
            SettingsBtn.AutoSize = true;
            SettingsBtn.Location = new Point(225, 887);
            SettingsBtn.Name = "SettingsBtn";
            SettingsBtn.Size = new Size(215, 45);
            SettingsBtn.TabIndex = 11;
            SettingsBtn.Text = "Advanced Settings";
            SettingsBtn.UseVisualStyleBackColor = true;
            SettingsBtn.Click += SettingsBtn_Click;
            // 
            // IPv6Combo
            // 
            IPv6Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            IPv6Combo.FormattingEnabled = true;
            IPv6Combo.Location = new Point(186, 100);
            IPv6Combo.Name = "IPv6Combo";
            IPv6Combo.Size = new Size(496, 39);
            IPv6Combo.TabIndex = 15;
            // 
            // IPv4Combo
            // 
            IPv4Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            IPv4Combo.FormattingEnabled = true;
            IPv4Combo.Location = new Point(186, 43);
            IPv4Combo.Name = "IPv4Combo";
            IPv4Combo.Size = new Size(496, 39);
            IPv4Combo.TabIndex = 14;
            // 
            // EnableIPv6Check
            // 
            EnableIPv6Check.AutoSize = true;
            EnableIPv6Check.Location = new Point(16, 100);
            EnableIPv6Check.Name = "EnableIPv6Check";
            EnableIPv6Check.Size = new Size(153, 35);
            EnableIPv6Check.TabIndex = 13;
            EnableIPv6Check.Text = "Enable IPv6";
            EnableIPv6Check.UseVisualStyleBackColor = true;
            // 
            // EnableIPv4Check
            // 
            EnableIPv4Check.AutoSize = true;
            EnableIPv4Check.Location = new Point(16, 43);
            EnableIPv4Check.Name = "EnableIPv4Check";
            EnableIPv4Check.Size = new Size(153, 35);
            EnableIPv4Check.TabIndex = 12;
            EnableIPv4Check.Text = "Enable IPv4";
            EnableIPv4Check.UseVisualStyleBackColor = true;
            // 
            // OfferMsrpCheck
            // 
            OfferMsrpCheck.AutoSize = true;
            OfferMsrpCheck.Location = new Point(32, 173);
            OfferMsrpCheck.Name = "OfferMsrpCheck";
            OfferMsrpCheck.Size = new Size(153, 35);
            OfferMsrpCheck.TabIndex = 19;
            OfferMsrpCheck.Text = "Offer MSRP";
            OfferMsrpCheck.UseVisualStyleBackColor = true;
            // 
            // OfferRttCheck
            // 
            OfferRttCheck.AutoSize = true;
            OfferRttCheck.Location = new Point(33, 132);
            OfferRttCheck.Name = "OfferRttCheck";
            OfferRttCheck.Size = new Size(130, 35);
            OfferRttCheck.TabIndex = 18;
            OfferRttCheck.Text = "Offer RTT";
            OfferRttCheck.UseVisualStyleBackColor = true;
            // 
            // OfferVideoCheck
            // 
            OfferVideoCheck.AutoSize = true;
            OfferVideoCheck.Location = new Point(33, 91);
            OfferVideoCheck.Name = "OfferVideoCheck";
            OfferVideoCheck.Size = new Size(152, 35);
            OfferVideoCheck.TabIndex = 17;
            OfferVideoCheck.Text = "Offer Video";
            OfferVideoCheck.UseVisualStyleBackColor = true;
            // 
            // OfferAudioCheck
            // 
            OfferAudioCheck.AutoSize = true;
            OfferAudioCheck.Location = new Point(33, 50);
            OfferAudioCheck.Name = "OfferAudioCheck";
            OfferAudioCheck.Size = new Size(154, 35);
            OfferAudioCheck.TabIndex = 16;
            OfferAudioCheck.Text = "Offer Audio";
            OfferAudioCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(MsrpEncryptionCombo);
            groupBox1.Controls.Add(RtpEncryptionCombo);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label7);
            groupBox1.Location = new Point(12, 669);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(359, 185);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Media Encryption";
            // 
            // MsrpEncryptionCombo
            // 
            MsrpEncryptionCombo.DropDownHeight = 130;
            MsrpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            MsrpEncryptionCombo.FormattingEnabled = true;
            MsrpEncryptionCombo.IntegralHeight = false;
            MsrpEncryptionCombo.Items.AddRange(new object[] { "None", "MSRP/TLS" });
            MsrpEncryptionCombo.Location = new Point(163, 109);
            MsrpEncryptionCombo.Name = "MsrpEncryptionCombo";
            MsrpEncryptionCombo.Size = new Size(168, 39);
            MsrpEncryptionCombo.TabIndex = 3;
            // 
            // RtpEncryptionCombo
            // 
            RtpEncryptionCombo.DropDownHeight = 130;
            RtpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            RtpEncryptionCombo.FormattingEnabled = true;
            RtpEncryptionCombo.IntegralHeight = false;
            RtpEncryptionCombo.Items.AddRange(new object[] { "None", "SDES-SRTP", "DTLS-SRTP" });
            RtpEncryptionCombo.Location = new Point(163, 48);
            RtpEncryptionCombo.Name = "RtpEncryptionCombo";
            RtpEncryptionCombo.Size = new Size(168, 39);
            RtpEncryptionCombo.TabIndex = 2;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(19, 109);
            label10.Name = "label10";
            label10.Size = new Size(74, 31);
            label10.TabIndex = 1;
            label10.Text = "MSRP";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(19, 48);
            label7.Name = "label7";
            label7.Size = new Size(123, 31);
            label7.TabIndex = 0;
            label7.Text = "RTP Media";
            // 
            // groupBox7
            // 
            groupBox7.AutoSize = true;
            groupBox7.Controls.Add(AudioFileBrowseBtn);
            groupBox7.Controls.Add(AudioFileTb);
            groupBox7.Controls.Add(label3);
            groupBox7.Controls.Add(UseDefaultAudioCheck);
            groupBox7.Controls.Add(UseRecordedAudioCheck);
            groupBox7.Controls.Add(AudioDeviceCombo);
            groupBox7.Controls.Add(label25);
            groupBox7.Location = new Point(927, 127);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(772, 313);
            groupBox7.TabIndex = 21;
            groupBox7.TabStop = false;
            groupBox7.Text = "Audio";
            // 
            // AudioFileBrowseBtn
            // 
            AudioFileBrowseBtn.AutoSize = true;
            AudioFileBrowseBtn.Location = new Point(655, 235);
            AudioFileBrowseBtn.Name = "AudioFileBrowseBtn";
            AudioFileBrowseBtn.Size = new Size(105, 41);
            AudioFileBrowseBtn.TabIndex = 6;
            AudioFileBrowseBtn.Text = "Browse";
            AudioFileBrowseBtn.UseVisualStyleBackColor = true;
            AudioFileBrowseBtn.Click += AudioFileBrowseBtn_Click;
            // 
            // AudioFileTb
            // 
            AudioFileTb.Location = new Point(194, 233);
            AudioFileTb.Name = "AudioFileTb";
            AudioFileTb.Size = new Size(444, 38);
            AudioFileTb.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 235);
            label3.Name = "label3";
            label3.Size = new Size(159, 31);
            label3.TabIndex = 4;
            label3.Text = "Recording File";
            // 
            // UseDefaultAudioCheck
            // 
            UseDefaultAudioCheck.AutoSize = true;
            UseDefaultAudioCheck.Location = new Point(14, 172);
            UseDefaultAudioCheck.Name = "UseDefaultAudioCheck";
            UseDefaultAudioCheck.Size = new Size(331, 35);
            UseDefaultAudioCheck.TabIndex = 3;
            UseDefaultAudioCheck.Text = "Use Default Audio Recording";
            UseDefaultAudioCheck.UseVisualStyleBackColor = true;
            // 
            // UseRecordedAudioCheck
            // 
            UseRecordedAudioCheck.AutoSize = true;
            UseRecordedAudioCheck.Location = new Point(14, 113);
            UseRecordedAudioCheck.Name = "UseRecordedAudioCheck";
            UseRecordedAudioCheck.Size = new Size(244, 35);
            UseRecordedAudioCheck.TabIndex = 2;
            UseRecordedAudioCheck.Text = "Use Recorded Audio";
            UseRecordedAudioCheck.UseVisualStyleBackColor = true;
            // 
            // AudioDeviceCombo
            // 
            AudioDeviceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            AudioDeviceCombo.FormattingEnabled = true;
            AudioDeviceCombo.Location = new Point(194, 50);
            AudioDeviceCombo.Name = "AudioDeviceCombo";
            AudioDeviceCombo.Size = new Size(555, 39);
            AudioDeviceCombo.TabIndex = 1;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(14, 53);
            label25.Name = "label25";
            label25.Size = new Size(149, 31);
            label25.TabIndex = 0;
            label25.Text = "Audio Device";
            // 
            // groupBox8
            // 
            groupBox8.AutoSize = true;
            groupBox8.Controls.Add(VideoDevicesCombo);
            groupBox8.Controls.Add(label26);
            groupBox8.Controls.Add(VideoListView);
            groupBox8.Location = new Point(927, 464);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(772, 353);
            groupBox8.TabIndex = 22;
            groupBox8.TabStop = false;
            groupBox8.Text = "Video";
            // 
            // VideoDevicesCombo
            // 
            VideoDevicesCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            VideoDevicesCombo.FormattingEnabled = true;
            VideoDevicesCombo.Location = new Point(202, 49);
            VideoDevicesCombo.Name = "VideoDevicesCombo";
            VideoDevicesCombo.Size = new Size(547, 39);
            VideoDevicesCombo.TabIndex = 2;
            VideoDevicesCombo.SelectedIndexChanged += VideoDevicesCombo_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(25, 52);
            label26.Name = "label26";
            label26.Size = new Size(147, 31);
            label26.TabIndex = 1;
            label26.Text = "Video Device";
            // 
            // VideoListView
            // 
            VideoListView.CheckBoxes = true;
            VideoListView.Columns.AddRange(new ColumnHeader[] { SubTypeHeader, WidthHeader, HeightHeader, FpsHeader });
            VideoListView.FullRowSelect = true;
            VideoListView.GridLines = true;
            VideoListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            VideoListView.HideSelection = true;
            VideoListView.Location = new Point(20, 106);
            VideoListView.MultiSelect = false;
            VideoListView.Name = "VideoListView";
            VideoListView.Size = new Size(729, 210);
            VideoListView.TabIndex = 0;
            VideoListView.UseCompatibleStateImageBehavior = false;
            VideoListView.View = View.Details;
            VideoListView.ItemCheck += VideoListView_ItemCheck;
            // 
            // SubTypeHeader
            // 
            SubTypeHeader.Text = "Sub Type";
            SubTypeHeader.Width = 100;
            // 
            // WidthHeader
            // 
            WidthHeader.Text = "Width";
            WidthHeader.Width = 100;
            // 
            // HeightHeader
            // 
            HeightHeader.Text = "Height";
            HeightHeader.Width = 100;
            // 
            // FpsHeader
            // 
            FpsHeader.Text = "Frames/Sec.";
            FpsHeader.Width = 100;
            // 
            // groupBox2
            // 
            groupBox2.AutoSize = true;
            groupBox2.Controls.Add(OfferCpimCheck);
            groupBox2.Controls.Add(OfferAudioCheck);
            groupBox2.Controls.Add(OfferVideoCheck);
            groupBox2.Controls.Add(OfferRttCheck);
            groupBox2.Controls.Add(OfferMsrpCheck);
            groupBox2.Location = new Point(12, 318);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(359, 332);
            groupBox2.TabIndex = 34;
            groupBox2.TabStop = false;
            groupBox2.Text = "Offer Media";
            // 
            // OfferCpimCheck
            // 
            OfferCpimCheck.AutoSize = true;
            OfferCpimCheck.Location = new Point(69, 214);
            OfferCpimCheck.Name = "OfferCpimCheck";
            OfferCpimCheck.Size = new Size(234, 35);
            OfferCpimCheck.TabIndex = 21;
            OfferCpimCheck.Text = "Use Message/CPIM";
            OfferCpimCheck.UseVisualStyleBackColor = true;
            // 
            // AddDataByReferenceCheck
            // 
            AddDataByReferenceCheck.AutoSize = true;
            AddDataByReferenceCheck.Location = new Point(34, 240);
            AddDataByReferenceCheck.Name = "AddDataByReferenceCheck";
            AddDataByReferenceCheck.Size = new Size(336, 35);
            AddDataByReferenceCheck.TabIndex = 39;
            AddDataByReferenceCheck.Text = "Additional Data By-Reference";
            AddDataByReferenceCheck.UseVisualStyleBackColor = true;
            // 
            // AddDataByValueCheck
            // 
            AddDataByValueCheck.AutoSize = true;
            AddDataByValueCheck.Location = new Point(34, 191);
            AddDataByValueCheck.Name = "AddDataByValueCheck";
            AddDataByValueCheck.Size = new Size(291, 35);
            AddDataByValueCheck.TabIndex = 38;
            AddDataByValueCheck.Text = "Additional Data By-Value";
            AddDataByValueCheck.UseVisualStyleBackColor = true;
            // 
            // SipPresenceCheck
            // 
            SipPresenceCheck.AutoSize = true;
            SipPresenceCheck.Location = new Point(34, 142);
            SipPresenceCheck.Name = "SipPresenceCheck";
            SipPresenceCheck.Size = new Size(355, 35);
            SipPresenceCheck.TabIndex = 37;
            SipPresenceCheck.Text = "Location By SIP Presence Event ";
            SipPresenceCheck.UseVisualStyleBackColor = true;
            // 
            // LocationByReferenceCheck
            // 
            LocationByReferenceCheck.AutoSize = true;
            LocationByReferenceCheck.Location = new Point(34, 93);
            LocationByReferenceCheck.Name = "LocationByReferenceCheck";
            LocationByReferenceCheck.Size = new Size(262, 35);
            LocationByReferenceCheck.TabIndex = 36;
            LocationByReferenceCheck.Text = "Location By-Reference";
            LocationByReferenceCheck.UseVisualStyleBackColor = true;
            // 
            // LocationByValueCheck
            // 
            LocationByValueCheck.AutoSize = true;
            LocationByValueCheck.Location = new Point(34, 44);
            LocationByValueCheck.Name = "LocationByValueCheck";
            LocationByValueCheck.Size = new Size(217, 35);
            LocationByValueCheck.TabIndex = 35;
            LocationByValueCheck.Text = "Location By-Value";
            LocationByValueCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.Controls.Add(IPv4Combo);
            groupBox3.Controls.Add(EnableIPv4Check);
            groupBox3.Controls.Add(EnableIPv6Check);
            groupBox3.Controls.Add(IPv6Combo);
            groupBox3.Controls.Add(PreferIPv6CheckBox);
            groupBox3.Location = new Point(12, 127);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(867, 176);
            groupBox3.TabIndex = 40;
            groupBox3.TabStop = false;
            groupBox3.Text = "Local IP Addresses";
            // 
            // groupBox4
            // 
            groupBox4.AutoSize = true;
            groupBox4.Controls.Add(LocationByValueCheck);
            groupBox4.Controls.Add(LocationByReferenceCheck);
            groupBox4.Controls.Add(AddDataByReferenceCheck);
            groupBox4.Controls.Add(SipPresenceCheck);
            groupBox4.Controls.Add(AddDataByValueCheck);
            groupBox4.Location = new Point(387, 318);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(492, 332);
            groupBox4.TabIndex = 41;
            groupBox4.TabStop = false;
            groupBox4.Text = "Location and Additional Data";
            // 
            // ChangeVideoBtn
            // 
            ChangeVideoBtn.AutoSize = true;
            ChangeVideoBtn.Location = new Point(593, 806);
            ChangeVideoBtn.Name = "ChangeVideoBtn";
            ChangeVideoBtn.Size = new Size(150, 41);
            ChangeVideoBtn.TabIndex = 47;
            ChangeVideoBtn.Text = "Change";
            ChangeVideoBtn.UseVisualStyleBackColor = true;
            ChangeVideoBtn.Click += ChangeVideoBtn_Click;
            // 
            // ChangeAudioBtn
            // 
            ChangeAudioBtn.AutoSize = true;
            ChangeAudioBtn.Location = new Point(387, 806);
            ChangeAudioBtn.Name = "ChangeAudioBtn";
            ChangeAudioBtn.Size = new Size(150, 41);
            ChangeAudioBtn.TabIndex = 46;
            ChangeAudioBtn.Text = "Change";
            ChangeAudioBtn.UseVisualStyleBackColor = true;
            ChangeAudioBtn.Click += ChangeAudioBtn_Click;
            // 
            // OfferVideoList
            // 
            OfferVideoList.FormattingEnabled = true;
            OfferVideoList.Location = new Point(596, 703);
            OfferVideoList.Name = "OfferVideoList";
            OfferVideoList.Size = new Size(150, 97);
            OfferVideoList.TabIndex = 45;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(593, 669);
            label6.Name = "label6";
            label6.Size = new Size(153, 31);
            label6.TabIndex = 44;
            label6.Text = "Video Codecs";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(387, 669);
            label5.Name = "label5";
            label5.Size = new Size(155, 31);
            label5.TabIndex = 42;
            label5.Text = "Audio Codecs";
            // 
            // OfferAudioList
            // 
            OfferAudioList.FormattingEnabled = true;
            OfferAudioList.Location = new Point(387, 703);
            OfferAudioList.Name = "OfferAudioList";
            OfferAudioList.Size = new Size(150, 97);
            OfferAudioList.TabIndex = 43;
            // 
            // FromNumberCombo
            // 
            FromNumberCombo.DropDownHeight = 150;
            FromNumberCombo.FormattingEnabled = true;
            FromNumberCombo.IntegralHeight = false;
            FromNumberCombo.Location = new Point(206, 66);
            FromNumberCombo.Name = "FromNumberCombo";
            FromNumberCombo.Size = new Size(248, 39);
            FromNumberCombo.Sorted = true;
            FromNumberCombo.TabIndex = 51;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 74);
            label2.Name = "label2";
            label2.Size = new Size(156, 31);
            label2.TabIndex = 50;
            label2.Text = "From Number";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(121, 31);
            label1.TabIndex = 49;
            label1.Text = "To SIP URI:";
            // 
            // ToSipUriTb
            // 
            ToSipUriTb.Location = new Point(204, 13);
            ToSipUriTb.Name = "ToSipUriTb";
            ToSipUriTb.Size = new Size(818, 38);
            ToSipUriTb.TabIndex = 48;
            // 
            // UseUrnCheckBox
            // 
            UseUrnCheckBox.AutoSize = true;
            UseUrnCheckBox.Location = new Point(1039, 15);
            UseUrnCheckBox.Name = "UseUrnCheckBox";
            UseUrnCheckBox.Size = new Size(227, 35);
            UseUrnCheckBox.TabIndex = 52;
            UseUrnCheckBox.Text = "Use urn:service:sos";
            UseUrnCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new Point(1450, 894);
            button1.Name = "button1";
            button1.Size = new Size(110, 41);
            button1.TabIndex = 53;
            button1.Text = "Help";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // UseTelUriCheck
            // 
            UseTelUriCheck.AutoSize = true;
            UseTelUriCheck.Location = new Point(479, 68);
            UseTelUriCheck.Name = "UseTelUriCheck";
            UseTelUriCheck.Size = new Size(150, 35);
            UseTelUriCheck.TabIndex = 54;
            UseTelUriCheck.Text = "Use Tel URI";
            UseTelUriCheck.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1708, 953);
            Controls.Add(UseTelUriCheck);
            Controls.Add(button1);
            Controls.Add(UseUrnCheckBox);
            Controls.Add(FromNumberCombo);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ToSipUriTb);
            Controls.Add(ChangeVideoBtn);
            Controls.Add(ChangeAudioBtn);
            Controls.Add(OfferVideoList);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(OfferAudioList);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox8);
            Controls.Add(groupBox7);
            Controls.Add(groupBox1);
            Controls.Add(SettingsBtn);
            Controls.Add(StartServerBtn);
            Controls.Add(CallStatusLabel);
            Controls.Add(CloseBtn);
            Controls.Add(CallBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OspSimulator";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button CallBtn;
        private Button CloseBtn;
        private CheckBox PreferIPv6CheckBox;
        private Label CallStatusLabel;
        private Button StartServerBtn;
        private Button SettingsBtn;
        private ComboBox IPv6Combo;
        private ComboBox IPv4Combo;
        private CheckBox EnableIPv6Check;
        private CheckBox EnableIPv4Check;
        private CheckBox OfferMsrpCheck;
        private CheckBox OfferRttCheck;
        private CheckBox OfferVideoCheck;
        private CheckBox OfferAudioCheck;
        private GroupBox groupBox1;
        private ComboBox MsrpEncryptionCombo;
        private ComboBox RtpEncryptionCombo;
        private Label label10;
        private Label label7;
        private GroupBox groupBox7;
        private ComboBox AudioDeviceCombo;
        private Label label25;
        private GroupBox groupBox8;
        private ComboBox VideoDevicesCombo;
        private Label label26;
        private ListView VideoListView;
        private ColumnHeader SubTypeHeader;
        private ColumnHeader WidthHeader;
        private ColumnHeader HeightHeader;
        private ColumnHeader FpsHeader;
        private GroupBox groupBox2;
        private CheckBox OfferCpimCheck;
        private CheckBox AddDataByReferenceCheck;
        private CheckBox AddDataByValueCheck;
        private CheckBox SipPresenceCheck;
        private CheckBox LocationByReferenceCheck;
        private CheckBox LocationByValueCheck;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Button ChangeVideoBtn;
        private Button ChangeAudioBtn;
        private ListBox OfferVideoList;
        private Label label6;
        private Label label5;
        private ListBox OfferAudioList;
        private ComboBox FromNumberCombo;
        private Label label2;
        private Label label1;
        private TextBox ToSipUriTb;
        private CheckBox UseUrnCheckBox;
        private Button button1;
        private CheckBox UseTelUriCheck;
        private CheckBox UseRecordedAudioCheck;
        private TextBox AudioFileTb;
        private Label label3;
        private CheckBox UseDefaultAudioCheck;
        private Button AudioFileBrowseBtn;
    }
}
