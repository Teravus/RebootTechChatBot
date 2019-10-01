using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaderSharp;

namespace RebootTechBotLib
{
    public class TwitchUser
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<KeyValuePair<string,string>> Badges { get; set; }
        public Enums.UserType UserType { get; set; } = Enums.UserType.Viewer;
        public bool IsTurbo { get; set; } = false;
        public static SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

        // This should be the first time we encounter the user
        public DateTime FirstTimeSeen { get; set; }

        // This should be the last time we saw the user
        public DateTime LastSeen { get; set; }

        public uint TotalTimesSeen { get; set; } = 0;

        // This is the total amount of time in seconds that the user has hung around.
        public uint ChatTime { get; set; } = 0;

        public uint TotalChatMessages { get; set; } = 0;

        public uint TotalWhisperMessages { get; set; } = 0;


        // If we encounter a raid, then..  we first see this user...  we attribute their referrer as the raid streamer.
        public string ReferringStreamer = string.Empty;

        public List<DateTime> FollowDate { get; set; } = new List<DateTime>();

        public List<RTChatMessage> ChatMessages { get; set; } = new List<RTChatMessage>();

        public List<RTWhisperMessage> WhisperMessages { get; set; } = new List<RTWhisperMessage>();

        public bool IsHighlightedUser { get; set; } = false;

        public bool IsChanged { get; set; } = false;
        public double PositiveSentiment { get; set; } = 0;
        public double NegativeSentiment { get; set; } = 0;
        public double NeutralSentiment { get; set; } = 0;
        public double CompoundScore { get; set; } = 0;

        public DateTime? LastSaved { get; set; }

        public void LogChatMessage (RTChatMessage message)
        {
            ChatTime += (uint)(int)(DateTime.UtcNow - LastSeen).TotalSeconds;
            LastSeen = DateTime.UtcNow;
            ChatMessages.Add(message);
            TotalChatMessages += 1;
            
            UserId = message.UserIdStr;
            DisplayName = message.DisplayName;
            Badges = message.Badges;
            UserName = message.UserName;
            UserType = message.UserType;
            IsTurbo = message.IsTurbo;
            if (message.Message.ToLowerInvariant().Contains(message.TwitchBotName.ToLowerInvariant()))
                IsHighlightedUser = true;
            IsChanged = true;
            var sentimentresults = analyzer.PolarityScores(message.Message);

            PositiveSentiment += sentimentresults.Positive;
            NegativeSentiment += sentimentresults.Negative;
            NeutralSentiment += sentimentresults.Neutral;
            CompoundScore += sentimentresults.Compound;


        }
        public void LogFollow(DateTime followDateTime)
        {
            FollowDate.Add(followDateTime);
        }
        public void AddChatTimeSinceLastSeen()
        {
            ChatTime += (uint)(int)(DateTime.UtcNow - LastSeen).TotalSeconds;
            
        }

        
    }
}
