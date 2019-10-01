using RebootTechBotLib.Data.Context;
using RebootTechBotLib.SharedTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity;

namespace RebootTechBotLib.Data
{
    public class SQLLiteTwitchConcreteCommandsStore : IDisposable
    {

        private const string SelectConcreteChatCommandSQLFields = " CommandId, ChannelName, CommandTrigger, CommandResponse, UserCreated, DateCreated, UserModified, DateModified, CommandPermissionRequired, CoolDownSeconds, IsActive  ";
        private const string InsertConcreteChatCommandSQLFields = " ChannelName, CommandTrigger, CommandResponse, UserCreated, DateCreated, UserModified, DateModified, CommandPermissionRequired, CoolDownSeconds, IsActive ";
        private const string DeleteConcreteChatCommandSQL = "DELETE FROM InformationalChatCommand WHERE CommandId=:commandid;";
        private const string UpdateConcreteChatCommandSQL = "UPDATE InformationalChatCommand SET ChannelName=COALESCE(:channelname,ChannelName), CommandTrigger=COALESCE(:commandtrigger,CommandTrigger), CommandResponse=COALESCE(:commandresponse,CommandResponse),UserCreated=COALESCE(:usercreated,UserCreated),DateCreated=COALESCE(:datecreated,DateCreated),UserModified=COALESCE(:usermodified,UserModified),DateModified=COALESCE(:datemodified,DateModified),CommandPermissionRequired=COALESCE(:commandpermissionrequired,CommandPermissionRequired),CoolDownSeconds=COALESCE(:cooldownseconds,CoolDownSeconds),IsActive=COALESCE(:isactive,IsActive) WHERE ";
        private const string InsertConcreteChatCommandSQL = "INSERT INTO InformationalChatCommand (" + InsertConcreteChatCommandSQLFields + ") VALUES " + "(:channelname, :commandtrigger, :commandresponse, :usercreated, :datecreated, :usermodified, :datemodified, :commandpermissionrequired, :cooldownseconds, :isactive);";
        private const string GetLastInsertedConcreteChatCommandSQL = "SELECT seq FROM sqlite_sequence WHERE name='InformationalChatCommand';";

        private const string SelectPeriodicChatSpeakSQLFields = " SpeakId, ChannelName, SpeakText, CoolDownSeconds, UserCreated, DateCreated, UserModified, DateModified, IsActive  ";
        private const string InsertPeriodicChatSpeakSQLFields = " ChannelName, SpeakText, CoolDownSeconds, UserCreated, DateCreated, UserModified, DateModified, IsActive ";
        private const string DeletePeriodicChatSpeakSQL = "DELETE FROM PeriodicChatSpeak WHERE CommandId=:commandid;";
        private const string UpdatePeriodicChatSpeakSQL = "UPDATE InformationaPeriodicChatSpeaklChatCommand SET ChannelName=COALESCE(:channelname,ChannelName), SpeakText=COALESCE(:speaktext,SpeakText), CoolDownSeconds=COALESCE(:cooldownseconds,CoolDownSeconds),UserCreated=COALESCE(:usercreated,UserCreated),DateCreated=COALESCE(:datecreated,DateCreated),UserModified=COALESCE(:usermodified,UserModified),DateModified=COALESCE(:datemodified,DateModified),IsActive=COALESCE(:isactive,IsActive) WHERE ";
        private const string InsertPeriodicChatSpeakSQL = "INSERT INTO PeriodicChatSpeak (" + InsertConcreteChatCommandSQLFields + ") VALUES " + "(:channelname, :speaktext, :cooldownseconds, :usercreated, :datecreated, :usermodified, :datemodified, :isactive);";
        private const string GetLastInsertedPeriodicChatSpeakCommandSQL = "SELECT seq FROM sqlite_sequence WHERE name='PeriodicChatSpeak';";


        private SQLiteConnection m_conn;
        private DbContext m_context;

