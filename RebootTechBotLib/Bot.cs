using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Communication;
using TwitchLib.Api.V5;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Communication.Events;
using RebootTechBotLib.Command;
using RebootTechBotLib.Config;
using RebootTechBotLib.SharedTypes;
using Newtonsoft.Json;
using TwitchLib.Api.Services;
using RebootTechBotLib.Infrastructure;
using TwitchLib.Api.Services.Events.FollowerService;
using RebootTechBotLib.ModuleInterfaces;

namespace RebootTechBotLib
{
    public class Bot
    {
        private static object LockObject = new object();

        private Timer saveTimer = null;

        // Twitch API
        private TwitchClient twitchClient = null;
        private TwitchLib.Api.TwitchAPI TwitchAPIi;
        private FollowerService followerService = null;
        private List<string> ChannelsToMonitorFollows = new List<string>();
        private bool FollowerServiceStarted = false;


        // delegages for the UI on the following events
        public delegate void BotLog(string message);
        public delegate void ChatMessage(string channel, RTChatMessage chatMessage);
        public delegate void UserJoinPart(string username, TwitchUser user);
        public delegate void Follow(FollowEventArgs args);

        // Events for a UI to hook to
        public event BotLog LogMessage;
        public event ChatMessage OnChatMessage;
        public event UserJoinPart OnUserJoin;
        public event UserJoinPart OnUserPart;
        public event Follow OnFollow;

    
        // Information collection for commands to use/reuse.
        private Dictionary<string, TwitchChannel> connectedChannels = new Dictionary<string, TwitchChannel>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string,DateTime> SeenUsers = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> TwitchUsernameToUserIdMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, TwitchUser> Twitch_UserId_TwitchUser = new Dictionary<string, TwitchUser>(StringComparer.OrdinalIgnoreCase);
        private Data.BotUserData userDB = new Data.BotUserData();
        private Data.BotChatData chatDB = new Data.BotChatData();
        // our bot logger and chat output
        private BotOutput m_Output = null;

        // our bot command processor
        private CommandProcessor m_CommandProcessor = new CommandProcessor();

        // The HttpServer
        private BaseHttpServer m_HttpServer;

        // The class that manages ChatModules
        private ChatModuleManager m_ChatModuleManager;
        
        // Our bot config
        private Config.BotConfig botConfig = null;
        

        public Bot()
        {
            // set our singleton output callbacks.
            m_Output = BotOutput.Instance;
            m_Output.SetChatCallback(SendTwitchChatMessage);
            m_Output.SetLogCallback(LogCallback);

            // load our config
            botConfig = LoadConfig(System.IO.Path.Combine(
                AppContext.BaseDirectory,
                "botconfig.txt"));

            // create our module manager.  we'll start it in start modules.
            m_ChatModuleManager = new ChatModuleManager(botConfig);
            m_ChatModuleManager.RegisterModuleInterfaceHandler<IConcreteChatCommandStorage>(new ModuleConcreteCommandStoreHandler());
            Data.BotChannelData dcty1 = new Data.BotChannelData();
            //SharedChannel chan = new SharedChannel()
            //{
            //    Channel = "MyCoolXYZZYxyZZYRandomChannel",
            //    ChannelId = "5678989",
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = DateTime.UtcNow,
            //    OwnerUserId = "798723978",

            //};
            //var savedchan = dcty1.SaveChannel(chan);
            //var chan = dcty1.GetChannel("MyCoolXYZZYxyZZYRandomChannel");
            //Data.BotUserData botuserdata = new Data.BotUserData();
            //SharedUser coolUser = new SharedUser()
            //{
            //    ChatTime = 5,
            //    DisplayName = "NOBODY",
            //    UserName = "NOBODY",
            //    UserType = (int)Enums.UserType.Viewer,
            //    FirstTimeSeen = DateTime.UtcNow,
            //    LastSeen = DateTime.UtcNow,
            //    IsTurbo = 0,
            //    ReferringStreamer = string.Empty,
            //    TotalChatMessages = 0,
            //    TotalTimesSeen = 1,
            //    TotalWhisperMessages = 0,
            //    UserId = "39393939",
            //    UserScore = 5,
            //    ProcessStatus = 0                
            //};
            //coolUser = botuserdata.SaveUser(coolUser);
            //SharedUser usernameUser = null;
            //usernameUser = botuserdata.GetUserByUserName("NOBODY");

            //SharedUser UserIdUser = null;
            //UserIdUser = botuserdata.GetUserByTwichUserId("39393939");
            //botuserdata.DeleteUser(UserIdUser);
        }
       
        public void Start()
        {
            // Clear the channels to monitor follows in case this isn't the first time we've run start.
            // (if we start and disconnect then run start again)
            ChannelsToMonitorFollows.Clear();

            // add our configured bot channel to the list of channels we want information about.
            ChannelsToMonitorFollows.Add(botConfig.general.Channel);

            // create our HttpServer with the information in the httpserver section of the config
            m_HttpServer = new BaseHttpServer(botConfig);
            MainServer.SetHttpServerInstance(m_HttpServer);
           

            // ---------- Twitch Stuff -----------//
            // use our twitch credentials to setup the Twitch API
            SetupTwitch(botConfig);
            // subscribe to the twitch api events
            SubscribeEvents();
            // connect to twitch
            twitchClient.Connect();

            // --------- End Twitch Stuff --------//

            // Start our HTTP Server
            m_HttpServer.Start();
            
            // Start the modules!
            StartModules();

            //m_HttpServer.AddHTTPHandler("hi", new GenericHTTPMethod(HandleHi));
            //m_CommandProcessor.Commands.AddCommand("repeat", false, "!say", "!say - repeats text", "This command repeats the text following it. Remember to enclose it in quotes", RepeatChat);
            //m_CommandProcessor.Commands.AddCommand("repeat", false, "!repeattext", "!repeatext repeats text", "This command repeats the text following it. Remember to enclose it in quotes", RepeatChat);
            saveTimer = new Timer(new TimerCallback(SaveUsers), userDB, 60000, 60000);
        }
        //private System.Collections.Hashtable HandleHi(System.Collections.Hashtable input)
        //{
        //    System.Collections.Hashtable response = new System.Collections.Hashtable();
        //    response["int_response_code"] = 200;
        //    response["str_response_string"] = "<html><head><title>Hi You!</title></head><body><H1>monkaS</H1><p>I'm not as think as you could be kindly.</p></body></html>";
        //    response["content_type"] = "text/html";

