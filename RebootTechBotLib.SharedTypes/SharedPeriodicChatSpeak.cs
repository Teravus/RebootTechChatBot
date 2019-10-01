using System;

namespace RebootTechBotLib.SharedTypes
{
    public class SharedPeriodicChatSpeak
    {
        public int SpeakId { get; set; }
        public string ChannelName { get; set; }
        public string SpeakText { get; set; }
        public int CoolDownSeconds { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public int IsActive { get; set; }
    }
}
