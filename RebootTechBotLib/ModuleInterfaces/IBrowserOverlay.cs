using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.ModuleInterfaces
{
    public delegate void BrowserOverlayConnected(object socket, string ip);
    public delegate void BrowserOverlayMessage(string module, string msginfo, string data);

    public interface IBrowserOverlay
    {
        event BrowserOverlayMessage OnOverlayMessage;
        event BrowserOverlayConnected OnOverlayConnected;
        void AddJSModule(string module, string js);
        void AddCSSModule(string module, string css);
        void SendModuleMessage(string module, string messageinfo, string message);
        void RegisterForMessage(string module, string messageinfo, BrowserOverlayMessage message);
        //utcmall /stacked

    }
}
