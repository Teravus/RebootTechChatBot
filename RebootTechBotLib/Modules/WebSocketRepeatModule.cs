using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib.Command;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;

namespace RebootTechBotLib.Modules
{
    public class WebSocketRepeatModule : IChatModule
    {
        public string Name => "WebSocket Repeat Module";
        private BotOutput m_Output = BotOutput.Instance;
        public bool IsShared => true;
        private WebSocketHttpServerHandler m_Handler = null;
        private WebSocketHttpServerHandler activeHandler = null;


        public void ChannelJoined(TwitchChannel channel)
        {
            //
            channel.OnChannelFollow += Channel_OnChannelFollow;
        }

        private void Channel_OnChannelFollow(object sender, FollowEventArgs args)
        {
            
        }

        public void ChannelParted(TwitchChannel channel)
        {
            channel.OnChannelFollow -= Channel_OnChannelFollow;
        }

        public void Initialize(BotConfig config)
        {
            MainServer.Instance.AddWebSocketHandler("/echo", WebSocketHandler);
            CommandServer.AddCommand(string.Empty, CommandPermissions.Unknown, "WebSocketRepeatModule", true, "!say", "!say [some text]", "Prints some text in the bubbles on the overlay", ShowBubbleText);
           
        }
        public void Started()
        {

        }
        private void ShowBubbleText(string module, string[] cmd)
        {
            StringBuilder sb = new StringBuilder();
            if (cmd.Length <= 1)
                return;

            for (int i=1;i <cmd.Length;i++)
            {
                sb.Append(cmd[i]);
                sb.Append(" ");
            }

            m_Handler.SendMessage(sb.ToString().Trim());
        }
        public void WebSocketHandler(string method, WebSocketHttpServerHandler handler)
        {
            m_Handler = handler;
            m_Handler.OnClose += Handler_OnClose;
            m_Handler.OnText += Handler_OnText;
            m_Handler.OnUpgradeCompleted += Handler_OnUpgradeCompleted;
            m_Handler.OnData += Handler_OnData;
            m_Handler.OnPing += Handler_OnPing;
            m_Handler.OnPong += Handler_OnPong;
            m_Handler.SetChunksize(8192);
            m_Handler.NoDelay_TCP_Nagle = true;
            m_Handler.HandshakeAndUpgrade();
            
        }

        private void Handler_OnPong(object sender, PongEventArgs pongdata)
        {
            //throw new NotImplementedException();
        }

        private void Handler_OnPing(object sender, PingEventArgs pingdata)
        {
            //m_Handler.P//throw new NotImplementedException();
        }

        private void Handler_OnData(object sender, WebsocketDataEventArgs data)
        {
            byte[] databuffer = data.Data;
            m_Output.LogMessage("Info", "[WEBSOCKET]: We got some weird data");

        }

        private void Handler_OnUpgradeCompleted(object sender, UpgradeCompletedEventArgs completeddata)
        {
            activeHandler = sender as WebSocketHttpServerHandler;

        }

        private void Handler_OnText(object sender, WebsocketTextEventArgs text)
        {
            m_Output.LogMessage("Info", "[WEBSOCKET]: We got some text data");
            var item = sender as WebSocketHttpServerHandler;
            item.SendMessage(text.Data);
        }

        private void Handler_OnClose(object sender, CloseEventArgs closedata)
        {
            var item = sender as WebSocketHttpServerHandler;
            if (item != null)
            {
                item.Dispose();
            }
        }

        public void Shutdown()
        {
            m_Handler.OnClose -= Handler_OnClose;
            m_Handler.OnText -= Handler_OnText;
            m_Handler.OnUpgradeCompleted -= Handler_OnUpgradeCompleted;
            m_Handler.OnData -= Handler_OnData;
            m_Handler.OnPing -= Handler_OnPing;
            m_Handler.OnPong -= Handler_OnPong;
            if (m_Handler != null)
            {
                m_Handler.Dispose();
                m_Handler = null;
            }
            if (activeHandler != null)
            {
                activeHandler.Dispose();
                m_Handler = null;
            }

        }
    }
}