        //    return response;
        //}
        //private void RepeatChat(string module, string[] cmd)
        //{
        //    string chatresult = string.Empty;
        //    if (cmd != null && cmd.Length > 1)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 1; i < cmd.Length; i++)
        //            sb.Append(cmd[i] + " ");
        //        chatresult = sb.ToString().Trim();

        //    }
        //    if (!string.IsNullOrEmpty(chatresult))
        //        m_Output.ChatMessage(null, chatresult);
        //}

        /// <summary>
        /// Loads and creates instances of our modules and tells them to begin processing whatever they do.
        /// </summary>
        private void StartModules()
        {
            m_ChatModuleManager.ModulesStart();
            m_ChatModuleManager.ModulesStarted();
        }
        private void ShutdownSave()
        {
            m_Output.LogMessage("info", "... Saving User statuses ... ");
            List<TwitchUser> users = new List<TwitchUser>();
            lock (Twitch_UserId_TwitchUser)
            {
                users = Twitch_UserId_TwitchUser.Values.ToList();
            }
            foreach (var user in users)
            {
                if (SeenUsers.ContainsKey(user.UserName))
                {
                    
                    SharedUser suser = null;

                    if (!string.IsNullOrEmpty(user.UserId))
                        suser = userDB.GetUserByTwitchUserId(user.UserId);

                    if (suser == null && !string.IsNullOrEmpty(user.UserName))
                        suser = userDB.GetUserByUserName(user.UserName);

                    if (suser == null)
                    {
                        suser = new SharedUser()
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            FirstTimeSeen = DateTime.UtcNow
                        };
                    }
                    suser.ChatTime = (int)user.ChatTime;
                    suser.DisplayName = user.DisplayName;
                    suser.IsTurbo = user.IsTurbo ? 1 : 0;
                    suser.LastSeen = user.LastSeen;
                    suser.ProcessStatus = 0;
                    suser.ReferringStreamer = user.ReferringStreamer;
                    suser.TotalChatMessages = (int)user.TotalChatMessages;
                    suser.TotalTimesSeen = (int)user.TotalTimesSeen;
                    suser.TotalWhisperMessages = (int)user.TotalWhisperMessages;
                    suser.UserId = user.UserId;
                    suser.UserName = user.UserName;
                    suser.UserScore = 1;
                    suser.UserType = (int)user.UserType;

                    userDB.SaveUser(suser);

                    user.LastSaved = DateTime.UtcNow;
                } 
            }
            m_Output.LogMessage("info", "... Done");
        }
        private void SaveUsers( object o)
        {
            
            List<TwitchUser> users = new List<TwitchUser>();
            lock (Twitch_UserId_TwitchUser)
            {
                users = Twitch_UserId_TwitchUser.Values.ToList();
            }
            foreach (var user in users)
            {
                if (user.IsChanged == true)
                {
                    bool savenow = false;
                    if (user.LastSaved == null)
                        savenow = true;
                    if (user.LastSaved != null && user.LastSaved.HasValue && (DateTime.UtcNow - user.LastSaved.Value).TotalSeconds >  300)
                    {
                        savenow = true;
                    }
                    if (savenow)
                    {
                        user.IsChanged = false;
                        Data.BotUserData userDB = o as Data.BotUserData;
                        SharedUser suser = null;

                        if (!string.IsNullOrEmpty(user.UserId))
                            suser = userDB.GetUserByTwitchUserId(user.UserId);

                        if (suser == null && !string.IsNullOrEmpty(user.UserName))
                            suser = userDB.GetUserByUserName(user.UserName);

                        if (suser == null)
                        {
                            suser = new SharedUser()
                            {
                                UserId = user.UserId,
                                UserName = user.UserName,
                                FirstTimeSeen = DateTime.UtcNow
                            };
                        }
                        suser.ChatTime = (int)user.ChatTime;
                        suser.DisplayName = user.DisplayName;
                        suser.IsTurbo = user.IsTurbo?1:0;
                        suser.LastSeen = user.LastSeen;
                        suser.ProcessStatus = 0;
                        suser.ReferringStreamer = user.ReferringStreamer;
                        suser.TotalChatMessages = (int)user.TotalChatMessages;
                        suser.TotalTimesSeen = (int)user.TotalTimesSeen;
                        suser.TotalWhisperMessages = (int)user.TotalWhisperMessages;
                        suser.UserId = user.UserId;
                        suser.UserName = user.UserName;
                        suser.UserScore = 1;
                        suser.UserType = (int)user.UserType;

                        userDB.SaveUser(suser);
                        m_Output.LogMessage("info", string.Format("[STORE]: Saving User {0}", user.UserName));
                        user.LastSaved = DateTime.UtcNow;
                    }

                }
            }
           
        }
        /// <summary>
        /// Stops the modules.   Time to die!
        /// </summary>
        private void StopModules()
        {
            m_ChatModuleManager.ModulesStart();
        }

