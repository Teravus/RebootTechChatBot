using System;

namespace RebootTechBotLib.SharedTypes
{
    public class SharedUserProcessHistoryEntry
    {
        public int ProcessHistoryId { get; set; }
        public int ProcessId { get; set; }
        public int ProcessStatusId { get; set; }
        public DateTime CompletedDate { get; set; }
        public int ChatTime { get; set; }
        public int TotalChats { get; set; }
        public int TotalWhispers { get; set; }
        public int TotalTimesSeen { get; set; }
        public string Category { get; set; }
        public string StreamTitle { get; set; }
        public int UserScore { get; set; }
    }
}
