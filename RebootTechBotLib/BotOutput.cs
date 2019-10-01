using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib
{
    public class BotOutput
    {
        public delegate void BotChat(string channel, string text);
        public delegate void Log(string severity, string text);

        private BotChat m_ChatCallback = null;
        private Log m_LogCallback = null;
        private static BotOutput m_instance = null;
        public static BotOutput Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new BotOutput();
                return m_instance;
            }
        }
        public void ChatMessage(string channel, string text)
        {
            BotChat chat = m_ChatCallback;
            if (chat != null)
                chat(channel, text);
        }

        public void LogMessage(string severity, string text)
        {
            Log log = m_LogCallback;
            if (log != null)
                log(severity, text);
        }

        public void SetChatCallback(BotChat callback)
        {
            m_ChatCallback = callback;
        }
        public void SetLogCallback(Log callback)
        {
            m_LogCallback = callback;
        }

    }
}
