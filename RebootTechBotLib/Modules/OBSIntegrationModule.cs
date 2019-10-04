using RebootTechBotLib;
using RebootTechBotLib.Command;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OBSWebsocketDotNet;

namespace RebootTechBotLib.Modules
{
    public class OBSIntegrationModule : IChatModule
    {
        BotOutput m_output;
        OBSWebsocket m_obs;
        Config.BotConfig m_config;
        List<OBSScene> m_scenes = new List<OBSScene>();
        private bool IsConnected { get; set; }
        private bool IsRecording { get; set; }
        private bool IsStreaming { get; set; }

        private int TotalStreamTime { get; set;}
        private int KbitsPerSecond { get; set; }
        private int BytesPerSec { get; set; }
        private float Framerate { get; set; }
        private float Strain { get; set; }
        private int DroppedFrames { get; set; }
        private int TotalFrames { get; set; }
        private OBSScene CurrentScene { get; set; }
        
        private Thread OBSWebSocketThread = null;
        public OBSIntegrationModule()
        {
            m_output = BotOutput.Instance;
            
            OBSWebSocketThread = new Thread(new ParameterizedThreadStart(ConnectOBS));
        }

        private void onStreamData(OBSWebsocket sender, StreamStatus status)
        {
            TotalStreamTime = status.TotalStreamTime;
            KbitsPerSecond = status.KbitsPerSec;
            BytesPerSec = status.BytesPerSec;
            Framerate = status.FPS;
            Strain = status.Strain * 100;
            DroppedFrames = status.DroppedFrames;
            TotalFrames = status.TotalFrames;
            

        }

        private void onRecordingStateChange(OBSWebsocket sender, OutputState type)
        {
            switch (type)
            {
                case OutputState.Started:
                    IsRecording = true;
                    break;
                default:
                    IsRecording = false;
                    break;
            }
        }

        private void onStreamingStateChange(OBSWebsocket sender, OutputState type)
        {
            switch (type)
            {
                case OutputState.Started:
                    IsStreaming = true;
                    break;
                default:
                    IsStreaming = false;
                    break;
            }
        }

        private void onTransitionDurationChange(OBSWebsocket sender, int newDuration)
        {
            // we don't really care for the bot right now.  throw new NotImplementedException();
        }

        private void onTransitionChange(OBSWebsocket sender, string newTransitionName)
        {
            // we don't really care for the bot right now.  
        }

        private void onProfileChange(object sender, EventArgs e)
        {
            //var scenes = m_obs.ListScenes();
            //m_scenes = scenes;
            //CurrentScene = m_obs.GetCurrentScene();
        }

        private void onSceneColChange(object sender, EventArgs e)
        {
            //var scenes = m_obs.ListScenes();
            //m_scenes = scenes;
            //CurrentScene = m_obs.GetCurrentScene();
        }

        private void onSceneChange(OBSWebsocket sender, string newSceneName)
        {
            //var scenes = m_obs.ListScenes();
            // m_scenes = scenes;
            //CurrentScene = m_obs.GetCurrentScene();
        }

        private void onDisconnect(object sender, EventArgs e)
        {
            IsConnected = false;

        }

        private void onConnect(object sender, EventArgs e)
        {
            IsConnected = true;
            var scenes = m_obs.ListScenes();
            m_scenes = scenes;
            CurrentScene = m_obs.GetCurrentScene();
        }

        public string Name => "SceneSwitcher Module";

        public bool IsShared => true;

        public void ChannelJoined(TwitchChannel channel)
        {
            
        }

        public void ChannelParted(TwitchChannel channel)
        {
            
        }
        public void Started()
        {

        }
        public void Initialize(BotConfig config)
        {
            m_config = config;
            m_obs = new OBSWebsocket();

            m_obs.Connected += onConnect;
            m_obs.Disconnected += onDisconnect;

            m_obs.SceneChanged += onSceneChange;
            m_obs.SceneCollectionChanged += onSceneColChange;
            m_obs.ProfileChanged += onProfileChange;
            m_obs.TransitionChanged += onTransitionChange;
            m_obs.TransitionDurationChanged += onTransitionDurationChange;

            m_obs.StreamingStateChanged += onStreamingStateChange;
            m_obs.RecordingStateChanged += onRecordingStateChange;

            m_obs.StreamStatus += onStreamData;
            CommandServer.AddCommand(string.Empty, CommandPermissions.Viewer , "OBSIntegrationModule", true, "!uptime", "!uptime", "Display how long the stream has been online.", PrintUptime);
            CommandServer.AddCommand(string.Empty, CommandPermissions.Mod | CommandPermissions.BroadCaster | CommandPermissions.VIP | CommandPermissions.Bits100000p, "OBSIntegrationModule", true, "!scenes", "!scenes", "Lists all scenes in the current scene collection.", PrintOBSScenes);
            CommandServer.AddCommand(string.Empty, CommandPermissions.Mod | CommandPermissions.BroadCaster | CommandPermissions.VIP | CommandPermissions.Bits100000p, "OBSIntegrationModule", true, "!switchscene", "!switchscene [sceneindex]", "Switches scene to scene identified by this scene index from the !scenes command", SwitchOBSScene);

            ConnectOBS(m_obs);
        }

        private void SwitchOBSScene(string module, string[] cmd)
        {
            var scenes = m_obs.ListScenes();
            try
            {
                int sceneID = Convert.ToInt32(cmd[1]);
                m_obs.SetCurrentScene(scenes[sceneID].Name);

            }
            catch
            {
            }
        }
        private void PrintOBSScenes(string module, string[] cmd)
        {
            var scenes = m_obs.ListScenes();
            StringBuilder builder = new StringBuilder();
            int i = -1;
            foreach (var scene in scenes)
            {
                i++;
                builder.AppendFormat("|[{0}]: {1} ", i, scene.Name);
            }
            BotOutput.Instance.ChatMessage(null, builder.ToString());
        }

        private void PrintUptime(string module, string[] cmd)
        {
            int uptimeseconds = TotalStreamTime;
            int minutes = (int)(TotalStreamTime / 60);
            int leftoverseconds = uptimeseconds - (minutes * 60);

            BotOutput.Instance.ChatMessage(null, string.Format("The stream has been online for {0} Minutes and {1} seconds.", minutes, leftoverseconds));
        }

        private void SetCurrentScene(string scenename)
        {
            OBSScene newscene = (from scene in m_scenes
                                where scene.Name.ToLowerInvariant() == scenename.ToLowerInvariant()
                                select scene).FirstOrDefault();
            m_obs.SetCurrentScene(newscene.Name);
            
        }
        private void ConnectOBS(object o)
        {
            OBSWebsocket obsws = o as OBSWebsocket;
            if (!obsws.IsConnected)
            {
                try
                {

                    obsws.Connect(OBSURL, WebSOcketPW);
                }
                catch (AuthFailureException)
                {
                    //MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (ErrorResponseException ex)
                {
                    //MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
           
        }
        private void DisconnectOBS()
        {
            
                m_obs.Disconnect();
            
        }
        public void Shutdown()
        {
            DisconnectOBS();
        }








        private string OBSURL = "ws://192.168.1.105:4444";
        private string WebSOcketPW = "F1r3st0rm";
    }
}










