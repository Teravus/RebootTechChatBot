using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Config
{
    public class BotConfig
    {
        public Credentials credentials { get; set; }
        public General general { get; set; }
        public HttpServer httpserver { get; set; }
        public OBSWebSocket obswebsocket { get; set; }

        public class Credentials
        {
            public string TwitchOAuth { get; set; }
            public string TwitchClientID { get; set; }
            public string TwitchRefreshToken { get; set; }
            public string ClientSecret { get; set; }

        }
        public class General
        {
            public string Channel { get; set; }
            public string BotName { get; set; }
            public string CommandIdentifier { get; set; }
            public string BotUserId { get; set; }
        }
        public class HttpServer
        {
            public int Port { get; set; }
            public string listenip { get; set; }
            public int BacklogQueue { get; set; }
            public string DocumentRoot { get; set; }
        }
        public class OBSWebSocket
        {
            public bool Enabled { get; set; }
            public string WebSocketURL { get; set; }
            public string WebSocketPassword { get; set; }
        }
    }

}
