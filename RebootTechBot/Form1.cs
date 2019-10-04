using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using RebootTechBotLib;
using RebootTechBot.models;

namespace RebootTechBot
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(string text);
        private delegate void ChatMessage(RTChatMessage message);
        private delegate void UserJoinedChat(string username);
        private delegate void UserLeftChat(string username);
        private int chattextline = 0;
        private int logtextline = 0;

        private ModerationModel lastModerationModel = null;

        Bot rbtechbot = new Bot();
        public Form1()
        {
            InitializeComponent();

            ChatWindow.Navigate("about:blank");
            HtmlDocument doc = ChatWindow.Document;
            //txtLog.Text += (doc.ToString());
            doc.Write("<html><head><title>W</title><style>div {font-size:10px;}</style></head><body><div>Welcome to the chat room!</div></body></html>");
        }

        private static readonly string[] Colors =
        {
            //"#000000",
            "#FF0000",
            "#FF00EF",
            "#9A00FF",
            "#5E00FF",
            "#00B3FF",
            "#00FF5E",
            "#FF7700"
        };

       

        private void WriteTextSafe(string text)
        {
            
            if (txtLog.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                Invoke(d, new object[] { text + System.Environment.NewLine });
            }
            else
            {
                int maxtext = 300;
                int choptext = 150;
                logtextline++;
                if (logtextline > maxtext)
                {
                    txtLog.Text = txtLog.Text.Substring(choptext, maxtext - choptext);
                    logtextline = choptext;
                }
                txtLog.Text += text + System.Environment.NewLine;
            }
        }
        
        private void UserJoinedChatSafe (string username)
        {
            var listviewkey = string.Format("cht_{0}", username);
            if (lstChatUsers.InvokeRequired)
            {
                var d = new UserJoinedChat(UserJoinedChatSafe);
                Invoke(d, new object[] { username });
            }
            else
            {
                if (!lstChatUsers.Items.ContainsKey(listviewkey))
                {
                    string tooltip = rbtechbot.GetToolTipOnUser(username);
                    lstChatUsers.Items.Add(new ListViewItem() { Text = username, Name = string.Format("cht_{0}", username), Tag="Name", ToolTipText = tooltip });
                }
                WriteChatMessageSafe(new RTChatMessage()
                {
                    DisplayName = username,
                    Message = "(Joined Chat)",
                    MessageIdStr = String.Format("Join_{0}", username)
                }
                );
            }
        }

        private void UserLeftChatSafe (string username)
        {
            if (lstChatUsers.InvokeRequired)
            {
                var d = new UserLeftChat(UserLeftChatSafe);
                Invoke(d, new object[] { username });
            }
            else
            {
                string listviewitemkey = string.Format("cht_{0}", username);
                if (lstChatUsers.Items.ContainsKey(listviewitemkey))
                {
                    lstChatUsers.Items.RemoveByKey(listviewitemkey);
                }
                WriteChatMessageSafe(new RTChatMessage()
                {
                    DisplayName = username,
                    Message = "(Left Chat)",
                    MessageIdStr = String.Format("Join_{0}", username)
                }
                );
            }
        }

        private void WriteChatMessageSafe(RTChatMessage message)
        {
            if (ChatWindow.InvokeRequired)
            {
                var d = new ChatMessage(WriteChatMessageSafe);
                Invoke(d, new object[] { message });
            }
            else
            {
               
                UpdateChatBrowserWithMessage(message);
                string listviewitemkey = string.Format("cht_{0}", message.UserName);
                if (lstChatUsers.Items.ContainsKey(listviewitemkey))
                {

                    string tooltip = rbtechbot.GetToolTipOnUser(message.UserName);
                    lstChatUsers.Items[listviewitemkey].ToolTipText = tooltip;
                    // lstChatUsers.Items[listviewitemkey]. = tooltip;
                    //lstChatUsers.Items.Add(new ListViewItem() { Text = username, Name = string.Format("cht_{0}", username), Tag = "Name", ToolTipText = tooltip });
                }
            }
        }

        private void UpdateChatBrowserWithMessage(RTChatMessage message)
        {
            int maxtext = 300;
            
            chattextline++;
           
            HtmlDocument doc = ChatWindow.Document;
            var element = doc.GetElementsByTagName("body");
            var bodyelement = element[0];

            string chatdisplayname = HttpUtility.HtmlEncode(message.DisplayName);
            string chatmessage = HttpUtility.HtmlEncode(message.Message);
            string messageid = HttpUtility.HtmlEncode(message.MessageIdStr);

            string color = Colors[(Math.Abs(chatdisplayname.ToUpperInvariant().GetHashCode()) % Colors.Length)];
            if (chattextline > maxtext)
            {
                bodyelement.InnerHtml = "<div>Welcome to the chat room!</div>";
                chattextline = 0;
            }
            bodyelement.InnerHtml += string.Format("<div data-id=\"{3}\">[<span style=\"color: {2};\">{0}</span>]: {1}</div>", chatdisplayname, chatmessage, color, messageid);
            
            ChatWindow.Document.Window.ScrollTo(0, ChatWindow.Document.Body.ScrollRectangle.Height);

        }

        private void LogMessage(string message)
        {
            WriteTextSafe(message);
            
        }
      
        // TODO: Rename Button
        private void button1_Click(object sender, EventArgs e)
        {
            rbtechbot.LogMessage += LogMessage;
            rbtechbot.OnChatMessage += Bot_ChatMessage;
            rbtechbot.OnUserJoin += Bot_ChatUserJoin;
            rbtechbot.OnUserPart += Bot_ChatUserPart;
            rbtechbot.Start();
        }

        private void Bot_ChatUserPart(string username, TwitchUser user)
        {
            UserLeftChatSafe(username);
        }

        private void Bot_ChatUserJoin(string username, TwitchUser user)
        {
            UserJoinedChatSafe(username);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rbtechbot.Stop();
            rbtechbot.OnUserPart -= Bot_ChatUserPart;
            rbtechbot.OnUserJoin -= Bot_ChatUserJoin;
            rbtechbot.LogMessage -= LogMessage;
            rbtechbot.OnChatMessage -= Bot_ChatMessage;
        }

        private void Bot_ChatMessage(string channel, RTChatMessage chatMessage)
        {
            WriteChatMessageSafe(chatMessage);
        }
        private void btnChat_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChatMessage.Text) && !string.IsNullOrWhiteSpace(txtChatMessage.Text))
            {
                string chatmessage = txtChatMessage.Text.Trim();
                rbtechbot.SendTwitchChatMessage(null, chatmessage);
                txtChatMessage.Text = string.Empty;

            }
            txtChatMessage.Focus();
        }

        private void tmpButton_Click(object sender, EventArgs e)
        {
            var message = new RTChatMessage()
            {
                DisplayName = "RebootTechBot",
                Message = txtChatMessage.Text.Trim(),
                MessageIdStr = "390280982352"
            };

            WriteChatMessageSafe(message);
            txtChatMessage.Text = string.Empty;
        }

       
        private void btnCensorTimeout_Click(object sender, EventArgs e)
        {
            var selecteditems = lstChatUsers.SelectedItems;
            var selecteditem = selecteditems[0];
            string TimeoutUsername = selecteditem.Text;
            rbtechbot.Timeout(null,TimeoutUsername, 1);
        }

        private void btnTimeout_Click(object sender, EventArgs e)
        {
            var selecteditems = lstChatUsers.SelectedItems;
            var selecteditem = selecteditems[0];
            string TimeoutUsername = selecteditem.Text;
            rbtechbot.Timeout(null, TimeoutUsername, 86400);
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            var selecteditems = lstChatUsers.SelectedItems;
            var selecteditem = selecteditems[0];
            string TimeoutUsername = selecteditem.Text;
            rbtechbot.Ban(null, TimeoutUsername, "Banned By Streamer");
        }

        private void btnApplyModerationChanges_Click(object sender, EventArgs e)
        {
            DisableApplyButtons();
        }
        private void btnCancelModerationChanges_Click(object sender, EventArgs e)
        {
            DisableApplyButtons();
        }

        private void btnBannedWordsAdd_Click(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void btnBannedWordsRem_Click(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkCapsModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkSpamModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkSymbolsModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkEmotesModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkMsgLengthModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkColorModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkExceptionModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkBannedWordModeration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtCapsModerationPercentage_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtCapsModerationMinLength_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtSpamModerationTimeCount_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkSpamModerationAllUsers_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtSymbolsModerationPercentage_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtSymbolsModerationMinLength_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtSymbolsModerationMaxGroupItems_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtMsgLengthModerationMaxLength_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtEmotesModerationPercentage_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtEmotesModerationMinLength_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionViewerInChatDuration_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionFollowerForHours_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionRegular_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionVIP_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionSubscribers_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void chkModExceptionModerators_CheckedChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtModExceptionViewerSeconds_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void txtModExceptionViewerHours_TextChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void ddModerationAction1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void ddModerationAction2_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void ddModerationAction3_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }

        private void ddModerationPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableApplyButtons();
        }
        private void EnableApplyButtons()
        {
            if (ddModerationPreset.SelectedItem.ToString() != "Custom")
                ddModerationPreset.SelectedItem = "Custom";
            btnApplyModerationChanges.Enabled = true;
            btnCancelModerationChanges.Enabled = true;
        }
        private void DisableApplyButtons()
        {
            btnApplyModerationChanges.Enabled = false;
            btnCancelModerationChanges.Enabled = false;
        }
       
    }
}