        /// <summary>
        /// No longer be informed of follows for some reason or other.      Should call this at shutdown or disconnect probably 
        /// </summary>
        private void StopFollowerService()
        {
            if (!FollowerServiceStarted)
                return;
            followerService.Stop();
            followerService.OnNewFollowersDetected -= FollowerService_OnNewFollowersDetected;
            FollowerServiceStarted = false;

        }

        // This is a setup and prep for the the follower service.  
        // Since this is a Web API, i'm not sure why we have to be joined to chat to actually consume it... 
        // I could write an OAUTH client to consume it with no bot active...   ಠ_ಠ
        private void SetupFollowerService()
        {
            if (FollowerServiceStarted)
                return;

            FollowerServiceStarted = true;
            //SetupFollowerService();
            var channel = twitchClient.GetJoinedChannel(botConfig.general.Channel);
            TwitchAPI twitchAPI = new TwitchAPI();
            twitchAPI.Settings.AccessToken = botConfig.credentials.TwitchOAuth;
            twitchAPI.Settings.ClientId = botConfig.credentials.TwitchClientID;
            

            followerService = new FollowerService(twitchAPI);
            followerService.SetChannelsByName(new List<string> { channel.Channel });
            followerService.OnNewFollowersDetected += FollowerService_OnNewFollowersDetected;
            followerService.Start();
            followerService.UpdateLatestFollowersAsync(false);

            //List<TwitchUser> users = GetUserById(new List<string>() { "133081326", "427795505", "100657218", "260383549", "419511568", "68752358", "426292066", "423985537", "86474067", "102281041" });
            /*{"data":[{"id":"133081326","login":"grcjudgeholden","display_name":"grcjudgeholden","type":"","broadcaster_type":"","description":"","profile_image_url":"https://static-cdn.jtvnw.net/user-default-pictures/bb97f7e6-f11a-4194-9708-52bf5a5125e8-profile_image-300x300.jpg","offline_image_url":"","view_count":1},{"id":"427795505","login":"melimelo88","display_name":"melimelo88","type":"","broadcaster_type":"","description":"","profile_image_url":"https://static-cdn.jtvnw.net/user-default-pictures/cd618d3e-f14d-4960-b7cf-094231b04735-profile_image-300x300.jpg","offline_image_url":"","view_count":418},{"id":"100657218","login":"acethenightwolf","display_name":"Acethenightwolf","type":"","broadcaster_type":"","description":"The Name Acethenightwolf but most my friends call me Ace.\rI like just about any games. And I'M not sure what else to put here.","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/acethenightwolf-profile_image-190ac11755977f76-300x300.jpeg","offline_image_url":"","view_count":38},{"id":"260383549","login":"fataljake4","display_name":"fataljake4","type":"","broadcaster_type":"","description":"","profile_image_url":"https://static-cdn.jtvnw.net/user-default-pictures/b83b1794-7df9-4878-916c-88c2ad2e4f9f-profile_image-300x300.jpg","offline_image_url":"","view_count":21},{"id":"419511568","login":"brtzyrl","display_name":"BrtzyRL","type":"","broadcaster_type":"","description":"Competitive Rocket League Player | Interactive Chat Streamer | Rocket League Coach | RLCS Qualifier Contender | ESL Monthly Tournament Contender","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/5c9c81d8-c424-4372-8863-012973abc026-profile_image-300x300.jpeg","offline_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/890e68c8-bb68-45b4-a8b8-ffd4afe73ea8-channel_offline_image-1920x1080.jpeg","view_count":63},{"id":"68752358","login":"slayernebula","display_name":"SlayerNebula","type":"","broadcaster_type":"","description":"Just here to have fun :D","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/2c855c30-5f5c-4b78-b584-b7b649ecc83d-profile_image-300x300.jpeg","offline_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/slayernebula-channel_offline_image-361b5730a5386424-1920x1080.jpeg","view_count":231},{"id":"426292066","login":"anabelleheya","display_name":"anabelleheya","type":"","broadcaster_type":"","description":"","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/aad3c066-3019-4c6c-920a-6d4a19c19ee4-profile_image-300x300.jpeg","offline_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/7546fdb9-941f-40be-8245-ee0574567f14-channel_offline_image-1920x1080.png","view_count":304},{"id":"423985537","login":"reboottechbot","display_name":"reboottechbot","type":"","broadcaster_type":"","description":"","profile_image_url":"https://static-cdn.jtvnw.net/user-default-pictures/0ecbb6c3-fecb-4016-8115-aa467b7c36ed-profile_image-300x300.jpg","offline_image_url":"","view_count":0},{"id":"86474067","login":"lantern_light","display_name":"Lantern_Light","type":"","broadcaster_type":"","description":"Hehe I play game poorly.","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/591bd1531bfadd74-profile_image-300x300.jpeg","offline_image_url":"","view_count":379},{"id":"102281041","login":"princessisabella2","display_name":"Princessisabella2","type":"","broadcaster_type":"","description":"I am the Editor in Chief of Contessa's Court magazine. It is an international fashion and luxury lifestyle magazine viewed worldwide. I  also play Minecraft and I like 7 Days to Die.","profile_image_url":"https://static-cdn.jtvnw.net/jtv_user_pictures/princessisabella2-profile_image-5a90d103e034e28a-300x300.jpeg","offline_image_url":"","view_count":522}]}
             */

        }


