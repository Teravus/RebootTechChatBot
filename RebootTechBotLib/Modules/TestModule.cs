using RebootTechBotLib;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Modules
{
    public class TestModule : IChatModule
    {
        BotOutput m_output;

        public TestModule()
        {
            m_output = BotOutput.Instance;
        }

        public string Name => "Test Module";

        public bool IsShared => false;

        public void ChannelJoined(TwitchChannel channel)
        {
            //m_output.ChatMessage(channel.Channel, string.Format("Hi denizens of my favorite channel {0}!", channel.Channel));
        }

        public void ChannelParted(TwitchChannel channel)
        {
            m_output.LogMessage("info", string.Format("[TESTMODULE]: Left Channel - {0} ", channel.Channel));
        }

        public void Initialize(BotConfig config)
        {
            m_output.LogMessage("info", string.Format("[TESTMODULE]: I was Initialized! My name is {0}", config.general.BotName));
        }
        public void Started()
        {

        }
        public void Shutdown()
        {
            m_output.LogMessage("info", "NOOOOOOOOOOOOOOOOoooooooooooooooooooooooooooooooooooooo............................... monkaS");
        }
    }
}
