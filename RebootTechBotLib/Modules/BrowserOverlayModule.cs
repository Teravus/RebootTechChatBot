using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib.ModuleInterfaces;

namespace RebootTechBotLib.Modules
{
    public class BrowserOverlayModule : IChatModule, IBrowserOverlay
    {
        public string Name => "BrowserOverlayModule";

        public bool IsShared => true;
        // So the module is going to request the IBrowserOverlay Interface and then register for overlay messages from the websocket.
        // The m_Registered Handlers is supposed to create a way to limit the amount of traffic that goes to each individual module, however, 
        // the delegate isn't right for that.  There isn't really even a delegate..  Just a content object.  I'm not sure what I was thinking.
        // Anyway..   the event will be triggered regardless of the registration...  or not.
          
        private WebSocketHttpServerHandler m_Handler = null;
        private WebSocketHttpServerHandler activeHandler = null;

        //                         Module,  Method,   Message to Module
        private MultiKeyDictionary<string, string, BrowserOverlayMessage> m_RegisteredHandlers = new MultiKeyDictionary<string, string, BrowserOverlayMessage>();
        
        private Dictionary<string, string> m_ModuleJS = new Dictionary<string, string>();
        private Dictionary<string, string> m_ModuleCSS = new Dictionary<string, string>();
        public event BrowserOverlayConnected OnOverlayConnected;
        public event BrowserOverlayMessage OnOverlayMessage;

        public void ChannelJoined(TwitchChannel channel)
        {
            channel.RegisterModuleInterfaceHandler<IBrowserOverlay>(this);
            channel.OnChannelFollow += Channel_OnChannelFollow;
            channel.OnChannelChatMessage += Channel_OnChannelChatMessage;
        }

        private void Channel_OnChannelChatMessage(RTChatMessage chat)
        {
            string chatmessage = chat.Message;
            //MediaDefinition media = new MediaDefinition()
            //{
            //    align = "center",
            //    Left = 0,
            //    Top = 0,
            //    MediaHeight = "50",
            //    MediaWidth = "50",
            //    MediaType = "image",
            //    ObjectRemovalTimeout = 20000,
            //    position = "relative",
            //    Media = new MediaOptions[] {
            //        new MediaOptions() { AddressUri = "/img/Kappa.gif", ContentType = "image/gif"}
            //    }
            //};
            //string mediamsgStr = JsonConvert.SerializeObject(media);
            //OverLayMessage msg = new OverLayMessage()
            //{
            //    id = 1,
            //    module = "overlaymessage",
            //    method = "media",
            //    data = mediamsgStr
            //};
            //if (activeHandler != null)
            //    activeHandler.SendMessage(JsonConvert.SerializeObject(msg));
            //FollowMsg msg = new FollowMsg() { FollowMessage = string.Format("{0}", chatmessage) };
            //string data = JsonConvert.SerializeObject(msg);
            //OverLayMessage overlaymsg = new OverLayMessage() { module= "overlaymessage", method = "follow", data = data };
            //SendMessage(overlaymsg);
            //ComposeJSAdditionMessage("TestModule", "https://cdn.sstatic.net/Js/stub.en.js?v=1e65d3e474a2");
            //ComposeCssAdditionMessage("TestModule", "/css/nofile.css");
        }

        private void Channel_OnChannelFollow(object sender, FollowEventArgs args)
        {
            FollowMsg msg = new FollowMsg() { FollowMessage = string.Format("{0} Followed", args.Follower) };
            string data = JsonConvert.SerializeObject(msg);
            OverLayMessage overlaymsg = new OverLayMessage() { method = "follow", data = data };
            SendMessage(overlaymsg);
        }

        public void ChannelParted(TwitchChannel channel)
        {

        }
        private void SendMessage(OverLayMessage msg)
        {
            string sendmsg = JsonConvert.SerializeObject(msg);
            if (activeHandler != null)
            {
                activeHandler.SendMessage(sendmsg);
            }
        }
        public void Initialize(BotConfig config)
        {
            MainServer.Instance.AddWebSocketHandler("/BrowserOverlay", WebSocketHandler);
            //UnknownProcessor = CommandServer.Instance(CommandPermissions.Unknown);
            //UnknownProcessor.Commands.AddCommand("WebSocketRepeatModule", true, "!say", "!say [some text]", "Prints some text in the bubbles on the overlay", ShowBubbleText);
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
            //byte[] databuffer = data.Data;
            //BotOutput.Instance.LogMessage("Info", "[WEBSOCKET]: We got some weird data");

        }

