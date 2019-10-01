using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib;

namespace RebootTechBotLib.Command
{
    public class CommandProcessor : ICommandProcessor
    {
        public ICommands Commands { get; private set; }
        BotOutput m_output = null;
        public CommandProcessor()
        {
            m_output = BotOutput.Instance;
            Commands = new Commands();
            //Commands.AddCommand("Help", false, string.Empty, string.Empty, string.Empty, Help);
            Commands.AddCommand("Help", 
                false, 
                "!help", 
                "!help [<item>]", 
                "Display help on a particular command or on a list of commands in a category", 
                Help
            );

        }
        private void Help(string module, string[] cmd)
        {
            List<string> help = Commands.GetHelp(cmd);
            StringBuilder output = new StringBuilder();
            foreach(string s in help)
            {
                output.Append(s);
                // Output(s)
            }
            m_output.ChatMessage(null, output.ToString());
            // output.ToString();
            //output.Instance.
        }
        public void RunCommand(string cmd)
        {
            string[] segments = Command.Commands.Parse(cmd);
            string[] resolved = Commands.Resolve(segments);

           //Command.Commands.Parse
            
        }
    }
}
