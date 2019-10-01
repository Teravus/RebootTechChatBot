using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RebootTechBotLib.Data;
using RebootTechBotLib.SharedTypes;

namespace RebootTechBotLib.Data
{
    public class BotChannelData
    {
        SQLLiteTwitchChannelData data = new SQLLiteTwitchChannelData();
        public BotChannelData()
        {
            data.Initialise(string.Empty);
        }
        public SharedChannel GetChannel(string channelname)
        {
            return data.GetChannelByChannelName(channelname);
        }
        public SharedChannel SaveChannel(SharedChannel channel)
        {
            return data.SaveChannel(channel);
        }
        public void DeleteChannel(SharedChannel chan)
        {
            data.DeleteChannel(chan);
        }
    }
    public class BotChatData
    {
        SQLLiteTwichChatData data = new SQLLiteTwichChatData();
        public BotChatData()
        {
            data.Initialise(string.Empty);
        }
        public IEnumerable<SharedChatMessage> GetChatMessagesByUserId(string userId)
        {
            return data.GetChatMessagesForUser(userId);
        }
        public SharedChatMessage InsertChatMessage(SharedChatMessage message)
        {
            return data.ChatMessageSave(message);
        }
    }
    public class BotUserData
    {
        SQLLiteTwitchUserData data = new SQLLiteTwitchUserData();
        public BotUserData()
        {
            data.Initialise(string.Empty);
           
        }
        public SharedUser GetUserById (int UserId)
        {
            return data.GetUserById(UserId);

        }
        public TwitchUser GetTwitchUserFromSharedUser(SharedUser Followeruser, SharedUser StreamerUser )
        {
            var result = new TwitchUser()
            {
                ChatTime = (uint)Followeruser.ChatTime,
                CompoundScore = Followeruser.UserScore,
                DisplayName = Followeruser.DisplayName,
                FirstTimeSeen = Followeruser.FirstTimeSeen.GetValueOrDefault(),
                LastSeen = Followeruser.LastSeen.GetValueOrDefault(),
                IsTurbo = Followeruser.IsTurbo == 1 ? true:false,
                TotalChatMessages = (uint)Followeruser.TotalChatMessages,
                ReferringStreamer = Followeruser.ReferringStreamer,
                TotalTimesSeen = (uint)Followeruser.TotalTimesSeen,
                TotalWhisperMessages = (uint)Followeruser.TotalWhisperMessages,
                UserId = Followeruser.UserId,
                UserName = Followeruser.UserName,
                UserType = (Enums.UserType)Followeruser.UserType
            };
            var followdata = data.GetFollowsForFollowerUserId(Followeruser.Id, StreamerUser.Id);
            foreach (var item in followdata)
            {
                result.FollowDate.Add(item.FollowDate);
            }
            return result;
        }
        public SharedUser GetUserByTwitchUserId (string UserId)
        {
            return data.GetUserByTwitchUserId(UserId);
        }
        public SharedUser GetUserByUserName (string UserName)
        {
            return data.GetUserByUserName(UserName);
        }
        public IEnumerable<SharedUser> GetUserList()
        {
            return data.GetUserList();
        }
        public SharedUser SaveUser(SharedUser user)
        {
            return data.UserSave(user);
        }
        public SharedFollow SaveFollow(SharedUser follower, SharedUser streamer, DateTime FollowDate)
        {
            return data.SaveFollow(new SharedFollow()
            {
                FollowDate = FollowDate,
                FromUserId = follower.Id,
                ToUserId = streamer.Id,
            });
        }
        
        public void DeleteUser(SharedUser user)
        {
            data.UserDelete(user);
        }
    }
    public class InfoChatCommandsAndPeriodicSpeak
    {
        SQLLiteTwitchConcreteCommandsStore data = new SQLLiteTwitchConcreteCommandsStore();
        public InfoChatCommandsAndPeriodicSpeak()
        {
            data.Initialise(string.Empty);
        }
        public SharedInformationalChatCommand GetCommandByCommandTriggerChanneName(string CommandTrigger, string channelname)
        {
            return data.GetCommandByCommandTriggerChanneName(CommandTrigger, channelname);
        }
        public SharedInformationalChatCommand GetCommandByCommandTrigger(string CommandTrigger)
        {
            return data.GetCommandByCommandTrigger(CommandTrigger);
        }
        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsByChannelName(string channelname)
        {
            return data.GetConcreteChatCommandsByChannelName(channelname);
        }
        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsAll()
        {
            return data.GetConcreteChatCommandsAll();
        }
        public SharedInformationalChatCommand ConcreteChatCommandSave(SharedInformationalChatCommand ChatCommand)
        {
            return data.ConcreteChatCommandSave(ChatCommand);
        }
        public void ConcreteChatCommandDelete(SharedInformationalChatCommand ChatCommand)
        {
            data.ConcreteChatCommandDelete(ChatCommand);
        }
        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakByChanneName(string channelname)
        {
            return data.GetPeriodicChatSpeakByChanneName(channelname);
        }
        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakAll()
        {
            return data.GetPeriodicChatSpeakAll();
        }
        public SharedPeriodicChatSpeak GetPeriodicChatSpeakBySpeakId(int speakid)
        {
            return data.GetPeriodicChatSpeakBySpeakId(speakid);
        }
        public SharedPeriodicChatSpeak PeriodicChatSpeakSave(SharedPeriodicChatSpeak PeriodicSpeak)
        {
            return data.PeriodicChatSpeakSave(PeriodicSpeak);
        }
        public void PeriodicChatSpeakDelete(SharedPeriodicChatSpeak ChatCommand)
        {
            data.PeriodicChatSpeakDelete(ChatCommand);
        }


    }
}
