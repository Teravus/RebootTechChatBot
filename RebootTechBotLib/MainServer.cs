using RebootTechBotLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib
{
    public class MainServer
    {
        private static BaseHttpServer _HttpServerInstance = null;
        public static BaseHttpServer Instance
        {
            get
            {
                return _HttpServerInstance;  
            }
        }
        public static void SetHttpServerInstance(BaseHttpServer server)
        {
            _HttpServerInstance = server;
        }
    }
}
