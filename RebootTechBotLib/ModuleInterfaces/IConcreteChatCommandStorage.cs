using RebootTechBotLib.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.ModuleInterfaces
{
    public interface IConcreteChatCommandStorage
    {
        /// <summary>
        /// Get the chat commands by command trigger and channel name.
        /// </summary>
        /// <param name="CommandTrigger">The command the bot responds to.  !twitter, for example.</param>
        /// <param name="channelname">The channel that the bot listens for this command</param>
        /// <returns>Returns a chat command definition</returns>
        SharedInformationalChatCommand GetCommandByCommandTriggerChanneName(string CommandTrigger, string channelname);
        /// <summary>
        /// Gets the chat commands by command trigger
        /// </summary>
        /// <param name="CommandTrigger">The command the bot responds to.  !twitter, for example.</param>
        /// <returns></returns>
        SharedInformationalChatCommand GetCommandByCommandTrigger(string CommandTrigger);
        /// <summary>
        /// Get all chat commands for a specific channel.
        /// </summary>
        /// <param name="channelname"></param>
        /// <returns></returns>
        IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsByChannelName(string channelname);

        /// <summary>
        /// Returns all concrete chat commands
        /// </summary>
        /// <returns></returns>
        IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsAll();

        /// <summary>
        /// Saves a new concrete chat command
        /// </summary>
        /// <param name="ChatCommand">concrete chat command definition</param>
        /// <returns></returns>
        SharedInformationalChatCommand ConcreteChatCommandSave(SharedInformationalChatCommand ChatCommand);

        /// <summary>
        /// Deletes a concrete chat command
        /// </summary>
        /// <param name="ChatCommand"></param>
        void ConcreteChatCommandDelete(SharedInformationalChatCommand ChatCommand);

        /// <summary>
        /// Gets a collection of auto speak definitions by channel
        /// </summary>
        /// <param name="channelname">Name of the channel that the bot is speaking in</param>
        /// <returns></returns>
        IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakByChanneName(string channelname);

        /// <summary>
        /// Get all registered automatic speak defintions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakAll();

        /// <summary>
        /// Save a automatic speak definition.
        /// </summary>
        /// <param name="PeriodicSpeak"></param>
        /// <returns></returns>
        SharedPeriodicChatSpeak PeriodicChatSpeakSave(SharedPeriodicChatSpeak PeriodicSpeak);

        /// <summary>
        /// delete an automatic speak defition.
        /// </summary>
        /// <param name="ChatCommand"></param>
        void PeriodicChatSpeakDelete(SharedPeriodicChatSpeak ChatCommand);
        
    }
}