        /// <summary>
        /// ಠ_ಠ  The API that this calls returns both IDS and Usernames.    Why does this not return usernames also?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FollowerService_OnNewFollowersDetected(object sender, OnNewFollowersDetectedArgs e)
        {
            /*{"total":10,"data":[
              {"from_id":"133081326","from_name":"grcjudgeholden","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-04-20T00:41:59Z"},
              {"from_id":"427795505","from_name":"melimelo88","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-04-18T00:39:55Z"},
              {"from_id":"100657218","from_name":"Acethenightwolf","to_id":"102175577", "to_name":"RebootTech","followed_at":"2019-04-17T06:17:00Z"},
              {"from_id":"260383549","from_name":"fataljake4","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-04-12T02:48:20Z"},
              {"from_id":"419511568","from_name":"BrtzyRL","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-04-12T00:57:44Z"},
              {"from_id":"68752358","from_name":"SlayerNebula","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-04-04T03:18:43Z"},
              {"from_id":"426292066","from_name":"anabelleheya","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-03-28T15:23:55Z"},
              {"from_id":"423985537","from_name":"reboottechbot","to_id":"102175577","to_name":"RebootTech","followed_at":"2019-03-22T05:59:06Z"},
              {"from_id":"86474067","from_name":"Lantern_Light","to_id":"102175577","to_name":"RebootTech","followed_at":"2016-01-29T03:44:07Z"},
              {"from_id":"102281041","from_name":"Princessisabella2","to_id":"102175577","to_name":"RebootTech","followed_at":"2015-10-01T03:11:38Z"}
              ],"pagination":{"cursor":"eyJiIjpudWxsLCJhIjp7IkN1cnNvciI6IjE0NDM2NjkwOTg1MjcxMzcwMDAifX0"}}
             */
            try
            {


                int followercount = e.NewFollowers.Count;
                if (followercount > 25)
                {
                    followercount = 25;
                }
                for (int i = 0; i < followercount; i++)
                {
                    string FollowerUserId = e.NewFollowers[i].FromUserId;
                    string FollowerName = e.NewFollowers[i].FromUserName;
                    string ChannelName = e.NewFollowers[i].ToUserName;
                    string ChannelId = e.NewFollowers[i].ToUserId;
                    DateTime FollowedAt = e.NewFollowers[i].FollowedAt;
                    bool ForceInChatIgnore = false;
                    TwitchUser followeruser = ProcessMessageUserIdentityInformation(new TwitchMessage() { UserIdStr = FollowerUserId, UserName = FollowerName }, out ForceInChatIgnore);
                    TwitchUser streameruser = ProcessMessageUserIdentityInformation(new TwitchMessage() { UserIdStr = ChannelId, UserName = ChannelId }, out ForceInChatIgnore);

                    var followerdb = userDB.GetUserByTwitchUserId(followeruser.UserId);
                    var streamerdb = userDB.GetUserByTwitchUserId(streameruser.UserId);

                    if (followerdb == null)
                    {
                        // This guy is null!
                        var item = GetTwitchUserFromUserNameOnly(FollowerName);

                        followerdb = new SharedUser();
                        followerdb.ChatTime = (int)item.ChatTime;
                        followerdb.DisplayName = item.DisplayName;
                        followerdb.IsTurbo = item.IsTurbo ? 1 : 0;
                        followerdb.LastSeen = item.LastSeen;
                        followerdb.ProcessStatus = 0;
                        followerdb.ReferringStreamer = item.ReferringStreamer;
                        followerdb.TotalChatMessages = (int)item.TotalChatMessages;
                        followerdb.TotalTimesSeen = (int)item.TotalTimesSeen;
                        followerdb.TotalWhisperMessages = (int)item.TotalWhisperMessages;
                        followerdb.UserId = item.UserId;
                        followerdb.UserName = item.UserName;
                        followerdb.UserScore = 1;
                        followerdb.UserType = (int)item.UserType;

                        followerdb = userDB.SaveUser(followerdb);
                    }

                    if (streamerdb == null)
                    {
                        // This guy is null!
                        var item = GetTwitchUserFromUserNameOnly(ChannelName);

                        streamerdb = new SharedUser();
                        streamerdb.ChatTime = (int)item.ChatTime;
                        streamerdb.DisplayName = item.DisplayName;
                        streamerdb.IsTurbo = item.IsTurbo ? 1 : 0;
                        streamerdb.LastSeen = item.LastSeen;
                        streamerdb.ProcessStatus = 0;
                        streamerdb.ReferringStreamer = item.ReferringStreamer;
                        streamerdb.TotalChatMessages = (int)item.TotalChatMessages;
                        streamerdb.TotalTimesSeen = (int)item.TotalTimesSeen;
                        streamerdb.TotalWhisperMessages = (int)item.TotalWhisperMessages;
                        streamerdb.UserId = item.UserId;
                        streamerdb.UserName = item.UserName;
                        streamerdb.UserScore = 1;
                        streamerdb.UserType = (int)item.UserType;

                        streamerdb = userDB.SaveUser(streamerdb);
                    }

                    userDB.SaveFollow(followerdb, streamerdb, FollowedAt);

                    var args = new FollowEventArgs() { FollowDate = FollowedAt, Follower = followeruser, Streamer = streameruser };
                    var d = OnFollow;
                    if (d != null)
                        d(args);
                    lock (connectedChannels)
                    {
                        if (connectedChannels.ContainsKey(streameruser.UserName))
                        {
                            var chan = connectedChannels[streameruser.UserName];
                            chan.IncomingFollow(args);
                        }
                    }
                }

            }
            catch
            { }


        }

        public void ReloadConfig()
        {
            botConfig = LoadConfig(System.IO.Path.Combine(
                AppContext.BaseDirectory,
                
                "botconfig.txt"));
        }
        public void Timeout(string channel, string username, uint seconds)
        {
            if (!twitchClient.IsConnected)
            {
                var evt = LogMessage;
                if (evt != null)
                {
                    evt("Bot Is Not Connected!");
                }
                return;
            }
            if (string.IsNullOrEmpty(channel))
            {
                channel = botConfig.general.Channel;
            }
            twitchClient.TimeoutUser(channel, username, TimeSpan.FromSeconds(seconds));
            
        }

