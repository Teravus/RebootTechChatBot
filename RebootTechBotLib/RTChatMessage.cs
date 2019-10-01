using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace RebootTechBotLib
{
    public class RTChatMessage : TwitchMessage
    {

        public string Channel { get; set; } = string.Empty;

        public bool IsMe { get; set; } = false;

        public string ChannelId { get; set; } = string.Empty;
        public bool IsHighlight { get; set; } = false;
        

        public RTChatMessage() : base()
        {

        }

        public RTChatMessage(ChatMessage msg) : base()
        {
            this.Message = msg.Message;
            this.IsTurbo = msg.IsTurbo;
            this.Badges = msg.Badges;
            this.DisplayName = msg.DisplayName;
            this.MessageIdStr = msg.Id;
            this.ChannelId = msg.RoomId;
            this.TwitchBotName = msg.BotUsername;
            this.UserIdStr = msg.UserId;
            this.UserName = msg.Username;
            this.UserType = (Enums.UserType)((int)msg.UserType);
            this.IsMe = msg.IsMe;
            this.Channel = msg.Channel;


        }
    }
}
