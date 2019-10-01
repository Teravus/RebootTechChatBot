using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public interface IChatModule
    {
        string Name
        {
            get;
        }

        bool IsShared
        {
            get;
        }

        void Initialize(Config.BotConfig config);

        void ChannelJoined(TwitchChannel channel);

        void ChannelParted(TwitchChannel channel);

        void Shutdown();
        void Started();
    }
}