        public string GetToolTipOnUser(string username)
        {
            TwitchUser user = null;
            string result = string.Empty;
            lock (LockObject)
            {
                if (TwitchUsernameToUserIdMapping.ContainsKey(username) && Twitch_UserId_TwitchUser.ContainsKey(TwitchUsernameToUserIdMapping[username]))
                {
                    user = Twitch_UserId_TwitchUser[TwitchUsernameToUserIdMapping[username]];
                }
            }
            if (user!= null)
            {
                int chats = user.ChatMessages.Count;
                int whispers = user.WhisperMessages.Count;
                DateTime firsttimeseen = user.FirstTimeSeen;
                DateTime lastTimeSeen = user.LastSeen;
                uint chatseconds = user.ChatTime += (uint)(int)(DateTime.UtcNow - user.FirstTimeSeen).TotalSeconds;
                DateTime? Follows = null;
                if (user.FollowDate.Count > 0)
                {
                    Follows = user.FollowDate[user.FollowDate.Count - 1];
                }
                result = string.Format("{0} Chats, {1} Whispers, Follow: {2}, Chattime: {3}, FirstSeen: {4}, CS {5}", chats, whispers, Follows != null ? Follows.GetValueOrDefault().ToLocalTime().ToShortDateString() : "No", chatseconds, firsttimeseen.ToLocalTime().ToShortDateString() + " " + firsttimeseen.ToLocalTime().ToShortTimeString(), user.CompoundScore);
            }
            else
            {
                result = "no info";
            }
            return result;
        }

        public void Ban(string channel, string username, string reason)
        {
            if (!twitchClient.IsConnected)
            {
                var evt = LogMessage;
                if (evt != null)
                {
                    evt("Bot Is Not Connected!");
                }
                return;
            }
            if (string.IsNullOrEmpty(channel))
            {
                channel = botConfig.general.Channel;
            }
            twitchClient.BanUser(channel, username, reason);
        }


        private BotConfig LoadConfig(string ConfigFile)
        {
            string ConfigText = System.IO.File.ReadAllText(ConfigFile);
            BotConfig config = JsonConvert.DeserializeObject<BotConfig>(ConfigText);
            return config;
        }

        public void Stop()
        {
            if (twitchClient.IsConnected)
                twitchClient.Disconnect();
            ShutdownSave();
            UnSubscribeEvents();
            m_HttpServer.Stop();
            if (saveTimer == null)
                return;
            saveTimer.Change(
                        System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite);
            saveTimer.Dispose();
            saveTimer = null;
            
        }

        public void SubscribeEvents()
        {
            twitchClient.OnLog += Client_OnLog;
            twitchClient.OnJoinedChannel += Client_OnJoinedChannel;
            twitchClient.OnMessageReceived += Client_OnMessageReceived;
            twitchClient.OnWhisperReceived += Client_OnWhisperReceived;
            twitchClient.OnNewSubscriber += Client_OnNewSubscriber;
            twitchClient.OnConnected += Client_OnConnected;
            twitchClient.OnExistingUsersDetected += Client_OnExistingUsersDetected;
            twitchClient.OnLeftChannel += Client_OnLeftChannel;
            twitchClient.OnMessageThrottled += Client_OnMessageThrottled;
            twitchClient.OnModeratorJoined += Client_OnModeratorJoined;
            twitchClient.OnModeratorLeft += Client_OnModeratorLeft;
            twitchClient.OnReconnected += Client_OnReconnected;
            twitchClient.OnUserJoined += Client_OnUserJoined;
            twitchClient.OnUserLeft += Client_OnUserLeft;
            twitchClient.OnUserStateChanged += Client_OnUserStateChanged;
            twitchClient.OnUserTimedout += Client_OnUserTimedOut;
            twitchClient.OnUserBanned += Client_OnUserBanned;
            twitchClient.OnError += Client_OnError;
            twitchClient.OnConnectionError += Client_OnConnectionError;
            twitchClient.OnConnected += TwitchClient_OnConnected;
        }

