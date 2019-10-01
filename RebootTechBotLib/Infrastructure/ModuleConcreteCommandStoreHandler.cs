using RebootTechBotLib.Data;
using RebootTechBotLib.ModuleInterfaces;
using RebootTechBotLib.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public class ModuleConcreteCommandStoreHandler : IConcreteChatCommandStorage
    {
        private InfoChatCommandsAndPeriodicSpeak data = new InfoChatCommandsAndPeriodicSpeak();
        public void ConcreteChatCommandDelete(SharedInformationalChatCommand ChatCommand)
        {
            data.ConcreteChatCommandDelete(ChatCommand);
        }

        public SharedInformationalChatCommand ConcreteChatCommandSave(SharedInformationalChatCommand ChatCommand)
        {
            return data.ConcreteChatCommandSave(ChatCommand);
        }

        public SharedInformationalChatCommand GetCommandByCommandTrigger(string CommandTrigger)
        {
            return data.GetCommandByCommandTrigger(CommandTrigger);
        }

        public SharedInformationalChatCommand GetCommandByCommandTriggerChanneName(string CommandTrigger, string channelname)
        {
            return data.GetCommandByCommandTriggerChanneName(CommandTrigger, channelname);
        }

        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsAll()
        {
            return data.GetConcreteChatCommandsAll();
        }

        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsByChannelName(string channelname)
        {
            return data.GetConcreteChatCommandsByChannelName(channelname);
        }

        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakAll()
        {
            return data.GetPeriodicChatSpeakAll();
        }

        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakByChanneName(string channelname)
        {
            return data.GetPeriodicChatSpeakByChanneName(channelname);
        }

        public void PeriodicChatSpeakDelete(SharedPeriodicChatSpeak ChatCommand)
        {
            data.PeriodicChatSpeakDelete(ChatCommand);
        }

        public SharedPeriodicChatSpeak PeriodicChatSpeakSave(SharedPeriodicChatSpeak PeriodicSpeak)
        {
            return data.PeriodicChatSpeakSave(PeriodicSpeak);
        }
    }
}
