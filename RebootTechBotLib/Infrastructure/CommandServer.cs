using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib.Command;

namespace RebootTechBotLib.Infrastructure
{
    public class CommandServer
    {
        private static readonly MultiKeyDictionary<string, CommandPermissions, CommandProcessor> m_Processors = new MultiKeyDictionary<string, CommandPermissions, CommandProcessor>();
        

        public static CommandProcessor Instance(string channel, CommandPermissions perms)
        {
            lock (m_Processors)
            {
                List<CommandPermissions> permslst = new List<CommandPermissions>();
                foreach (CommandPermissions item in Enum.GetValues(typeof(CommandPermissions)))
                {
                    if ((perms & item) == item)
                    {
                        if (!m_Processors.ContainsKey(channel, item))
                            m_Processors.Add(channel, item, new CommandProcessor());
                        permslst.Add(item);
                    }
                }
            }

            if (m_Processors.ContainsKey(channel, perms))
                return m_Processors[channel, perms];
            return null;
        }

        public static void BubbleCommandRun(string channel, CommandPermissions userperms, string command)
        {
            MultiKeyDictionary<string, CommandPermissions, CommandProcessor> processorcopy = null;

            lock (m_Processors)
            {
                processorcopy = new MultiKeyDictionary<string, CommandPermissions, CommandProcessor>(m_Processors);
                if (!processorcopy.ContainsKey(channel))
                    return;
            }
            bool foundprocessor = false;
            foreach (CommandPermissions key in processorcopy[channel].Keys)
            {
                if ((userperms & key) == key)
                {
                    if (processorcopy[channel][key].Commands.HasCommand(command))
                    {
                        processorcopy[channel][key].RunCommand(command);
                        foundprocessor = true;
                        break;
                    }
                }
            }
            if (!foundprocessor)
            {

                foreach (CommandPermissions key in processorcopy[string.Empty].Keys)
                {
                    if ((userperms & key) == key)
                    {
                        if (processorcopy[string.Empty][key].Commands.HasCommand(command))
                        {
                            processorcopy[string.Empty][key].RunCommand(command);
                            foundprocessor = true;
                            break;
                        }
                    }
                }
            }
        }
        public static void AddCommand(string channel, CommandPermissions perms, string module, bool shared, string command, string help, string longhelp, CommandDelegate fn, int cooldownms =0)
        {
            lock (m_Processors)
            {

                List<CommandPermissions> permslst = new List<CommandPermissions>();
                foreach (CommandPermissions item in Enum.GetValues(typeof(CommandPermissions)))
                {
                    if ((perms & item) == item)
                    {
                        if (!m_Processors.ContainsKey(channel,item))
                            m_Processors.Add(channel, item, new CommandProcessor());
                        permslst.Add(item);
                        
                        m_Processors[channel][item].Commands.AddCommand(module, shared, command, help, longhelp, fn, cooldownms);
                    }
                }
            }
        }
        public static void RemCommand(string channel, CommandPermissions perms, string module, bool shared, string command, string help, string longhelp, CommandDelegate fn)
        {
            lock (m_Processors)
            {
                List<CommandPermissions> permslst = new List<CommandPermissions>();
                foreach (CommandPermissions item in Enum.GetValues(typeof(CommandPermissions)))
                {
                    if ((perms & item) == item)
                    {
                        if (!m_Processors.ContainsKey(channel, item))
                            m_Processors.Add(channel, item, new CommandProcessor());
                        permslst.Add(item);

                        m_Processors[channel][item].Commands.RemCommand(module, shared, command, help, longhelp, fn);
                    }

                }
            }
        }
    }
    [Flags]
    public enum CommandPermissions : int
    {
        Unknown = 1 << 0, 
        Viewer =1 << 1, 
        Regular=1 << 2, 
        Mod= 1 << 3, 
        BroadCaster= 1 << 4,
        Subscriber = 1 << 5,
        Follower = 1<<6,
        VIP = 1<<7,
        Bits1p = 1<<8,
        Bits100p = 1<<9,
        Bits1000p = 1<<10,
        Bits5000p = 1<<11,
        Bits10000p = 1<<12,
        Bits25000p = 1<<13,
        Bits50000p = 1<<14,
        Bits75000p = 1<<15,
        Bits100000p = 1<<16,
        Bits200000p = 1<<17,
        Bits300000p = 1<<18,
        Bits400000p = 1<<19,
        Bits500000p = 1<<20
    }
}
