using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib
{
    public class FollowEventArgs : EventArgs
    {
        public TwitchUser Follower { get; set; }
        public TwitchUser Streamer { get; set; }
        public DateTime FollowDate { get; set; }
    }
}
