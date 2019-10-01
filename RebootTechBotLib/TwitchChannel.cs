using RebootTechBotLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace RebootTechBotLib
{
    public class TwitchChannel
    {
        public delegate void ChannelChatMessage(RTChatMessage chat);
        public delegate void Follow(object sender, FollowEventArgs args);

        public event ChannelChatMessage OnChannelChatMessage;
        public event Follow OnChannelFollow;

        public string Channel { get; set; } = string.Empty;

        public bool FollowerOnlyChat
        {
            get {
                return (FollowerOnlyChatTime.HasValue && FollowerOnlyChatTime.Value != null && FollowerOnlyChatTime.Value != TimeSpan.FromSeconds(0));
            }
        }

        public bool SlowModeChat
        {
            get
            {
                return (SlowModeSeconds.HasValue && SlowModeSeconds != null && SlowModeSeconds.Value > 0);
            }
        }

        public TimeSpan? FollowerOnlyChatTime { get; set; }

        public int? SlowModeSeconds { get; set; }

        public bool SubOnlyChat { get; set; } = false;

        public bool EmoteOnlyChat { get; set; } = false;

        public bool Murcury { get; set; } = false;

        public bool? R9K { get; set; } = false;
        public bool? Rituals { get; set; } = false;
        public string Language { get; set; } = string.Empty;
        public string Id { get; set; } = null;
        private ChatModuleManager m_ModuleManager = null;

        public TwitchChannel()
        {
        }

        public TwitchChannel(ChatModuleManager manager)
        {
            m_ModuleManager = manager;

        }

        public TwitchChannel( JoinedChannel chan, ChatModuleManager manager)
        {
            this.m_ModuleManager = manager;
            this.Channel = chan.Channel;
            var channelstate = chan.ChannelState;
            if (channelstate != null)
            {
                Language = channelstate.BroadcasterLanguage;

                if (channelstate.EmoteOnly != null && channelstate.EmoteOnly.GetValueOrDefault() == true)
                    this.EmoteOnlyChat = true;

                if (channelstate.SubOnly != null && channelstate.SubOnly.GetValueOrDefault() == true)
                    this.SubOnlyChat = true;

                this.Murcury = channelstate.Mercury;
                this.R9K = channelstate.R9K;
                this.Rituals = channelstate.Rituals;
                this.Id = chan.ChannelState.RoomId;
            }

        }
        public void IncomingChatMessage(RTChatMessage message)
        {
            ChannelChatMessage d = OnChannelChatMessage;
            if (d != null)
            {
                d(message);
            }
        }
        public void IncomingFollow(FollowEventArgs args)
        {
            var d = OnChannelFollow;
            if (d != null)
            {
                d(this, args);
            }
        }
        // Pass through methods to ChatModuleManager.  Better for these to be passthrough, 
        // than for modules to keep references to the module manager.

        public void RegisterModuleInterfaceHandler<T>(T type)
        {
            m_ModuleManager.RegisterModuleInterfaceHandler(type);
        }
        public T GetModuleInterfaceHandler<T>()
        {
            return m_ModuleManager.GetModuleInterfaceHandler<T>();
        }
    }
}