        public void UnSubscribeEvents()
        {
            twitchClient.OnLog -= Client_OnLog;
            twitchClient.OnJoinedChannel -= Client_OnJoinedChannel;
            twitchClient.OnMessageReceived -= Client_OnMessageReceived;
            twitchClient.OnWhisperReceived -= Client_OnWhisperReceived;
            twitchClient.OnNewSubscriber -= Client_OnNewSubscriber;
            twitchClient.OnConnected -= Client_OnConnected;
            twitchClient.OnExistingUsersDetected -= Client_OnExistingUsersDetected;
            twitchClient.OnLeftChannel -= Client_OnLeftChannel;
            twitchClient.OnMessageThrottled -= Client_OnMessageThrottled;
            twitchClient.OnModeratorJoined -= Client_OnModeratorJoined;
            twitchClient.OnModeratorLeft -= Client_OnModeratorLeft;
            twitchClient.OnReconnected -= Client_OnReconnected;
            twitchClient.OnUserJoined -= Client_OnUserJoined;
            twitchClient.OnUserLeft -= Client_OnUserLeft;
            twitchClient.OnUserStateChanged -= Client_OnUserStateChanged;
            twitchClient.OnUserTimedout -= Client_OnUserTimedOut;
            twitchClient.OnUserBanned -= Client_OnUserBanned;
            twitchClient.OnError -= Client_OnError;
            twitchClient.OnConnectionError -= Client_OnConnectionError;


        }
        public void SendTwitchChatMessage(string channel, string message)
        {
            if (string.IsNullOrEmpty(channel))
            {
                channel = botConfig.general.Channel;
            }
            if (!twitchClient.IsConnected)
            {
                var evt = LogMessage;
                if (evt != null)
                {
                    evt("Bot Is Not Connected!");
                }
                return;
            }
            twitchClient.SendMessage(channel, message);
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
           
            var evt = LogMessage;
            if (evt != null)
            {
                evt("Connected");
            }
        }


        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            var evt = LogMessage;
            if (evt != null)
            {
                evt(string.Format(e.Error.Message));
            }
        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            var evt = LogMessage;
            if (evt != null)
            {
                evt(string.Format(e.Exception.ToString()));
            }
        }

        private void Client_OnUserBanned(object sender, OnUserBannedArgs e)
        {
            var bannedInfo = e.UserBan;
            //bannedInfo.BanReason;
            //bannedInfo.Channel;
            //bannedInfo.Username;
        }

        private void Client_OnUserTimedOut(object sender, OnUserTimedoutArgs e)
        {
            var timedoutinfo = e.UserTimeout;
            //timedoutinfo.Channel;
            //timedoutinfo.TimeoutDuration; // timeout in (??seconds??)
            //timedoutinfo.TimeoutReason;
            //timedoutinfo.Username;
        }

        private void Client_OnUserStateChanged(object sender, OnUserStateChangedArgs e)
        {

            //var statechangeinfo = e.UserState;
            //statechangeinfo.
        }

        private void Client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            var leftchanne = e.Channel;
            var leftuser = e.Username;
            TwitchUser user = null;
            bool WasInDictionary = false;
            lock (LockObject)
            {
                
                if (TwitchUsernameToUserIdMapping.ContainsKey(leftuser))
                {
                    user = Twitch_UserId_TwitchUser[TwitchUsernameToUserIdMapping[leftuser]];

                    user.AddChatTimeSinceLastSeen();
                    user.IsChanged = true;
                }
                if (SeenUsers.ContainsKey(leftuser))
                {
                    SeenUsers.Remove(leftuser);
                    WasInDictionary = true;
                   
                }
            }
            if (WasInDictionary)
            {
                UserJoinPart d = OnUserPart;
                if (d != null)
                {
                    d(leftuser, user);
                }
            }
        }
        private TwitchUser GetTwitchUserFromUserNameOnly(string UserName)
        {
            TwitchUser user = null;
            string userid = string.Empty;
            lock (TwitchUsernameToUserIdMapping)
            {
                if (TwitchUsernameToUserIdMapping.ContainsKey(UserName))
                    userid = TwitchUsernameToUserIdMapping[UserName];
            }
            if (string.IsNullOrEmpty(userid))
            {
                // badness
                lock (Twitch_UserId_TwitchUser)
                {
                    if (Twitch_UserId_TwitchUser.ContainsKey(userid))
                        user = Twitch_UserId_TwitchUser[userid];
                }
            }
            if (user == null)
            {
                // now we have to look it up!
                SharedUser dbuser = userDB.GetUserByUserName(UserName);
                if (dbuser == null)
                {
                    // Get User from Twitch API
                    TwitchUser tu = this.GetUserByUserNames(new List<string>() { UserName }).FirstOrDefault();
                    if (tu != null)
                    {
                        dbuser = new SharedUser()
                        {
                            ChatTime = 0,
                            DisplayName = tu.DisplayName,
                            UserName = tu.UserName,
                            UserId = tu.UserId,
                            FirstTimeSeen = DateTime.UtcNow,
                            LastSeen = DateTime.UtcNow,
                            IsTurbo = 0,
                            ProcessStatus = 0,
                            ReferringStreamer = String.Empty,
                            TotalChatMessages = 0,
                            TotalTimesSeen = 1,
                            TotalWhisperMessages = 0,
                            UserScore = 1,
                            UserType = 0
                        };
                        userDB.SaveUser(dbuser); // save this so we don't have to spend an API call again.

                        tu.Badges = new List<KeyValuePair<string, string>>();
                        tu.ChatMessages = new List<RTChatMessage>();
                        tu.ChatTime = 0;
                        tu.FirstTimeSeen = DateTime.UtcNow;
                        tu.FollowDate = new List<DateTime>();
                        tu.IsHighlightedUser = false;
                        tu.IsTurbo = false;
                        tu.LastSeen = DateTime.UtcNow;
                        tu.ReferringStreamer = string.Empty;
                        tu.TotalChatMessages = 0;
                        tu.TotalTimesSeen = 1;
                        tu.TotalWhisperMessages = 0;
                        tu.UserType = (Enums.UserType)0;
                        tu.WhisperMessages = new List<RTWhisperMessage>();
                        user = tu;
                    }
                    else
                    {
                        // I dunno who this is!
                    }
                }
                if (dbuser != null)
                {
                    user = new TwitchUser()
                    {
                        UserId = dbuser.UserId,
                        Badges = new List<KeyValuePair<string, string>>(),
                        ChatMessages = new List<RTChatMessage>(),
                        ChatTime = (uint)dbuser.ChatTime,
                        DisplayName = dbuser.DisplayName,
                        FirstTimeSeen = dbuser.FirstTimeSeen.Value,
                        FollowDate = new List<DateTime>(),
                        IsChanged = false,
                        IsTurbo = dbuser.IsTurbo == 1 ? true : false,
                        IsHighlightedUser = false,
                        ReferringStreamer = dbuser.ReferringStreamer,
                        LastSeen = DateTime.UtcNow,
                        TotalChatMessages = (uint)dbuser.TotalChatMessages,
                        TotalTimesSeen = (uint)dbuser.TotalTimesSeen,
                        TotalWhisperMessages = (uint)dbuser.TotalWhisperMessages,
                        UserName = dbuser.UserName,
                        UserType = (Enums.UserType)dbuser.UserType,
                        WhisperMessages = new List<RTWhisperMessage>()

                    };
                    lock (TwitchUsernameToUserIdMapping)
                    {
                        if (!TwitchUsernameToUserIdMapping.ContainsKey(UserName))
                            TwitchUsernameToUserIdMapping.Add(UserName, user.UserId);
                    }

                    lock (Twitch_UserId_TwitchUser)
                    {
                        if (!Twitch_UserId_TwitchUser.ContainsKey(user.UserId))
                            Twitch_UserId_TwitchUser.Add(userid, user);
                    }
                }


            }
            return user;
            
        }
        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            var joinedchannel = e.Channel;
            var joineduser = e.Username;
            bool addedToDictionary = false;
            TwitchUser user = null;
            lock (SeenUsers)
            {
                if (!SeenUsers.ContainsKey(joineduser))
                {
                    SeenUsers.Add(joineduser, DateTime.UtcNow);
                    addedToDictionary = true;
                    
                }

            }
            {

                user = GetTwitchUserFromUserNameOnly(joineduser);
                if (user == null)
                    user = new TwitchUser()
                    {
                        UserName = joineduser

                    };

                    // TODO:  If we have a user with this username in the database, pull it out of the database and stick the additional data in the bot dictionaries.
            }
            if (addedToDictionary)
            {
                UserJoinPart d = OnUserJoin;
                if (d != null)
                {
                    d(joineduser, user);
                }
            }
            
        }

        private void Client_OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            var reconnectinfo = e;
            
            
        }

        private void Client_OnModeratorLeft(object sender, OnModeratorLeftArgs e)
        {
            // When there are no moderators in the chat...  Kick on the bot moderation.
            // when there are moderators in the chat...  lower the amount of bot moderation.
        }

        private void Client_OnModeratorJoined(object sender, OnModeratorJoinedArgs e)
        {
            var modUsername = e.Username;
            var modChannel = e.Channel;

        }

        private void Client_OnMessageThrottled(object sender, OnMessageThrottledEventArgs e)
        {
            //var messagethrottledinfo = e.
        }

        private void Client_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            //var leftargs = e.
            JoinedChannel _tchan = twitchClient.GetJoinedChannel(e.Channel);
            TwitchChannel partedTwitchChannel = null;
            lock (connectedChannels)
            {
                if (connectedChannels.ContainsKey(e.Channel))
                    partedTwitchChannel = connectedChannels[e.Channel];
                else
                    partedTwitchChannel = new TwitchChannel(_tchan, m_ChatModuleManager);
                if (connectedChannels.ContainsKey(e.Channel))
                    connectedChannels.Remove(e.Channel);
            }
            m_ChatModuleManager.ChannelLeft(sender, partedTwitchChannel);

        }

        private void Client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            var userlist = e.Users;
            string Channel = e.Channel;
            List<string> existingusers = e.Users;

        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            //var connectedinfo = e.
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
           
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            var message = new RTWhisperMessage(e.WhisperMessage);
            bool triggerjoin = false;
            TwitchUser user = ProcessMessageUserIdentityInformation(message, out triggerjoin);
            if (triggerjoin)
            {
                UserJoinPart d = OnUserJoin;
                if (d != null)
                {
                    d(user.UserName, user);
                }
            }
            user.WhisperMessages.Add(message);
            // message.
           // m_CommandProcessor.RunCommand(message.Message);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ThreadPool.QueueUserWorkItem(Client_OnMessageReceivedThread, e);
        }
        private void Client_OnMessageReceivedThread(object o)
        {
            OnMessageReceivedArgs e = o as OnMessageReceivedArgs;
            var message = new RTChatMessage(e.ChatMessage);

            bool triggerjoin = false;
            TwitchUser user = ProcessMessageUserIdentityInformation(message, out triggerjoin);
            if (triggerjoin)
            {
                UserJoinPart de = OnUserJoin;
                if (de != null)
                {
                    de(user.UserName, user);
                }
            }

            user.LogChatMessage(message);

            ChatMessage d = OnChatMessage;
            if (d != null)
                d(e.ChatMessage.Channel, message);
            // Run the command on the command processor
            CommandServer.BubbleCommandRun(e.ChatMessage.Channel, Utility.Utilities.GetCommandPermissionFromTwitchLibUserType(e.ChatMessage.UserType, (int)user.ChatTime), message.Message);
            TwitchChannel twitchChannel = null;
            lock (connectedChannels)
            {
                if (connectedChannels.ContainsKey(e.ChatMessage.Channel))
                    twitchChannel = connectedChannels[e.ChatMessage.Channel];
                else
                {
                    JoinedChannel _tchan = twitchClient.GetJoinedChannel(e.ChatMessage.Channel);
                    twitchChannel = new TwitchChannel(_tchan, m_ChatModuleManager);
                }
            }
            if (twitchChannel != null)
                twitchChannel.IncomingChatMessage(new RTChatMessage(e.ChatMessage));

            ThreadPool.QueueUserWorkItem(new WaitCallback(SaveChatMessage), new ChatSaveObject(chatDB) { message = new RTChatMessage(e.ChatMessage) });
        }

        // TODO: Extract to separate file.
        class ChatSaveObject
        {
            public ChatSaveObject(Data.BotChatData pData)
            {
                _data = pData;
            }
            private Data.BotChatData _data = null;
            public Data.BotChatData data { get { return _data; } }
            public RTChatMessage message { get; set; }
        }
        private void SaveChatMessage(object o)
        {
            ChatSaveObject ChatSaveArgs = o as ChatSaveObject;
            ChatSaveArgs.message.CalulateSentiment();
            ChatSaveArgs.data.InsertChatMessage(new SharedChatMessage()
            {
                ChannelId = ChatSaveArgs.message.ChannelId,
                ChannelName = ChatSaveArgs.message.Channel,
                IsTurbo = ChatSaveArgs.message.IsTurbo, 
                Message = ChatSaveArgs.message.Message, 
                MessageId = ChatSaveArgs.message.MessageIdStr,
                Sentiment = ChatSaveArgs.message.Sentiment,
                UserId = ChatSaveArgs.message.UserIdStr,
                UserName = ChatSaveArgs.message.UserName,
                UserType = (int)(uint)ChatSaveArgs.message.UserType

            });
            
        }
        private TwitchUser ProcessMessageUserIdentityInformation(TwitchMessage message, out bool ForceJoinChat)
        {
            TwitchUser twitchUser = null;
            bool triggerjoin = false;
            lock (LockObject)
            {
                if (TwitchUsernameToUserIdMapping.ContainsKey(message.UserName))
                    TwitchUsernameToUserIdMapping[message.UserName] = message.UserIdStr;
                else
                {
                    TwitchUsernameToUserIdMapping.Add(message.UserName, message.UserIdStr);
                }
                if (Twitch_UserId_TwitchUser.ContainsKey(message.UserIdStr))
                {
                    Twitch_UserId_TwitchUser[message.UserIdStr].LastSeen = DateTime.UtcNow;
                    twitchUser = Twitch_UserId_TwitchUser[message.UserIdStr];
                }
                else
                {
                    TwitchUser useritem = new TwitchUser()
                    {
                        Badges = message.Badges,
                        ChatTime = 0,
                        FirstTimeSeen = DateTime.UtcNow,
                        LastSeen = DateTime.UtcNow,
                        DisplayName = message.DisplayName,
                        UserName = message.UserName,
                        IsTurbo = message.IsTurbo,
                        IsHighlightedUser = false,
                        ReferringStreamer = string.Empty,
                        UserId = message.UserIdStr,
                        UserType = (Enums.UserType)(int)message.UserType,
                        ChatMessages = new List<RTChatMessage>(),
                        TotalWhisperMessages = 0,
                        TotalChatMessages = 0,
                        FollowDate = new List<DateTime>(),
                        TotalTimesSeen = 1,
                        WhisperMessages = new List<RTWhisperMessage>(),
                        IsChanged = true
                    };
                    Twitch_UserId_TwitchUser.Add(message.UserIdStr, useritem);
                    twitchUser = useritem;
                    triggerjoin = true;
                }
            }
            
            ForceJoinChat = triggerjoin;
            return twitchUser;
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            JoinedChannel _tchan = twitchClient.GetJoinedChannel(e.Channel);
            TwitchChannel joinedTwitchChannel = new TwitchChannel(_tchan, m_ChatModuleManager);

           
            
            // This is the bot joining the channel.

            // You can only set up the follower service once the bot has joined the channel!
            SetupFollowerService();
            lock (connectedChannels)
            {
                if (!connectedChannels.ContainsKey(e.Channel))
                    connectedChannels.Add(e.Channel, joinedTwitchChannel);
            }

            ChannelJoinedThreadParams parms = new ChannelJoinedThreadParams()
            {
                botref = this,
                joinedtwitchchannel = joinedTwitchChannel,
                modman = m_ChatModuleManager
            };
            ThreadPool.QueueUserWorkItem(ExecuteChannelJoinedOnSeparateThread, parms);
            


        }
        public class ChannelJoinedThreadParams
        {
            public Bot botref { get; set; }
            public ChatModuleManager modman { get; set; }
            public TwitchChannel joinedtwitchchannel { get; set; }
        }
        private void ExecuteChannelJoinedOnSeparateThread( object o)
        {
            ChannelJoinedThreadParams parms = o as ChannelJoinedThreadParams;
            parms.modman.ChannelJoined(parms.botref, parms.joinedtwitchchannel);
            ///m_ChatModuleManager.ChannelJoined(this, joinedTwitchChannel);
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            var evt = LogMessage;
            if (evt != null)
            {
                evt(string.Format(e.Data));
            }
        }
        public void LogCallback(string severity, string message)
        {
            var evt = LogMessage;
            if (evt != null)
            {
                evt(string.Format(message));
            }
        }


        public void SetupTwitch(Config.BotConfig pBotConfig)
        {
            ConnectionCredentials creds = new ConnectionCredentials(pBotConfig.general.BotUserId, pBotConfig.credentials.TwitchOAuth);
     
            twitchClient = new TwitchClient();
            twitchClient.Initialize(creds, pBotConfig.general.Channel);
            TwitchAPIi = new TwitchAPI();
            TwitchAPIi.Helix.Settings.ClientId = pBotConfig.credentials.TwitchClientID;
            TwitchAPIi.Helix.Settings.Secret = pBotConfig.credentials.ClientSecret;
            

        }
        

        private List<TwitchUser> GetUserById(List<string> id)
        {


            List<TwitchUser> userdata = new List<TwitchUser>();
            var results = TwitchAPIi.Helix.Users.GetUsersAsync(ids:id).Result;
            foreach (var result in results.Users)
            {
                //result.Id;
                //result.DisplayName;
                //result.Login;
                //result.Type;
                userdata.Add(new TwitchUser()
                {
                     UserId = result.Id,
                      DisplayName = result.DisplayName,
                     UserName = result.Login
                });
            }

            return userdata;
        }
        private List<TwitchUser> GetUserByUserNames(List<string> Names)
        {


            List<TwitchUser> userdata = new List<TwitchUser>();
            var results = TwitchAPIi.Helix.Users.GetUsersAsync(logins:Names).Result;
            foreach (var result in results.Users)
            {
                //result.Id;
                //result.DisplayName;
                //result.Login;
                //result.Type;
                
                userdata.Add(new TwitchUser()
                {
                    UserId = result.Id,
                    DisplayName = string.IsNullOrEmpty(result.DisplayName)?result.Login:result.DisplayName,
                    UserName = result.Login, 
                    
                });
            }

            return userdata;
        }


    }
}
