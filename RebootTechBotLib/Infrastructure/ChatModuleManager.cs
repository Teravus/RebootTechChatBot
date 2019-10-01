using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public class ChatModuleManager
    {
        private Dictionary<Guid, IChatModule> m_LoadedModules = new Dictionary<Guid, IChatModule>();
        private List<Type> m_nonSharedModulesTypes = new List<Type>();
        private Dictionary<string, Dictionary<Guid, IChatModule>> m_nonSharedModules = new Dictionary<string, Dictionary<Guid, IChatModule>>();
        private Dictionary<Guid, IChatModule> m_SharedModules = new Dictionary<Guid, IChatModule>();
        private Config.BotConfig m_Config;
        private Dictionary<Type, List<object>> m_ModuleInterfaces = new Dictionary<Type, List<object>>();

        public ChatModuleManager(Config.BotConfig pBotConfig)
        {
            m_Config = pBotConfig;
        }

        public void ModulesStart()
        {
            var type = typeof(IChatModule);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);


            foreach (var t in types)
            {
                var obj = (IChatModule)Activator.CreateInstance(t);
                var id = Guid.NewGuid();

                if (obj.IsShared)
                {
                    m_LoadedModules.Add(id, obj);
                    m_SharedModules.Add(id, obj);
                }
                else
                {

                    m_nonSharedModulesTypes.Add(t);

                }
                if (obj.IsShared)
                {
                    Config.BotConfig neuteredConfig = new Config.BotConfig();
                    neuteredConfig.general = m_Config.general;
                    neuteredConfig.httpserver = m_Config.httpserver;
                    obj.Initialize(neuteredConfig);

                }
            }


        }
        private void StopModules()
        {

        }
        public void ChannelJoined (object sender, TwitchChannel joinedTwitchChannel)
        {
            Dictionary<Guid, IChatModule> loadedModulesCopy;
            lock (m_LoadedModules)
            {
                loadedModulesCopy = new Dictionary<Guid, IChatModule>(m_LoadedModules);
            }
            foreach (Guid key in loadedModulesCopy.Keys)
            {
                loadedModulesCopy[key].ChannelJoined(joinedTwitchChannel);
            }
            bool NeedToCreate = true;
            lock (m_nonSharedModules)
            {
                if (!m_nonSharedModules.ContainsKey(joinedTwitchChannel.Channel))
                    m_nonSharedModules.Add(joinedTwitchChannel.Channel, new Dictionary<Guid, IChatModule>());
                else
                {
                    NeedToCreate = false;
                }
            }

            if (NeedToCreate)
            {
                Config.BotConfig neuteredConfig = new Config.BotConfig();
                neuteredConfig.general = m_Config.general;
                neuteredConfig.httpserver = m_Config.httpserver;
                
                foreach (Type t in m_nonSharedModulesTypes)
                {

                    var obj = (IChatModule)Activator.CreateInstance(t);
                    var id = Guid.NewGuid();
                    m_LoadedModules.Add(id, obj);
                    obj.Initialize(neuteredConfig);
                    obj.ChannelJoined(joinedTwitchChannel);
                    lock (m_nonSharedModules)
                    {
                        m_nonSharedModules[joinedTwitchChannel.Channel].Add(id, obj);
                    }
                }
                foreach (var obj in m_nonSharedModules[joinedTwitchChannel.Channel].Keys)
                {
                    m_nonSharedModules[joinedTwitchChannel.Channel][obj].Started();
                }
            }
        }
        public void ChannelLeft(object sender, TwitchChannel partedTwitchChannel)
        {
            Dictionary<Guid, IChatModule> LoadedModuleCopy;
            lock (m_LoadedModules)
            {
                LoadedModuleCopy = new Dictionary<Guid, IChatModule>(m_LoadedModules);
            }

            // all modules
            foreach (Guid key in LoadedModuleCopy.Keys)
            {

                if (LoadedModuleCopy[key] == null)
                    continue;

                LoadedModuleCopy[key].ChannelParted(partedTwitchChannel);

            }

            // nonshared modules
            Dictionary<Guid, IChatModule> m_channelNonSharedModulesCopy;
            lock (m_nonSharedModules)
            {
                m_channelNonSharedModulesCopy = new Dictionary<Guid, IChatModule>(m_nonSharedModules[partedTwitchChannel.Channel]);
            }
            List<Guid> dereferenceModuleList = new List<Guid>();
            foreach (Guid key in m_channelNonSharedModulesCopy.Keys)
            {
                m_channelNonSharedModulesCopy[key].Shutdown();
                dereferenceModuleList.Add(key);
            }
            lock (m_nonSharedModules)
            {
                if (m_nonSharedModules.ContainsKey(partedTwitchChannel.Channel))
                    m_nonSharedModules.Remove(partedTwitchChannel.Channel);
            }
            lock (m_LoadedModules)
            {
                for (int i = 0; i < dereferenceModuleList.Count; i++)
                {
                    if (m_LoadedModules.ContainsKey(dereferenceModuleList[i]))
                    {
                        m_LoadedModules.Remove(dereferenceModuleList[i]);
                    }
                }
            }
        }
        
        public void RegisterModuleInterfaceHandler<T>(T type)
        {

            List<Object> l = null;
            if (!m_ModuleInterfaces.TryGetValue(typeof(T), out l))
            {
                l = new List<Object>();
                m_ModuleInterfaces.Add(typeof(T), l);
            }

            if (l.Count > 0)
                return;

            l.Add(type);
        }


        public T GetModuleInterfaceHandler<T>()
        {
            List<object> l = null;
            if (m_ModuleInterfaces.TryGetValue(typeof(T), out l))
            {
                if (l.Count > 0)
                    return (T)(l[0]);
            }
            return default(T);
        }

        internal void ModulesStarted()
        {
            Dictionary<Guid, IChatModule> LoadedModuleCopy;
            lock (m_LoadedModules)
            {
                LoadedModuleCopy = new Dictionary<Guid, IChatModule>(m_LoadedModules);
            }
            foreach (Guid key in LoadedModuleCopy.Keys)
            {
                LoadedModuleCopy[key].Started();
            }
        }
    }
}
