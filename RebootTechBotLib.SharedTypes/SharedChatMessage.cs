using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.SharedTypes
{
    public class SharedChatMessage
    {
        public int MId { get;set; }

        //MessageId INTEGER Primary Key AutoIncrement,
        public string MessageId { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public bool IsTurbo { get; set; }
        public string ChannelName { get; set; }
        public string Message { get; set; }
        public double Sentiment { get; set; }

       
    }
}
