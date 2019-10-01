using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace RebootTechBotLib
{
    public class RTWhisperMessage : TwitchMessage
    {
        public RTWhisperMessage() : base()
        {

        }

        public RTWhisperMessage( WhisperMessage msg) : base()
        {
            this.Message = msg.Message;
            this.IsTurbo = msg.IsTurbo;
            this.Badges = msg.Badges;
            this.DisplayName = msg.DisplayName;
            this.MessageIdStr = msg.MessageId;
            this.ThreadIdStr = msg.ThreadId;
            this.TwitchBotName = msg.BotUsername;
            this.UserIdStr = msg.UserId;
            this.UserName = msg.Username;
            this.UserType = (Enums.UserType)((int)msg.UserType);
            
            
        }
       

       
       
        /// <summary>
        /// ???
        /// </summary>
        public string ThreadIdStr { get; set; } = string.Empty;

       

      

    }
}
