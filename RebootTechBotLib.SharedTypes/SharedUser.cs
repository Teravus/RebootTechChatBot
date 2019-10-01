using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.SharedTypes
{
    public class SharedUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int UserType { get; set; }
        public int IsTurbo { get; set; }
        public DateTime? FirstTimeSeen { get; set; }
        public DateTime? LastSeen { get; set; }
        public int ChatTime { get; set; }
        public string ReferringStreamer { get; set; }
        public int TotalTimesSeen { get; set; }
        public int TotalChatMessages { get; set; }
        public int TotalWhisperMessages { get; set; }
        public int UserScore { get; set; }
        public int ProcessStatus { get; set; }
    }
}