        protected virtual Assembly Assembly
        {
            get { return GetType().Assembly; }
        }
        public void Initialise(string dbconnect)
        {
            if (dbconnect == string.Empty)
            {
                dbconnect = "URI=file:TwitchConcreteCommand.db";
            }
            m_conn = new SQLiteConnection(dbconnect);
            m_conn.Open();

            Migration m = new Migration(m_conn, Assembly, "ConcreteCommandsStore");
            m.Update();
            m_conn.Close();
            m_conn.Dispose();
            m_conn = null;
            m_context = new TwitchConcreteCommandStoreDBContext(TwitchConcreteCommandStoreDBContext.GetConnectionString());
            return;
        }
        public SharedInformationalChatCommand GetCommandByCommandTriggerChanneName(string CommandTrigger, string channelname)
        {
            if (string.IsNullOrEmpty(CommandTrigger) || string.IsNullOrWhiteSpace(CommandTrigger))
                return null;

            SharedInformationalChatCommand ChatCommand = null;
            lock (this)
            {
                string sql = "select " + SelectConcreteChatCommandSQLFields + " from InformationalChatCommand where CommandTrigger=:commandtrigger AND ChannelName=:channelname;";
                ChatCommand = m_context.Database.SqlQuery<SharedInformationalChatCommand>(sql, 
                    new SQLiteParameter[] 
                    {
                        new SQLiteParameter(":commandtrigger", CommandTrigger),
                        new SQLiteParameter(":channelname", channelname)
                    }).FirstOrDefault();
            }
            return ChatCommand;
        }
        public SharedInformationalChatCommand GetCommandByCommandTrigger(string CommandTrigger)
        {
            if (string.IsNullOrEmpty(CommandTrigger) || string.IsNullOrWhiteSpace(CommandTrigger))
                return null;

            SharedInformationalChatCommand ChatCommand = null;
            lock (this)
            {
                string sql = "select " + SelectConcreteChatCommandSQLFields + " from InformationalChatCommand where CommandTrigger=:commandtrigger;";
                ChatCommand = m_context.Database.SqlQuery<SharedInformationalChatCommand>(sql,
                    new SQLiteParameter[]
                    {
                        new SQLiteParameter(":commandtrigger", CommandTrigger)
                    }).FirstOrDefault();
            }
            return ChatCommand;
        }

        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsByChannelName(string channelname)
        {
            IEnumerable<SharedInformationalChatCommand> ChatCommands = null;
            lock (this)
            {
                string sql = "select " + SelectConcreteChatCommandSQLFields + " from InformationalChatCommand where ChannelName=:channelname;";
                ChatCommands = m_context.Database.SqlQuery<SharedInformationalChatCommand>(sql, new SQLiteParameter[] { new SQLiteParameter(":channelname", channelname) });
            }
            return ChatCommands;
        }

        public IEnumerable<SharedInformationalChatCommand> GetConcreteChatCommandsAll()
        {
            IEnumerable<SharedInformationalChatCommand> ChatCommands = null;
            lock (this)
            {
                string sql = "select " + SelectConcreteChatCommandSQLFields + " from InformationalChatCommand;";
                ChatCommands = m_context.Database.SqlQuery<SharedInformationalChatCommand>(sql, new SQLiteParameter[] { });
            }
            return ChatCommands;
        }

