using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using RebootTechBotLib.ModuleInterfaces;
using RebootTechBotLib.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Modules
{
    public class ConcreteInformationCommandModule : IChatModule
    {
        public string Name => "ConcreteInformationCommandModule";

        public bool IsShared => false;
        private TwitchChannel chan = null;
        private IConcreteChatCommandStorage storagePlugin = null;

        private List<SharedInformationalChatCommand> concreteCommands = null;
        private Dictionary<string, SharedInformationalChatCommand> triggerResponses 
            = new Dictionary<string, SharedInformationalChatCommand>(StringComparer.OrdinalIgnoreCase);
        
        public void ChannelJoined(TwitchChannel channel)
        {
            chan = channel;
            // Get storage plugin
            storagePlugin = chan.GetModuleInterfaceHandler<IConcreteChatCommandStorage>();
            if (storagePlugin != null)
            {
                // get the commands out of the plugin
                IEnumerable<SharedInformationalChatCommand> storageresult 
                    = storagePlugin.GetConcreteChatCommandsByChannelName(channel.Channel);
                if (storageresult != null)
                {
                    // if not null, convert ienumerable to list.
                    concreteCommands = storageresult.ToList();
                }
                
            }
            if (concreteCommands != null)
            {
                LoadConcreteCommands(concreteCommands);
            }
            CommandServer.AddCommand(channel.Channel,CommandPermissions.Mod, "General", false, "!addcommand", "!addcommand !command cooldown \"some new response\"", "Adds a concrete 1 to 1 command response", RegisterChatCommand);
            CommandServer.AddCommand(channel.Channel, CommandPermissions.Mod, "General", false, "!remcommand", "!remcommand !command", "Adds a concrete 1 to 1 command response", UnRegisterChatCommand);
        }

        private void RegisterChatCommand(string module, string[] param)
        {
            if (param != null && param.Length > 3)
            {
                string commandstring = param[1];
                string timeout = param[2];
                int intTimeout = 0;
                Int32.TryParse(param[2], out intTimeout);
                
                StringBuilder sb = new StringBuilder();
                for (int i = 3; i < param.Length; i++)
                    sb.AppendFormat("{0} ", param[i]);

                string responsestring = sb.ToString().Trim();

                SharedInformationalChatCommand cmd = new SharedInformationalChatCommand()
                {
                    ChannelName = chan.Channel,
                    CommandPermissionRequired = CommandPermissions.Unknown.ToString(),
                    CoolDownSeconds = intTimeout,
                    CommandResponse = responsestring,
                    CommandTrigger = commandstring,
                    DateCreated = DateTime.UtcNow,
                    UserCreated = "chat",
                    IsActive = 1

                };
                cmd = storagePlugin.ConcreteChatCommandSave(cmd);
                ActivateConcreteCommand(cmd);
                BotOutput.Instance.ChatMessage(chan.Channel, string.Format("Command {0} activated", commandstring));
            }
            else
            {
                BotOutput.Instance.ChatMessage(chan.Channel, "Invalid Parameters - Please provide the command and the reponse for the command you want to add. To get help, type help !addcommand");
            }
        }

        private void UnRegisterChatCommand(string module, string[] param)
        {
            if (param != null && param.Length > 1)
            {
                string commandstring = param[1];
                SharedInformationalChatCommand cmd = null;
                
                lock (triggerResponses)
                {
                    if (triggerResponses.ContainsKey(commandstring))
                    {
                        cmd = triggerResponses[commandstring];
                    }
                }
                if (cmd != null)
                {
                    cmd.IsActive = 0;
                    cmd = storagePlugin.ConcreteChatCommandSave(cmd);
                    DeactivateConcreteCommand(cmd);
                    BotOutput.Instance.ChatMessage(chan.Channel, string.Format("Command {0} deactivated", commandstring));
                }
                else
                {
                    BotOutput.Instance.ChatMessage(chan.Channel, "Unknown command, please try again with an actual command Kappa.  For a list of all commands, type help General.");
                }

                
            }
            else
            {
                BotOutput.Instance.ChatMessage(chan.Channel, "Invalid Parameters - Please provide the command that you want to remove. To get help, type help !remcommand");
            }
        }

        private void LoadConcreteCommands(List<SharedInformationalChatCommand> commands)
        {
            foreach (SharedInformationalChatCommand cmd in commands)
            {
                if (cmd.IsActive == 1)
                    ActivateConcreteCommand(cmd);
            }
        }

        private void ActivateConcreteCommand(SharedInformationalChatCommand cmd)
        {
            if (!triggerResponses.ContainsKey(cmd.CommandTrigger))
            {
                triggerResponses.Add(cmd.CommandTrigger, cmd);
                CommandServer.AddCommand(chan.Channel, CommandPermissions.Unknown, "General", false, cmd.CommandTrigger, string.Empty, string.Empty, HandleConcreteCommand, cmd.CoolDownSeconds);
            }
        }

        private void DeactivateConcreteCommand(SharedInformationalChatCommand cmd)
        {
            lock (triggerResponses)
            {
                if (triggerResponses.ContainsKey(cmd.CommandTrigger))
                {

                    triggerResponses.Remove(cmd.CommandTrigger);
                    //triggerResponses.Remove(cmd.CommandTrigger, cmd);
                    //CommandServer.(CommandPermissions.Unknown, "General", false, cmd.CommandTrigger, string.Empty, string.Empty, HandleConcreteCommand);
                    CommandServer.RemCommand(chan.Channel, CommandPermissions.Unknown, "General", false, cmd.CommandTrigger, string.Empty, string.Empty, HandleConcreteCommand);
                }
            }
        }

        private void HandleConcreteCommand(string module, string[] command)
        {
            if (command != null && command.Length > 0)
            {

                SharedInformationalChatCommand cmd = null;
                lock (triggerResponses)
                {
                    if (triggerResponses.ContainsKey(command[0]))
                    {
                        cmd = triggerResponses[command[0]];

                    }
                }
                BotOutput.Instance.ChatMessage(chan.Channel, cmd.CommandResponse);
            }
        }
        public void ChannelParted(TwitchChannel channel)
        {
            storagePlugin = null;
            chan = null;
        }

        public void Initialize(BotConfig config)
        {
           
        }

        public void Shutdown()
        {
            
        }

        public void Started()
        {
            
        }
    }
}
