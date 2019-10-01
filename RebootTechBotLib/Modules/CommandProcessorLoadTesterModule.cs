using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using RebootTechBotLib.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Modules
{
    public class CommandProcessorLoadTesterModule : IChatModule
    {
        Random rng = new Random();
        const string AllowedChars =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        public string Name => "ConcreteInformationCommandModule";

        public bool IsShared => false;
        private TwitchChannel chan = null;
        private Dictionary<string, SharedInformationalChatCommand> triggerResponses
            = new Dictionary<string, SharedInformationalChatCommand>(StringComparer.OrdinalIgnoreCase);

        public void ChannelJoined(TwitchChannel channel)
        {
            chan = channel;
            //for (int i = 0; i < 10000; i++)
            //{
            //    string name = CreateString(5);
            //    string response = CreateString(35);
            //    if (!triggerResponses.ContainsKey(name))
            //        triggerResponses.Add(name, new SharedInformationalChatCommand()
            //        {
            //            ChannelName = channel.Channel,
            //            CommandId = 0,
            //            CommandPermissionRequired = string.Empty,
            //            CommandResponse = response,
            //            CommandTrigger = name,
            //            CoolDownSeconds = 0,
            //            IsActive = 1,
            //            DateCreated = DateTime.Now,
            //            UserCreated = "c"

            //        });
            //}
            //foreach (var item in triggerResponses.Keys)
            //{
            //    ActivateConcreteCommand(triggerResponses[item]);
            //}

        }
        public void ChannelParted(TwitchChannel channel)
        {
            
        }

        public void Initialize(BotConfig config)
        {
            
        }

        public string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rng.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public void Shutdown()
        {
        }

        public void Started()
        {
          
        }
        private void ActivateConcreteCommand(SharedInformationalChatCommand cmd)
        {
            //if (!triggerResponses.ContainsKey(cmd.CommandTrigger))
            //{
               // triggerResponses.Add(cmd.CommandTrigger, cmd);
                CommandServer.AddCommand(chan.Channel, CommandPermissions.Unknown | CommandPermissions.Viewer | CommandPermissions.VIP | CommandPermissions.Bits1000p | CommandPermissions.Follower | CommandPermissions.BroadCaster | CommandPermissions.Subscriber | CommandPermissions.Bits100p, "General", false, cmd.CommandTrigger, string.Empty, string.Empty, HandleConcreteCommand, cmd.CoolDownSeconds);
            //}
        }
        private void HandleConcreteCommand(string module, string[] command)
        {
            BotOutput.Instance.LogMessage("info", "Got Command: " + command);
        }
    }
}