        public SharedInformationalChatCommand ConcreteChatCommandSave(SharedInformationalChatCommand ChatCommand)
        {

            SQLiteParameter[] sQLiteParameters = new SQLiteParameter[]
                {
                    new SQLiteParameter(":channelname", ChatCommand.ChannelName),
                    new SQLiteParameter(":commandtrigger", ChatCommand.CommandTrigger),
                    new SQLiteParameter(":commandresponse",ChatCommand.CommandResponse),
                    new SQLiteParameter(":usercreated",ChatCommand.UserCreated),
                    new SQLiteParameter(":datecreated",ChatCommand.DateCreated),
                    new SQLiteParameter(":usermodified", ChatCommand.UserModified),
                    new SQLiteParameter(":datemodified", ChatCommand.DateModified),
                    new SQLiteParameter(":commandpermissionrequired", ChatCommand.CommandPermissionRequired),
                    new SQLiteParameter(":cooldownseconds", ChatCommand.CoolDownSeconds),
                    new SQLiteParameter(":isactive", ChatCommand.IsActive),
                    new SQLiteParameter(":commandid", ChatCommand.CommandId)
                };
            if (ChatCommand.CommandId != 0)
            {
                // Update
                SharedInformationalChatCommand result = ChatCommand;
                lock (this)
                {
                    string sql = UpdateConcreteChatCommandSQL + " CommandId=:commandid";
                    m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                }
                result = GetCommandByCommandTrigger(ChatCommand.CommandTrigger);


                return result;
            }
            else
            {
                // Insert
                SharedInformationalChatCommand result = ChatCommand;
                //lock (this)
                //{
                string sql = InsertConcreteChatCommandSQL;
                m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                //}
                int resultid = m_context.Database.SqlQuery<int>(GetLastInsertedConcreteChatCommandSQL, new SQLiteParameter[] { }).FirstOrDefault();
                result = GetCommandByCommandTrigger(ChatCommand.CommandTrigger);
                return result;
            }
        }
        public void ConcreteChatCommandDelete(SharedInformationalChatCommand ChatCommand)
        {
            lock (this)
            {
                string sql = DeleteConcreteChatCommandSQL;
                m_context.Database.ExecuteSqlCommand(sql, new SQLiteParameter[] { new SQLiteParameter(":commandid", ChatCommand.CommandId) });
            }
        }

        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakByChanneName(string channelname)
        {
            if (string.IsNullOrEmpty(channelname) || string.IsNullOrWhiteSpace(channelname))
                return null;

            IEnumerable<SharedPeriodicChatSpeak> ChatCommand = null;
            lock (this)
            {
                string sql = "select " + SelectPeriodicChatSpeakSQLFields + " from PeriodicChatSpeak where ChannelName=:channelname;";
                ChatCommand = m_context.Database.SqlQuery<SharedPeriodicChatSpeak>(sql,
                    new SQLiteParameter[]
                    {
                        new SQLiteParameter(":channelname", channelname)
                    });
            }
            return ChatCommand;
        }
        public IEnumerable<SharedPeriodicChatSpeak> GetPeriodicChatSpeakAll()
        {
            
            IEnumerable<SharedPeriodicChatSpeak> ChatCommand = null;
            lock (this)
            {
                string sql = "select " + SelectPeriodicChatSpeakSQLFields + " from PeriodicChatSpeak;";
                ChatCommand = m_context.Database.SqlQuery<SharedPeriodicChatSpeak>(sql,
                    new SQLiteParameter[]
                    {
                     
                    });
            }
            return ChatCommand;
        }
        public SharedPeriodicChatSpeak GetPeriodicChatSpeakBySpeakId( int speakid)
        {

            SharedPeriodicChatSpeak ChatCommand = null;
            lock (this)
            {
                string sql = "select " + SelectPeriodicChatSpeakSQLFields + " from PeriodicChatSpeak WHERE SpeakId=:speakid;";
                ChatCommand = m_context.Database.SqlQuery<SharedPeriodicChatSpeak>(sql,
                    new SQLiteParameter[]
                    {

                    }).FirstOrDefault();
            }
            return ChatCommand;
        }
        public SharedPeriodicChatSpeak PeriodicChatSpeakSave(SharedPeriodicChatSpeak PeriodicSpeak)
        {

            SQLiteParameter[] sQLiteParameters = new SQLiteParameter[]
                {
                    new SQLiteParameter(":channelname", PeriodicSpeak.ChannelName),
                    new SQLiteParameter(":speaktext", PeriodicSpeak.SpeakText),
                    new SQLiteParameter(":cooldownseconds",PeriodicSpeak.CoolDownSeconds),
                    new SQLiteParameter(":usercreated",PeriodicSpeak.UserCreated),
                    new SQLiteParameter(":datecreated",PeriodicSpeak.DateCreated),
                    new SQLiteParameter(":usermodified", PeriodicSpeak.UserModified),
                    new SQLiteParameter(":datemodified", PeriodicSpeak.DateModified),
                    new SQLiteParameter(":isactive", PeriodicSpeak.IsActive),
                    new SQLiteParameter(":speakid", PeriodicSpeak.SpeakId)
                };
            if (PeriodicSpeak.SpeakId != 0)
            {
                // Update
                SharedPeriodicChatSpeak result = PeriodicSpeak;
                lock (this)
                {
                    string sql = UpdatePeriodicChatSpeakSQL + " CommandId=:commandid";
                    m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                }
                result = GetPeriodicChatSpeakBySpeakId(PeriodicSpeak.SpeakId); 


                return result;
            }
            else
            {
                // Insert
                SharedPeriodicChatSpeak result = PeriodicSpeak;
                //lock (this)
                //{
                string sql = InsertPeriodicChatSpeakSQLFields;
                m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                //}
                int resultid = m_context.Database.SqlQuery<int>(GetLastInsertedPeriodicChatSpeakCommandSQL, new SQLiteParameter[] { }).FirstOrDefault();
                result = GetPeriodicChatSpeakBySpeakId(resultid);
                return result;
            }
        }
        public void PeriodicChatSpeakDelete(SharedPeriodicChatSpeak ChatCommand)
        {
            lock (this)
            {
                string sql = DeletePeriodicChatSpeakSQL;
                m_context.Database.ExecuteSqlCommand(sql, new SQLiteParameter[] { new SQLiteParameter(":speakid", ChatCommand.SpeakId) });
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    // TODO: dispose managed state (managed objects).
                    if (m_conn != null)
                    {
                        m_conn.Close();
                        m_conn.Dispose();
                    }
                    m_conn = null;
                    if (m_context != null)
                    {
                        m_context.Dispose();
                        m_context = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SQLLiteTwitchConcreteCommandsStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
