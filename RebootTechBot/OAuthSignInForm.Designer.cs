namespace RebootTechBot
{
    partial class OAuthSignInForm
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
            this.scOAuthPanel = new System.Windows.Forms.SplitContainer();
            this.gbTwitchAppSetup = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.clbScopes = new System.Windows.Forms.CheckedListBox();
            this.btnStartOAuth = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbClientSecret = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbClientId = new System.Windows.Forms.TextBox();
            this.oAuthBrowser = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.scOAuthPanel)).BeginInit();
            this.scOAuthPanel.Panel1.SuspendLayout();
            this.scOAuthPanel.Panel2.SuspendLayout();
            this.scOAuthPanel.SuspendLayout();
            this.gbTwitchAppSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // scOAuthPanel
            // 
            this.scOAuthPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOAuthPanel.Location = new System.Drawing.Point(0, 0);
            this.scOAuthPanel.Name = "scOAuthPanel";
            this.scOAuthPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scOAuthPanel.Panel1
            // 
            this.scOAuthPanel.Panel1.Controls.Add(this.gbTwitchAppSetup);
            // 
            // scOAuthPanel.Panel2
            // 
            this.scOAuthPanel.Panel2.Controls.Add(this.oAuthBrowser);
            this.scOAuthPanel.Size = new System.Drawing.Size(1052, 649);
            this.scOAuthPanel.SplitterDistance = 143;
            this.scOAuthPanel.TabIndex = 0;
            // 
            // gbTwitchAppSetup
            // 
            this.gbTwitchAppSetup.Controls.Add(this.label3);
            this.gbTwitchAppSetup.Controls.Add(this.clbScopes);
            this.gbTwitchAppSetup.Controls.Add(this.btnStartOAuth);
            this.gbTwitchAppSetup.Controls.Add(this.label2);
            this.gbTwitchAppSetup.Controls.Add(this.tbClientSecret);
            this.gbTwitchAppSetup.Controls.Add(this.label1);
            this.gbTwitchAppSetup.Controls.Add(this.tbClientId);
            this.gbTwitchAppSetup.Location = new System.Drawing.Point(177, 12);
            this.gbTwitchAppSetup.Name = "gbTwitchAppSetup";
            this.gbTwitchAppSetup.Size = new System.Drawing.Size(630, 111);
            this.gbTwitchAppSetup.TabIndex = 0;
            this.gbTwitchAppSetup.TabStop = false;
            this.gbTwitchAppSetup.Text = "Twitch Application Setup";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Scopes";
            // 
            // clbScopes
            // 
            this.clbScopes.FormattingEnabled = true;
            this.clbScopes.HorizontalScrollbar = true;
            this.clbScopes.Items.AddRange(new object[] {
            "analytics:read:extensions",
            "analytics:read:games",
            "bits:read",
            "channel:read:subscriptions",
            "clips:edit",
            "user:edit",
            "user:edit:broadcast",
            "user:read:broadcast",
            "user:read:email",
            "channel_check_subscription",
            "channel_commercial",
            "channel_editor",
            "channel_feed_edit",
            "channel_feed_read",
            "channel_read",
            "channel_stream",
            "channel_subscriptions",
            "collections_edit",
            "communities_edit",
            "communities_moderate",
            "openid",
            "user_blocks_edit",
            "user_blocks_read",
            "user_follows_edit",
            "user_read",
            "user_subscriptions",
            "viewing_activity_read",
            "channel:moderate",
            "chat:edit",
            "chat:read",
            "whispers:read",
            "whispers:edit"});
            this.clbScopes.Location = new System.Drawing.Point(366, 19);
            this.clbScopes.Name = "clbScopes";
            this.clbScopes.Size = new System.Drawing.Size(241, 79);
            this.clbScopes.TabIndex = 5;
            this.clbScopes.ThreeDCheckBoxes = true;
            // 
            // btnStartOAuth
            // 
            this.btnStartOAuth.Location = new System.Drawing.Point(135, 71);
            this.btnStartOAuth.Name = "btnStartOAuth";
            this.btnStartOAuth.Size = new System.Drawing.Size(109, 23);
            this.btnStartOAuth.TabIndex = 4;
            this.btnStartOAuth.Text = "Initiate OAuth";
            this.btnStartOAuth.UseVisualStyleBackColor = true;
            this.btnStartOAuth.Click += new System.EventHandler(this.btnStartOAuth_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Client Secret";
            // 
            // tbClientSecret
            // 
            this.tbClientSecret.Location = new System.Drawing.Point(83, 45);
            this.tbClientSecret.Name = "tbClientSecret";
            this.tbClientSecret.PasswordChar = '*';
            this.tbClientSecret.Size = new System.Drawing.Size(215, 20);
            this.tbClientSecret.TabIndex = 2;
            this.tbClientSecret.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ClientID";
            // 
            // tbClientId
            // 
            this.tbClientId.Location = new System.Drawing.Point(83, 19);
            this.tbClientId.Name = "tbClientId";
            this.tbClientId.PasswordChar = '*';
            this.tbClientId.Size = new System.Drawing.Size(215, 20);
            this.tbClientId.TabIndex = 0;
            this.tbClientId.UseSystemPasswordChar = true;
            // 
            // oAuthBrowser
            // 
            this.oAuthBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oAuthBrowser.Location = new System.Drawing.Point(0, 0);
            this.oAuthBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.oAuthBrowser.Name = "oAuthBrowser";
            this.oAuthBrowser.Size = new System.Drawing.Size(1052, 502);
            this.oAuthBrowser.TabIndex = 3;
            // 
            // OAuthSignInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 649);
            this.Controls.Add(this.scOAuthPanel);
            this.Name = "OAuthSignInForm";
            this.Text = "OAuthSignInForm";
            this.Load += new System.EventHandler(this.OAuthSignInForm_Load);
            this.scOAuthPanel.Panel1.ResumeLayout(false);
            this.scOAuthPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOAuthPanel)).EndInit();
            this.scOAuthPanel.ResumeLayout(false);
            this.gbTwitchAppSetup.ResumeLayout(false);
            this.gbTwitchAppSetup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scOAuthPanel;
        private System.Windows.Forms.GroupBox gbTwitchAppSetup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox clbScopes;
        private System.Windows.Forms.Button btnStartOAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbClientSecret;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClientId;
        private System.Windows.Forms.WebBrowser oAuthBrowser;
    }
}