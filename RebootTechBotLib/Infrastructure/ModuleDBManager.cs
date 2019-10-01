using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib.Data;
namespace RebootTechBotLib.Infrastructure
{
    public class ModuleDBManager
    {
        private static ModuleDBManager m_manager = null;
        public static ModuleDBManager Instance
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = new ModuleDBManager();
                }
                return m_manager;
            }
        }
    }
}
