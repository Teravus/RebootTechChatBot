using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaderSharp;

namespace RebootTechBotLib
{
    public class TwitchMessage
    {
        private static SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();
        /// <summary>
        /// The messaged that the user typed
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// The user's twitch username
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// The properly capitalized username for the twitch user
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Hopefully, this contains a Unique ID for this user regardless of UserName.
        /// </summary>
        public string UserIdStr { get; set; } = string.Empty;

        /// <summary>
        /// The type of viewer in chat.   Viewer, Moderator, Broadcaster, Global Moderator, Admin
        /// </summary>
        public Enums.UserType UserType { get; set; } = Enums.UserType.Viewer;


        /// <summary>
        /// In cases where there are multiple bots running, this will contain the name of the bot that got the message.
        /// </summary>
        public string TwitchBotName { get; set; } = string.Empty;

        /// <summary>
        /// Hopefully...   a unique ID for the message so that we don't process the message more than once.
        /// </summary>
        public string MessageIdStr { get; set; } = string.Empty;


        /// <summary>
        /// Contains if this user is a turbo user.   But why do we care?    Are we going to do something special with this information.
        /// </summary>
        public bool IsTurbo { get; set; } = false;

        public double Sentiment { get; internal set; }

        /// <summary>
        /// Contain the icons next to the user's name.   This will be probably a name, and a URL   or a URL and a Description.    
        /// </summary>
        public List<KeyValuePair<string, string>> Badges { get; set; }

        public void CalulateSentiment()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                var results = analyzer.PolarityScores(Message);
                this.Sentiment = results.Compound;
            }
        }
    }
}