        private void Handler_OnUpgradeCompleted(object sender, UpgradeCompletedEventArgs completeddata)
        {
            activeHandler = sender as WebSocketHttpServerHandler;
            var handler = OnOverlayConnected;
            if (handler != null)
                handler(this, activeHandler.GetRemoteIPEndpoint().ToString());

            foreach (var key in m_ModuleCSS.Keys)
            {
                ComposeCssAdditionMessage(key, m_ModuleCSS[key]);
            }
            foreach (var key in m_ModuleJS.Keys)
            {
                ComposeJSAdditionMessage(key, m_ModuleJS[key]);
            }

        }
        private void ComposeJSAdditionMessage (string module, string javascriptfile)
        {
            OverLayMessage overlaymsg = new OverLayMessage() {
                module = "overlaymessage",
                method = "newjavascript",
                //id = ?
                data = JsonConvert.SerializeObject(new newJavaScript() { JSUri = javascriptfile, module = module })
            };
            SendMessage(overlaymsg);
            
        }
        private void ComposeCssAdditionMessage(string module, string cssfile)
        {
            OverLayMessage overlaymsg = new OverLayMessage()
            {
                module = "overlaymessage",
                method = "newstylesheet",
                //id = ?
                data = JsonConvert.SerializeObject(new newCss() {  cssURI = cssfile, module = module })
            };
            SendMessage(overlaymsg);
            
        }
        private void Handler_OnText(object sender, WebsocketTextEventArgs text)
        {
            //BotOutput.Instance.LogMessage("Info", "[WEBSOCKET]: We got some text data");
            //var item = sender as WebSocketHttpServerHandler;
            //item.SendMessage(text.Data);
            OverLayMessage msg = null;
            if (TryParseJSON(text.Data, out msg))
            {
                if (m_RegisteredHandlers.ContainsKey(msg.module, msg.method))
                {
                    var item = m_RegisteredHandlers[msg.module, msg.method];
                    if (item != null)
                        item(msg.module, msg.method, msg.data);
                    // This is supposed to send a message from the overlay back to the module over the interface.
                }
                else
                {
                    // parse it ourselves for our own reasons?
                }
            }
        }

        private bool TryParseJSON(string data, out OverLayMessage msg)
        {
            msg = null;
            try
            {
                msg = JsonConvert.DeserializeObject<OverLayMessage>(data);
                return msg != null;
            }
            catch
            { }
            return false;
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

        public void Started()
        {
            
        }

        public void AddJSModule(string module, string js)
        {
            // this is supposed to add additional javascript calls from the overlay.
            if (m_ModuleJS.ContainsKey(module))
            {
                m_ModuleJS[module] = js;
            } 
            else
            {
                m_ModuleJS.Add(module, js);
            }
            

            ComposeJSAdditionMessage(module, js);
        }

        public void AddCSSModule(string module, string css)
        {
            // TODO: Fix.  Disconnecting and Reconnecting throws an exception because this isn't cleared.
            if (m_ModuleCSS.ContainsKey(module))
            {
                m_ModuleCSS[module] = css;
            }
            else
            {
                m_ModuleCSS.Add(module, css);
            }
            
            ComposeCssAdditionMessage(module, css);
        }

        public void SendModuleMessage(string module, string messageinfo, string message)
        {
            //  This is supposed to be from the module...  send a message TO the overlay
            if (activeHandler != null)
            {
                OverLayMessage msg = new OverLayMessage()
                {
                    id = 1,
                    module = module,
                    method = messageinfo,
                    data = message
                };
                SendMessage(msg);
            }
        }

        public void RegisterForMessage(string module, string messageinfo, BrowserOverlayMessage message)
        {
            if (!m_RegisteredHandlers.ContainsKey(module,messageinfo))
            {
                m_RegisteredHandlers.Add(module, messageinfo, message);
            }

        }
    }
    public class OverLayMessage
    {   
        public int id { get; set; }
        public string module { get; set; }
        public string method { get; set; }
        public string data { get; set; }
    }
    public class FollowMsg
    {
        public string FollowMessage { get; set; }

    }
    public class MediaDefinition
    {
        public MediaOptions[] Media {get;set;}
        public string MediaWidth { get; set; }
        public string MediaHeight { get; set; }
        public string MediaType { get; set; }
        public int ObjectRemovalTimeout { get; set;  }
        public string align { get; set; }
        public string position { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }
    public class MediaOptions
    {
        public string AddressUri { get; set; }
        public string ContentType { get; set; }
    }
    public class newJavaScript
    {
        public string module { get; set; }
        public string JSUri { get; set; }
    }
    public class newCss
    {
        public string module { get; set; }
        public string cssURI { get; set; }
    }
    /*
     {
	// 
		Media: [
			{
				AddressUri: "/img/kappa.gif",
				ContentType: "img/gif"
			}, 
			{
				AddressUri: "/img/kappa.ogg",
				ContentType: "video/ogg",
			},
			{
				AddressUri: "/img/kappa.mp4",
				ContentType: "video/mp4",
			},
		],
		MediaWidth: 0,
		MediaHeight: 0,
		MediaType: "video", // MediaType: "video",  MediaType:"audio",
		ObjectRemovalTimeout : 0,
		align: "center" // align: "left", align: "right"
		position: "absolute" // position: "relative"
		top: 0, 
		left: 0
		
	} */
}
