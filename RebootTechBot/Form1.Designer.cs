namespace RebootTechBot
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ChatPage = new System.Windows.Forms.TabPage();
            this.btnCensorTimeout = new System.Windows.Forms.Button();
            this.btnTimeout = new System.Windows.Forms.Button();
            this.btnBan = new System.Windows.Forms.Button();
            this.txtChatMessage = new System.Windows.Forms.TextBox();
            this.btnChat = new System.Windows.Forms.Button();
            this.lstChatUsers = new System.Windows.Forms.ListView();
            this.UserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChatWindow = new System.Windows.Forms.WebBrowser();
            this.LogPage = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.ModerationPage = new System.Windows.Forms.TabPage();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.ddModerationAction3 = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.ddModerationAction2 = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.ddModerationAction1 = new System.Windows.Forms.ComboBox();
            this.btnCancelModerationChanges = new System.Windows.Forms.Button();
            this.btnApplyModerationChanges = new System.Windows.Forms.Button();
            this.chkModExceptionModerators = new System.Windows.Forms.CheckBox();
            this.chkModExceptionSubscribers = new System.Windows.Forms.CheckBox();
            this.chkModExceptionVIP = new System.Windows.Forms.CheckBox();
            this.chkModExceptionRegular = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtModExceptionViewerHours = new System.Windows.Forms.TextBox();
            this.chkModExceptionFollowerForHours = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtModExceptionViewerSeconds = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkModExceptionViewerInChatDuration = new System.Windows.Forms.CheckBox();
            this.chkExceptionModeration = new System.Windows.Forms.CheckBox();
            this.btnBannedWordsRem = new System.Windows.Forms.Button();
            this.btnBannedWordsAdd = new System.Windows.Forms.Button();
            this.lvBannedWords = new System.Windows.Forms.ListView();
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSpamModerationTimeCount = new System.Windows.Forms.TextBox();
            this.chkBannedWordModeration = new System.Windows.Forms.CheckBox();
            this.chkColorModeration = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMsgLengthModerationMaxLength = new System.Windows.Forms.TextBox();
            this.chkMsgLengthModeration = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtEmotesModerationMinLength = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtEmotesModerationPercentage = new System.Windows.Forms.TextBox();
            this.chkEmotesModeration = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSymbolsModerationMaxGroupItems = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSymbolsModerationMinLength = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSymbolsModerationPercentage = new System.Windows.Forms.TextBox();
            this.chkSymbolsModeration = new System.Windows.Forms.CheckBox();
            this.chkSpamModerationAllUsers = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ddModerationPreset = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCapsModerationMinLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpamModerationMessageCount = new System.Windows.Forms.TextBox();
            this.chkSpamModeration = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCapsModerationPercentage = new System.Windows.Forms.TextBox();
            this.chkCapsModeration = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.ChatPage.SuspendLayout();
            this.LogPage.SuspendLayout();
            this.ModerationPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ChatPage);
            this.tabControl1.Controls.Add(this.LogPage);
            this.tabControl1.Controls.Add(this.ModerationPage);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(574, 478);
            this.tabControl1.TabIndex = 2;
            // 
            // ChatPage
            // 
            this.ChatPage.Controls.Add(this.btnCensorTimeout);
            this.ChatPage.Controls.Add(this.btnTimeout);
            this.ChatPage.Controls.Add(this.btnBan);
            this.ChatPage.Controls.Add(this.txtChatMessage);
            this.ChatPage.Controls.Add(this.btnChat);
            this.ChatPage.Controls.Add(this.lstChatUsers);
            this.ChatPage.Controls.Add(this.ChatWindow);
            this.ChatPage.Location = new System.Drawing.Point(4, 22);
            this.ChatPage.Name = "ChatPage";
            this.ChatPage.Padding = new System.Windows.Forms.Padding(3);
            this.ChatPage.Size = new System.Drawing.Size(566, 452);
            this.ChatPage.TabIndex = 1;
            this.ChatPage.Text = "Chat";
            this.ChatPage.UseVisualStyleBackColor = true;
            // 
            // btnCensorTimeout
            // 
            this.btnCensorTimeout.Location = new System.Drawing.Point(473, 386);
            this.btnCensorTimeout.Name = "btnCensorTimeout";
            this.btnCensorTimeout.Size = new System.Drawing.Size(87, 23);
            this.btnCensorTimeout.TabIndex = 6;
            this.btnCensorTimeout.Text = "CensorTimeout";
            this.btnCensorTimeout.UseVisualStyleBackColor = true;
            this.btnCensorTimeout.Click += new System.EventHandler(this.btnCensorTimeout_Click);
            // 
            // btnTimeout
            // 
            this.btnTimeout.Location = new System.Drawing.Point(399, 386);
            this.btnTimeout.Name = "btnTimeout";
            this.btnTimeout.Size = new System.Drawing.Size(75, 23);
            this.btnTimeout.TabIndex = 5;
            this.btnTimeout.Text = "24h Timeout";
            this.btnTimeout.UseVisualStyleBackColor = true;
            this.btnTimeout.Click += new System.EventHandler(this.btnTimeout_Click);
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point(350, 386);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(54, 23);
            this.btnBan.TabIndex = 4;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler(this.btnBan_Click);
            // 
            // txtChatMessage
            // 
            this.txtChatMessage.Location = new System.Drawing.Point(7, 426);
            this.txtChatMessage.Name = "txtChatMessage";
            this.txtChatMessage.Size = new System.Drawing.Size(255, 20);
            this.txtChatMessage.TabIndex = 3;
            // 
            // btnChat
            // 
            this.btnChat.Location = new System.Drawing.Point(268, 423);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(75, 23);
            this.btnChat.TabIndex = 2;
            this.btnChat.Text = "Chat";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // lstChatUsers
            // 
            this.lstChatUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserName});
            this.lstChatUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstChatUsers.Location = new System.Drawing.Point(347, 0);
            this.lstChatUsers.Name = "lstChatUsers";
            this.lstChatUsers.ShowItemToolTips = true;
            this.lstChatUsers.Size = new System.Drawing.Size(213, 379);
            this.lstChatUsers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstChatUsers.TabIndex = 1;
            this.lstChatUsers.UseCompatibleStateImageBehavior = false;
            this.lstChatUsers.View = System.Windows.Forms.View.Details;
            // 
            // UserName
            // 
            this.UserName.Tag = "Name";
            this.UserName.Text = "Name";
            this.UserName.Width = 180;
            // 
            // ChatWindow
            // 
            this.ChatWindow.Location = new System.Drawing.Point(3, 6);
            this.ChatWindow.MinimumSize = new System.Drawing.Size(20, 20);
            this.ChatWindow.Name = "ChatWindow";
            this.ChatWindow.Size = new System.Drawing.Size(340, 411);
            this.ChatWindow.TabIndex = 0;
            // 
            // LogPage
            // 
            this.LogPage.Controls.Add(this.txtLog);
            this.LogPage.Location = new System.Drawing.Point(4, 22);
            this.LogPage.Name = "LogPage";
            this.LogPage.Padding = new System.Windows.Forms.Padding(3);
            this.LogPage.Size = new System.Drawing.Size(566, 452);
            this.LogPage.TabIndex = 0;
            this.LogPage.Text = "Log";
            this.LogPage.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(560, 446);
            this.txtLog.TabIndex = 0;
            // 
            // ModerationPage
            // 
            this.ModerationPage.Controls.Add(this.label29);
            this.ModerationPage.Controls.Add(this.label30);
            this.ModerationPage.Controls.Add(this.label28);
            this.ModerationPage.Controls.Add(this.ddModerationAction3);
            this.ModerationPage.Controls.Add(this.label27);
            this.ModerationPage.Controls.Add(this.ddModerationAction2);
            this.ModerationPage.Controls.Add(this.label26);
            this.ModerationPage.Controls.Add(this.ddModerationAction1);
            this.ModerationPage.Controls.Add(this.btnCancelModerationChanges);
            this.ModerationPage.Controls.Add(this.btnApplyModerationChanges);
            this.ModerationPage.Controls.Add(this.chkModExceptionModerators);
            this.ModerationPage.Controls.Add(this.chkModExceptionSubscribers);
            this.ModerationPage.Controls.Add(this.chkModExceptionVIP);
            this.ModerationPage.Controls.Add(this.chkModExceptionRegular);
            this.ModerationPage.Controls.Add(this.label25);
            this.ModerationPage.Controls.Add(this.txtModExceptionViewerHours);
            this.ModerationPage.Controls.Add(this.chkModExceptionFollowerForHours);
            this.ModerationPage.Controls.Add(this.label19);
            this.ModerationPage.Controls.Add(this.txtModExceptionViewerSeconds);
            this.ModerationPage.Controls.Add(this.label15);
            this.ModerationPage.Controls.Add(this.chkModExceptionViewerInChatDuration);
            this.ModerationPage.Controls.Add(this.chkExceptionModeration);
            this.ModerationPage.Controls.Add(this.btnBannedWordsRem);
            this.ModerationPage.Controls.Add(this.btnBannedWordsAdd);
            this.ModerationPage.Controls.Add(this.lvBannedWords);
            this.ModerationPage.Controls.Add(this.label24);
            this.ModerationPage.Controls.Add(this.label23);
            this.ModerationPage.Controls.Add(this.label22);
            this.ModerationPage.Controls.Add(this.label21);
            this.ModerationPage.Controls.Add(this.label20);
            this.ModerationPage.Controls.Add(this.label18);
            this.ModerationPage.Controls.Add(this.label17);
            this.ModerationPage.Controls.Add(this.label16);
            this.ModerationPage.Controls.Add(this.txtSpamModerationTimeCount);
            this.ModerationPage.Controls.Add(this.chkBannedWordModeration);
            this.ModerationPage.Controls.Add(this.chkColorModeration);
            this.ModerationPage.Controls.Add(this.label14);
            this.ModerationPage.Controls.Add(this.txtMsgLengthModerationMaxLength);
            this.ModerationPage.Controls.Add(this.chkMsgLengthModeration);
            this.ModerationPage.Controls.Add(this.label11);
            this.ModerationPage.Controls.Add(this.txtEmotesModerationMinLength);
            this.ModerationPage.Controls.Add(this.label12);
            this.ModerationPage.Controls.Add(this.label13);
            this.ModerationPage.Controls.Add(this.txtEmotesModerationPercentage);
            this.ModerationPage.Controls.Add(this.chkEmotesModeration);
            this.ModerationPage.Controls.Add(this.label10);
            this.ModerationPage.Controls.Add(this.txtSymbolsModerationMaxGroupItems);
            this.ModerationPage.Controls.Add(this.label7);
            this.ModerationPage.Controls.Add(this.txtSymbolsModerationMinLength);
            this.ModerationPage.Controls.Add(this.label8);
            this.ModerationPage.Controls.Add(this.label9);
            this.ModerationPage.Controls.Add(this.txtSymbolsModerationPercentage);
            this.ModerationPage.Controls.Add(this.chkSymbolsModeration);
            this.ModerationPage.Controls.Add(this.chkSpamModerationAllUsers);
            this.ModerationPage.Controls.Add(this.label6);
            this.ModerationPage.Controls.Add(this.label3);
            this.ModerationPage.Controls.Add(this.ddModerationPreset);
            this.ModerationPage.Controls.Add(this.label5);
            this.ModerationPage.Controls.Add(this.txtCapsModerationMinLength);
            this.ModerationPage.Controls.Add(this.label4);
            this.ModerationPage.Controls.Add(this.txtSpamModerationMessageCount);
            this.ModerationPage.Controls.Add(this.chkSpamModeration);
            this.ModerationPage.Controls.Add(this.label2);
            this.ModerationPage.Controls.Add(this.label1);
            this.ModerationPage.Controls.Add(this.txtCapsModerationPercentage);
            this.ModerationPage.Controls.Add(this.chkCapsModeration);
            this.ModerationPage.Location = new System.Drawing.Point(4, 22);
            this.ModerationPage.Name = "ModerationPage";
            this.ModerationPage.Size = new System.Drawing.Size(566, 452);
            this.ModerationPage.TabIndex = 2;
            this.ModerationPage.Text = "Moderation";
            this.ModerationPage.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label29.Location = new System.Drawing.Point(311, 343);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(120, 2);
            this.label29.TabIndex = 68;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(308, 331);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(120, 13);
            this.label30.TabIndex = 67;
            this.label30.Text = "Moderation Actions:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(315, 395);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(98, 13);
            this.label28.TabIndex = 66;
            this.label28.Text = "3rd Offense Action:";
            // 
            // ddModerationAction3
            // 
            this.ddModerationAction3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddModerationAction3.FormattingEnabled = true;
            this.ddModerationAction3.Items.AddRange(new object[] {
            "Ignore - highlight",
            "Warning",
            "Purge",
            "1 minute timeout",
            "5 minute timeout",
            "10 minute timeout",
            "30 minutes timeout",
            "1 hour timeout",
            "5 hour timeout",
            "10 hour timeout",
            "15 hour timeout",
            "1 day timeout",
            "1 week timeout",
            "Perma ban"});
            this.ddModerationAction3.Location = new System.Drawing.Point(417, 392);
            this.ddModerationAction3.Name = "ddModerationAction3";
            this.ddModerationAction3.Size = new System.Drawing.Size(121, 21);
            this.ddModerationAction3.TabIndex = 65;
            this.ddModerationAction3.SelectedIndexChanged += new System.EventHandler(this.ddModerationAction3_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(315, 372);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(101, 13);
            this.label27.TabIndex = 64;
            this.label27.Text = "2nd Offense Action:";
            // 
            // ddModerationAction2
            // 
            this.ddModerationAction2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddModerationAction2.FormattingEnabled = true;
            this.ddModerationAction2.Items.AddRange(new object[] {
            "Ignore - highlight",
            "Warning",
            "Purge",
            "1 minute timeout",
            "5 minute timeout",
            "10 minute timeout",
            "30 minutes timeout",
            "1 hour timeout",
            "5 hour timeout",
            "10 hour timeout",
            "15 hour timeout",
            "1 day timeout",
            "1 week timeout",
            "Perma ban"});
            this.ddModerationAction2.Location = new System.Drawing.Point(417, 369);
            this.ddModerationAction2.Name = "ddModerationAction2";
            this.ddModerationAction2.Size = new System.Drawing.Size(121, 21);
            this.ddModerationAction2.TabIndex = 63;
            this.ddModerationAction2.SelectedIndexChanged += new System.EventHandler(this.ddModerationAction2_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(315, 349);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(97, 13);
            this.label26.TabIndex = 62;
            this.label26.Text = "1st Offense Action:";
            // 
            // ddModerationAction1
            // 
            this.ddModerationAction1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddModerationAction1.FormattingEnabled = true;
            this.ddModerationAction1.Items.AddRange(new object[] {
            "Ignore - highlight",
            "Warning",
            "Purge",
            "1 minute timeout",
            "5 minute timeout",
            "10 minute timeout",
            "30 minutes timeout",
            "1 hour timeout",
            "5 hour timeout",
            "10 hour timeout",
            "15 hour timeout",
            "1 day timeout",
            "1 week timeout",
            "Perma ban"});
            this.ddModerationAction1.Location = new System.Drawing.Point(417, 346);
            this.ddModerationAction1.Name = "ddModerationAction1";
            this.ddModerationAction1.Size = new System.Drawing.Size(121, 21);
            this.ddModerationAction1.TabIndex = 61;
            this.ddModerationAction1.SelectedIndexChanged += new System.EventHandler(this.ddModerationAction1_SelectedIndexChanged);
            // 
            // btnCancelModerationChanges
            // 
            this.btnCancelModerationChanges.Location = new System.Drawing.Point(299, 423);
            this.btnCancelModerationChanges.Name = "btnCancelModerationChanges";
            this.btnCancelModerationChanges.Size = new System.Drawing.Size(163, 23);
            this.btnCancelModerationChanges.TabIndex = 60;
            this.btnCancelModerationChanges.Text = "Cancel Moderation Changes";
            this.btnCancelModerationChanges.UseVisualStyleBackColor = true;
            this.btnCancelModerationChanges.Click += new System.EventHandler(this.btnCancelModerationChanges_Click);
            // 
            // btnApplyModerationChanges
            // 
            this.btnApplyModerationChanges.Location = new System.Drawing.Point(130, 423);
            this.btnApplyModerationChanges.Name = "btnApplyModerationChanges";
            this.btnApplyModerationChanges.Size = new System.Drawing.Size(163, 23);
            this.btnApplyModerationChanges.TabIndex = 59;
            this.btnApplyModerationChanges.Text = "Apply Moderation Changes";
            this.btnApplyModerationChanges.UseVisualStyleBackColor = true;
            this.btnApplyModerationChanges.Click += new System.EventHandler(this.btnApplyModerationChanges_Click);
            // 
            // chkModExceptionModerators
            // 
            this.chkModExceptionModerators.AutoSize = true;
            this.chkModExceptionModerators.Location = new System.Drawing.Point(320, 306);
            this.chkModExceptionModerators.Name = "chkModExceptionModerators";
            this.chkModExceptionModerators.Size = new System.Drawing.Size(52, 17);
            this.chkModExceptionModerators.TabIndex = 58;
            this.chkModExceptionModerators.Text = "Mods";
            this.chkModExceptionModerators.UseVisualStyleBackColor = true;
            this.chkModExceptionModerators.CheckedChanged += new System.EventHandler(this.chkModExceptionModerators_CheckedChanged);
            // 
            // chkModExceptionSubscribers
            // 
            this.chkModExceptionSubscribers.AutoSize = true;
            this.chkModExceptionSubscribers.Location = new System.Drawing.Point(320, 289);
            this.chkModExceptionSubscribers.Name = "chkModExceptionSubscribers";
            this.chkModExceptionSubscribers.Size = new System.Drawing.Size(81, 17);
            this.chkModExceptionSubscribers.TabIndex = 57;
            this.chkModExceptionSubscribers.Text = "Subscribers";
            this.chkModExceptionSubscribers.UseVisualStyleBackColor = true;
            this.chkModExceptionSubscribers.CheckedChanged += new System.EventHandler(this.chkModExceptionSubscribers_CheckedChanged);
            // 
            // chkModExceptionVIP
            // 
            this.chkModExceptionVIP.AutoSize = true;
            this.chkModExceptionVIP.Location = new System.Drawing.Point(320, 272);
            this.chkModExceptionVIP.Name = "chkModExceptionVIP";
            this.chkModExceptionVIP.Size = new System.Drawing.Size(43, 17);
            this.chkModExceptionVIP.TabIndex = 56;
            this.chkModExceptionVIP.Text = "VIP";
            this.chkModExceptionVIP.UseVisualStyleBackColor = true;
            this.chkModExceptionVIP.CheckedChanged += new System.EventHandler(this.chkModExceptionVIP_CheckedChanged);
            // 
            // chkModExceptionRegular
            // 
            this.chkModExceptionRegular.AutoSize = true;
            this.chkModExceptionRegular.Location = new System.Drawing.Point(320, 254);
            this.chkModExceptionRegular.Name = "chkModExceptionRegular";
            this.chkModExceptionRegular.Size = new System.Drawing.Size(63, 17);
            this.chkModExceptionRegular.TabIndex = 55;
            this.chkModExceptionRegular.Text = "Regular";
            this.chkModExceptionRegular.UseVisualStyleBackColor = true;
            this.chkModExceptionRegular.CheckedChanged += new System.EventHandler(this.chkModExceptionRegular_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(439, 237);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(36, 13);
            this.label25.TabIndex = 54;
            this.label25.Text = "hours.";
            // 
            // txtModExceptionViewerHours
            // 
            this.txtModExceptionViewerHours.Location = new System.Drawing.Point(398, 235);
            this.txtModExceptionViewerHours.Name = "txtModExceptionViewerHours";
            this.txtModExceptionViewerHours.Size = new System.Drawing.Size(36, 20);
            this.txtModExceptionViewerHours.TabIndex = 53;
            this.txtModExceptionViewerHours.TextChanged += new System.EventHandler(this.txtModExceptionViewerHours_TextChanged);
            // 
            // chkModExceptionFollowerForHours
            // 
            this.chkModExceptionFollowerForHours.AutoSize = true;
            this.chkModExceptionFollowerForHours.Location = new System.Drawing.Point(320, 236);
            this.chkModExceptionFollowerForHours.Name = "chkModExceptionFollowerForHours";
            this.chkModExceptionFollowerForHours.Size = new System.Drawing.Size(80, 17);
            this.chkModExceptionFollowerForHours.TabIndex = 52;
            this.chkModExceptionFollowerForHours.Text = "Follower for";
            this.chkModExceptionFollowerForHours.UseVisualStyleBackColor = true;
            this.chkModExceptionFollowerForHours.CheckedChanged += new System.EventHandler(this.chkModExceptionFollowerForHours_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(467, 218);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 13);
            this.label19.TabIndex = 51;
            this.label19.Text = "seconds.";
            // 
            // txtModExceptionViewerSeconds
            // 
            this.txtModExceptionViewerSeconds.Location = new System.Drawing.Point(425, 215);
            this.txtModExceptionViewerSeconds.Name = "txtModExceptionViewerSeconds";
            this.txtModExceptionViewerSeconds.Size = new System.Drawing.Size(36, 20);
            this.txtModExceptionViewerSeconds.TabIndex = 50;
            this.txtModExceptionViewerSeconds.TextChanged += new System.EventHandler(this.txtModExceptionViewerSeconds_TextChanged);
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label15.Location = new System.Drawing.Point(311, 214);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(110, 2);
            this.label15.TabIndex = 49;
            // 
            // chkModExceptionViewerInChatDuration
            // 
            this.chkModExceptionViewerInChatDuration.AutoSize = true;
            this.chkModExceptionViewerInChatDuration.Location = new System.Drawing.Point(320, 217);
            this.chkModExceptionViewerInChatDuration.Name = "chkModExceptionViewerInChatDuration";
            this.chkModExceptionViewerInChatDuration.Size = new System.Drawing.Size(108, 17);
            this.chkModExceptionViewerInChatDuration.TabIndex = 48;
            this.chkModExceptionViewerInChatDuration.Text = "Viewer in chat for";
            this.chkModExceptionViewerInChatDuration.UseVisualStyleBackColor = true;
            this.chkModExceptionViewerInChatDuration.CheckedChanged += new System.EventHandler(this.chkModExceptionViewerInChatDuration_CheckedChanged);
            // 
            // chkExceptionModeration
            // 
            this.chkExceptionModeration.AutoSize = true;
            this.chkExceptionModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExceptionModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExceptionModeration.Location = new System.Drawing.Point(306, 199);
            this.chkExceptionModeration.Name = "chkExceptionModeration";
            this.chkExceptionModeration.Size = new System.Drawing.Size(159, 17);
            this.chkExceptionModeration.TabIndex = 47;
            this.chkExceptionModeration.Text = "Moderation Exceptions:";
            this.chkExceptionModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExceptionModeration.UseVisualStyleBackColor = true;
            this.chkExceptionModeration.CheckedChanged += new System.EventHandler(this.chkExceptionModeration_CheckedChanged);
            // 
            // btnBannedWordsRem
            // 
            this.btnBannedWordsRem.Location = new System.Drawing.Point(129, 378);
            this.btnBannedWordsRem.Name = "btnBannedWordsRem";
            this.btnBannedWordsRem.Size = new System.Drawing.Size(99, 23);
            this.btnBannedWordsRem.TabIndex = 46;
            this.btnBannedWordsRem.Text = "Remove Word";
            this.btnBannedWordsRem.UseVisualStyleBackColor = true;
            this.btnBannedWordsRem.Click += new System.EventHandler(this.btnBannedWordsRem_Click);
            // 
            // btnBannedWordsAdd
            // 
            this.btnBannedWordsAdd.Location = new System.Drawing.Point(48, 378);
            this.btnBannedWordsAdd.Name = "btnBannedWordsAdd";
            this.btnBannedWordsAdd.Size = new System.Drawing.Size(75, 23);
            this.btnBannedWordsAdd.TabIndex = 45;
            this.btnBannedWordsAdd.Text = "Add Word";
            this.btnBannedWordsAdd.UseVisualStyleBackColor = true;
            this.btnBannedWordsAdd.Click += new System.EventHandler(this.btnBannedWordsAdd_Click);
            // 
            // lvBannedWords
            // 
            this.lvBannedWords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Value,
            this.Type});
            this.lvBannedWords.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBannedWords.Location = new System.Drawing.Point(29, 264);
            this.lvBannedWords.Name = "lvBannedWords";
            this.lvBannedWords.ShowItemToolTips = true;
            this.lvBannedWords.Size = new System.Drawing.Size(235, 109);
            this.lvBannedWords.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvBannedWords.TabIndex = 44;
            this.lvBannedWords.UseCompatibleStateImageBehavior = false;
            this.lvBannedWords.View = System.Windows.Forms.View.Details;
            // 
            // Value
            // 
            this.Value.Tag = "Name";
            this.Value.Text = "Value";
            this.Value.Width = 152;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 84;
            // 
            // label24
            // 
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label24.Location = new System.Drawing.Point(19, 259);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(130, 2);
            this.label24.TabIndex = 43;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label23.Location = new System.Drawing.Point(22, 54);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(140, 2);
            this.label23.TabIndex = 42;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label22.Location = new System.Drawing.Point(310, 55);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(140, 2);
            this.label22.TabIndex = 41;
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label21.Location = new System.Drawing.Point(22, 122);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(140, 2);
            this.label21.TabIndex = 40;
            // 
            // label20
            // 
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label20.Location = new System.Drawing.Point(311, 131);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 2);
            this.label20.TabIndex = 39;
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Location = new System.Drawing.Point(22, 208);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(140, 2);
            this.label18.TabIndex = 37;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(377, 76);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 13);
            this.label17.TabIndex = 36;
            this.label17.Text = "Seconds.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(316, 76);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 13);
            this.label16.TabIndex = 35;
            this.label16.Text = "In";
            // 
            // txtSpamModerationTimeCount
            // 
            this.txtSpamModerationTimeCount.Location = new System.Drawing.Point(334, 73);
            this.txtSpamModerationTimeCount.Name = "txtSpamModerationTimeCount";
            this.txtSpamModerationTimeCount.Size = new System.Drawing.Size(37, 20);
            this.txtSpamModerationTimeCount.TabIndex = 34;
            this.txtSpamModerationTimeCount.TextChanged += new System.EventHandler(this.txtSpamModerationTimeCount_TextChanged);
            // 
            // chkBannedWordModeration
            // 
            this.chkBannedWordModeration.AutoSize = true;
            this.chkBannedWordModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBannedWordModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBannedWordModeration.Location = new System.Drawing.Point(16, 246);
            this.chkBannedWordModeration.Name = "chkBannedWordModeration";
            this.chkBannedWordModeration.Size = new System.Drawing.Size(180, 17);
            this.chkBannedWordModeration.TabIndex = 33;
            this.chkBannedWordModeration.Text = "Banned Words Moderation:";
            this.chkBannedWordModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBannedWordModeration.UseVisualStyleBackColor = true;
            this.chkBannedWordModeration.CheckedChanged += new System.EventHandler(this.chkBannedWordModeration_CheckedChanged);
            // 
            // chkColorModeration
            // 
            this.chkColorModeration.AutoSize = true;
            this.chkColorModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkColorModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkColorModeration.Location = new System.Drawing.Point(306, 175);
            this.chkColorModeration.Name = "chkColorModeration";
            this.chkColorModeration.Size = new System.Drawing.Size(126, 17);
            this.chkColorModeration.TabIndex = 30;
            this.chkColorModeration.Text = "Color Moderation:";
            this.chkColorModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkColorModeration.UseVisualStyleBackColor = true;
            this.chkColorModeration.CheckedChanged += new System.EventHandler(this.chkColorModeration_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(26, 213);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Max Message Length:";
            // 
            // txtMsgLengthModerationMaxLength
            // 
            this.txtMsgLengthModerationMaxLength.Location = new System.Drawing.Point(138, 210);
            this.txtMsgLengthModerationMaxLength.Name = "txtMsgLengthModerationMaxLength";
            this.txtMsgLengthModerationMaxLength.Size = new System.Drawing.Size(36, 20);
            this.txtMsgLengthModerationMaxLength.TabIndex = 28;
            this.txtMsgLengthModerationMaxLength.TextChanged += new System.EventHandler(this.txtMsgLengthModerationMaxLength_TextChanged);
            // 
            // chkMsgLengthModeration
            // 
            this.chkMsgLengthModeration.AutoSize = true;
            this.chkMsgLengthModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMsgLengthModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMsgLengthModeration.Location = new System.Drawing.Point(16, 195);
            this.chkMsgLengthModeration.Name = "chkMsgLengthModeration";
            this.chkMsgLengthModeration.Size = new System.Drawing.Size(190, 17);
            this.chkMsgLengthModeration.TabIndex = 27;
            this.chkMsgLengthModeration.Text = "Message Length Moderation:";
            this.chkMsgLengthModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMsgLengthModeration.UseVisualStyleBackColor = true;
            this.chkMsgLengthModeration.CheckedChanged += new System.EventHandler(this.chkMsgLengthModeration_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(316, 153);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(201, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Shortest Message Length to Caps Check";
            // 
            // txtEmotesModerationMinLength
            // 
            this.txtEmotesModerationMinLength.Location = new System.Drawing.Point(517, 150);
            this.txtEmotesModerationMinLength.Name = "txtEmotesModerationMinLength";
            this.txtEmotesModerationMinLength.Size = new System.Drawing.Size(37, 20);
            this.txtEmotesModerationMinLength.TabIndex = 25;
            this.txtEmotesModerationMinLength.TextChanged += new System.EventHandler(this.txtEmotesModerationMinLength_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(499, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "%";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(316, 135);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Percent Capitals in Message:";
            // 
            // txtEmotesModerationPercentage
            // 
            this.txtEmotesModerationPercentage.Location = new System.Drawing.Point(460, 132);
            this.txtEmotesModerationPercentage.Name = "txtEmotesModerationPercentage";
            this.txtEmotesModerationPercentage.Size = new System.Drawing.Size(37, 20);
            this.txtEmotesModerationPercentage.TabIndex = 22;
            this.txtEmotesModerationPercentage.TextChanged += new System.EventHandler(this.txtEmotesModerationPercentage_TextChanged);
            // 
            // chkEmotesModeration
            // 
            this.chkEmotesModeration.AutoSize = true;
            this.chkEmotesModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEmotesModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEmotesModeration.Location = new System.Drawing.Point(306, 117);
            this.chkEmotesModeration.Name = "chkEmotesModeration";
            this.chkEmotesModeration.Size = new System.Drawing.Size(138, 17);
            this.chkEmotesModeration.TabIndex = 21;
            this.chkEmotesModeration.Text = "Emotes Moderation:";
            this.chkEmotesModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEmotesModeration.UseVisualStyleBackColor = true;
            this.chkEmotesModeration.CheckedChanged += new System.EventHandler(this.chkEmotesModeration_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 164);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Max Number of Symbols Together:";
            // 
            // txtSymbolsModerationMaxGroupItems
            // 
            this.txtSymbolsModerationMaxGroupItems.Location = new System.Drawing.Point(197, 161);
            this.txtSymbolsModerationMaxGroupItems.Name = "txtSymbolsModerationMaxGroupItems";
            this.txtSymbolsModerationMaxGroupItems.Size = new System.Drawing.Size(37, 20);
            this.txtSymbolsModerationMaxGroupItems.TabIndex = 19;
            this.txtSymbolsModerationMaxGroupItems.TextChanged += new System.EventHandler(this.txtSymbolsModerationMaxGroupItems_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(214, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Shortest Message Length to Symbol Check:";
            // 
            // txtSymbolsModerationMinLength
            // 
            this.txtSymbolsModerationMinLength.Location = new System.Drawing.Point(240, 142);
            this.txtSymbolsModerationMinLength.Name = "txtSymbolsModerationMinLength";
            this.txtSymbolsModerationMinLength.Size = new System.Drawing.Size(37, 20);
            this.txtSymbolsModerationMinLength.TabIndex = 17;
            this.txtSymbolsModerationMinLength.TextChanged += new System.EventHandler(this.txtSymbolsModerationMinLength_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(211, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Percent Symbols in Message:";
            // 
            // txtSymbolsModerationPercentage
            // 
            this.txtSymbolsModerationPercentage.Location = new System.Drawing.Point(172, 124);
            this.txtSymbolsModerationPercentage.Name = "txtSymbolsModerationPercentage";
            this.txtSymbolsModerationPercentage.Size = new System.Drawing.Size(37, 20);
            this.txtSymbolsModerationPercentage.TabIndex = 14;
            this.txtSymbolsModerationPercentage.TextChanged += new System.EventHandler(this.txtSymbolsModerationPercentage_TextChanged);
            // 
            // chkSymbolsModeration
            // 
            this.chkSymbolsModeration.AutoSize = true;
            this.chkSymbolsModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSymbolsModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSymbolsModeration.Location = new System.Drawing.Point(16, 109);
            this.chkSymbolsModeration.Name = "chkSymbolsModeration";
            this.chkSymbolsModeration.Size = new System.Drawing.Size(217, 17);
            this.chkSymbolsModeration.TabIndex = 13;
            this.chkSymbolsModeration.Text = "Symbols/Punctuation Moderation:";
            this.chkSymbolsModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSymbolsModeration.UseVisualStyleBackColor = true;
            this.chkSymbolsModeration.CheckedChanged += new System.EventHandler(this.chkSymbolsModeration_CheckedChanged);
            // 
            // chkSpamModerationAllUsers
            // 
            this.chkSpamModerationAllUsers.AutoSize = true;
            this.chkSpamModerationAllUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpamModerationAllUsers.Location = new System.Drawing.Point(316, 92);
            this.chkSpamModerationAllUsers.Name = "chkSpamModerationAllUsers";
            this.chkSpamModerationAllUsers.Size = new System.Drawing.Size(108, 17);
            this.chkSpamModerationAllUsers.TabIndex = 12;
            this.chkSpamModerationAllUsers.Text = "Across All Users?";
            this.chkSpamModerationAllUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpamModerationAllUsers.UseVisualStyleBackColor = true;
            this.chkSpamModerationAllUsers.CheckedChanged += new System.EventHandler(this.chkSpamModerationAllUsers_CheckedChanged);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(3, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(556, 2);
            this.label6.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(165, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Moderation Preset:";
            // 
            // ddModerationPreset
            // 
            this.ddModerationPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddModerationPreset.FormattingEnabled = true;
            this.ddModerationPreset.Items.AddRange(new object[] {
            "Off",
            "Low",
            "Medium",
            "High",
            "Custom"});
            this.ddModerationPreset.Location = new System.Drawing.Point(267, 9);
            this.ddModerationPreset.Name = "ddModerationPreset";
            this.ddModerationPreset.Size = new System.Drawing.Size(121, 21);
            this.ddModerationPreset.TabIndex = 9;
            this.ddModerationPreset.SelectedIndexChanged += new System.EventHandler(this.ddModerationPreset_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Shortest Message Length to Caps Check";
            // 
            // txtCapsModerationMinLength
            // 
            this.txtCapsModerationMinLength.Location = new System.Drawing.Point(227, 73);
            this.txtCapsModerationMinLength.Name = "txtCapsModerationMinLength";
            this.txtCapsModerationMinLength.Size = new System.Drawing.Size(37, 20);
            this.txtCapsModerationMinLength.TabIndex = 7;
            this.txtCapsModerationMinLength.TextChanged += new System.EventHandler(this.txtCapsModerationMinLength_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(316, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Number of Repeated Messages:";
            // 
            // txtSpamModerationMessageCount
            // 
            this.txtSpamModerationMessageCount.Location = new System.Drawing.Point(477, 55);
            this.txtSpamModerationMessageCount.Name = "txtSpamModerationMessageCount";
            this.txtSpamModerationMessageCount.Size = new System.Drawing.Size(37, 20);
            this.txtSpamModerationMessageCount.TabIndex = 5;
            // 
            // chkSpamModeration
            // 
            this.chkSpamModeration.AutoSize = true;
            this.chkSpamModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpamModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSpamModeration.Location = new System.Drawing.Point(306, 40);
            this.chkSpamModeration.Name = "chkSpamModeration";
            this.chkSpamModeration.Size = new System.Drawing.Size(128, 17);
            this.chkSpamModeration.TabIndex = 4;
            this.chkSpamModeration.Text = "Spam Moderation:";
            this.chkSpamModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSpamModeration.UseVisualStyleBackColor = true;
            this.chkSpamModeration.CheckedChanged += new System.EventHandler(this.chkSpamModeration_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Percent Capitals in Message:";
            // 
            // txtCapsModerationPercentage
            // 
            this.txtCapsModerationPercentage.Location = new System.Drawing.Point(170, 55);
            this.txtCapsModerationPercentage.Name = "txtCapsModerationPercentage";
            this.txtCapsModerationPercentage.Size = new System.Drawing.Size(37, 20);
            this.txtCapsModerationPercentage.TabIndex = 1;
            this.txtCapsModerationPercentage.TextChanged += new System.EventHandler(this.txtCapsModerationPercentage_TextChanged);
            // 
            // chkCapsModeration
            // 
            this.chkCapsModeration.AutoSize = true;
            this.chkCapsModeration.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCapsModeration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCapsModeration.Location = new System.Drawing.Point(16, 40);
            this.chkCapsModeration.Name = "chkCapsModeration";
            this.chkCapsModeration.Size = new System.Drawing.Size(125, 17);
            this.chkCapsModeration.TabIndex = 0;
            this.chkCapsModeration.Text = "Caps Moderation:";
            this.chkCapsModeration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCapsModeration.UseVisualStyleBackColor = true;
            this.chkCapsModeration.CheckedChanged += new System.EventHandler(this.chkCapsModeration_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(492, 496);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Disconnect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 496);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 528);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "RebootTechBot";
            this.tabControl1.ResumeLayout(false);
            this.ChatPage.ResumeLayout(false);
            this.ChatPage.PerformLayout();
            this.LogPage.ResumeLayout(false);
            this.LogPage.PerformLayout();
            this.ModerationPage.ResumeLayout(false);
            this.ModerationPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage LogPage;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TabPage ChatPage;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView lstChatUsers;
        private System.Windows.Forms.WebBrowser ChatWindow;
        private System.Windows.Forms.TextBox txtChatMessage;
        private System.Windows.Forms.Button btnChat;
        private System.Windows.Forms.ColumnHeader UserName;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.Button btnCensorTimeout;
        private System.Windows.Forms.Button btnTimeout;
        private System.Windows.Forms.TabPage ModerationPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddModerationPreset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCapsModerationMinLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpamModerationMessageCount;
        private System.Windows.Forms.CheckBox chkSpamModeration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCapsModerationPercentage;
        private System.Windows.Forms.CheckBox chkCapsModeration;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtSpamModerationTimeCount;
        private System.Windows.Forms.CheckBox chkBannedWordModeration;
        private System.Windows.Forms.CheckBox chkColorModeration;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtMsgLengthModerationMaxLength;
        private System.Windows.Forms.CheckBox chkMsgLengthModeration;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtEmotesModerationMinLength;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtEmotesModerationPercentage;
        private System.Windows.Forms.CheckBox chkEmotesModeration;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSymbolsModerationMaxGroupItems;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSymbolsModerationMinLength;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSymbolsModerationPercentage;
        private System.Windows.Forms.CheckBox chkSymbolsModeration;
        private System.Windows.Forms.CheckBox chkSpamModerationAllUsers;
        private System.Windows.Forms.Button btnBannedWordsRem;
        private System.Windows.Forms.Button btnBannedWordsAdd;
        private System.Windows.Forms.ListView lvBannedWords;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.CheckBox chkModExceptionModerators;
        private System.Windows.Forms.CheckBox chkModExceptionSubscribers;
        private System.Windows.Forms.CheckBox chkModExceptionVIP;
        private System.Windows.Forms.CheckBox chkModExceptionRegular;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtModExceptionViewerHours;
        private System.Windows.Forms.CheckBox chkModExceptionFollowerForHours;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtModExceptionViewerSeconds;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkModExceptionViewerInChatDuration;
        private System.Windows.Forms.CheckBox chkExceptionModeration;
        private System.Windows.Forms.Button btnCancelModerationChanges;
        private System.Windows.Forms.Button btnApplyModerationChanges;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox ddModerationAction1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ComboBox ddModerationAction3;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ComboBox ddModerationAction2;
    }
}

