using Newtonsoft.Json;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using RebootTechBotLib.ModuleInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Modules
{
    public class SentimentIconModule : IChatModule
    {
        public string Name => "SentimentIconModule";

        public bool IsShared => false;

        private IBrowserOverlay overlayModule;

        private TwitchChannel thischannel; 

        public void ChannelJoined(TwitchChannel channel)
        {
            thischannel = channel;
            thischannel.OnChannelChatMessage += Thischannel_OnChannelChatMessage;
        }

        private void Thischannel_OnChannelChatMessage(RTChatMessage chat)
        {
            chat.CalulateSentiment();
            // chat.Sentiment
            var message = new SentimentMessage()
            {
                sentimentvalue = (float)chat.Sentiment
            };

            if (overlayModule != null)
                overlayModule.SendModuleMessage("sentimenticon", "currentsentiment", JsonConvert.SerializeObject(message));
        }

        public void ChannelParted(TwitchChannel channel)
        {
            thischannel.OnChannelChatMessage -= Thischannel_OnChannelChatMessage;
        }

        public void Initialize(BotConfig config)
        {
            
        }

        public void Shutdown()
        {
            //thischannel.OnChannelChatMessage -= Thischannel_OnChannelChatMessage;
            thischannel = null;
            overlayModule = null;
        }

        public void Started()
        {
            overlayModule = thischannel.GetModuleInterfaceHandler<IBrowserOverlay>();
            if (overlayModule != null)
            {
                overlayModule.AddCSSModule("sentimenticon", "/css/sentimenticon/styles.css");
                overlayModule.AddJSModule("sentimenticon", "/js/sentimenticon/sentimenticon.js");
            }
            
        }
        public class SentimentMessage
        {
            public float sentimentvalue { get; set; }
        }
    }
}
