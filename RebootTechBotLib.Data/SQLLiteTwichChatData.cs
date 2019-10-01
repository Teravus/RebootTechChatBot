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
    public class SQLLiteTwichChatData : IDisposable
    {

        private const string SelectChatMessageSQLFields = " MId, MessageId, ChannelId, UserId, UserType, UserName, IsTurbo, ChannelName, Message, Sentiment  ";
        private const string InsertChatMessageSQLFields = " MessageId, ChannelId, UserId, UserType, UserName, IsTurbo, ChannelName, Message, Sentiment ";
        private const string DeleteChatMessageSQL = "DELETE FROM ChatMessage WHERE Id=:id;";
        private const string UpdateChatMessageSQL = "UPDATE ChatMessage SET MessageId=:messageid, ChannelId=:channelid, UserId=:userid, UserType=:usertype, UserName=:username, IsTurbo=:isturbo, ChannelName=:channelname, Message=:message, Sentiment=:sentiment WHERE ";
        private const string InsertChatMessageSQL = "INSERT INTO ChatMessage (" + InsertChatMessageSQLFields + ") VALUES " + "(:messageid, :channelid, :userid, :usertype, :username, :isturbo, :channelname, :message, :sentiment);";
        private const string GetLastInsertedChatMessageSQL = "SELECT seq FROM sqlite_sequence WHERE name='ChatMessage';";


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
                dbconnect = "URI=file:TwitchChatMessage.db";
            }
            m_conn = new SQLiteConnection(dbconnect);
            m_conn.Open();

            Migration m = new Migration(m_conn, Assembly, "TwitchMessageStore");
            m.Update();
            m_conn.Close();
            m_conn.Dispose();
            m_conn = null;
            m_context = new TwitchChatMessageDBContext(TwitchChatMessageDBContext.GetConnectionString());
            return;
        }
       
        public IEnumerable<SharedChatMessage> GetChatMessagesForUser(string userId)
        {
            IEnumerable<SharedChatMessage> ChatMessages = null;
            lock (this)
            {
                string sql = "select " + SelectChatMessageSQLFields + " from ChatMessage where UserId=:userid ;";
                ChatMessages = m_context.Database.SqlQuery<SharedChatMessage>(sql, new SQLiteParameter[] { new SQLiteParameter(":userid", userId) });
            }
            return ChatMessages;
        }
        public SharedChatMessage ChatMessageSave(SharedChatMessage msg)
        {
            //:messageid, :channelid, :userid, :usertype, :username, :isturbo, :channelname, :message, :sentiment
            SQLiteParameter[] sQLiteParameters = new SQLiteParameter[]
                {
                    new SQLiteParameter(":messageid", msg.MessageId),
                    new SQLiteParameter(":channelid", msg.ChannelId),
                    new SQLiteParameter(":userid",msg.UserId),
                    new SQLiteParameter(":usertype",msg.UserType),
                    new SQLiteParameter(":username",msg.UserName),
                    new SQLiteParameter(":isturbo", msg.IsTurbo?1:0),
                    new SQLiteParameter(":channelname", msg.ChannelName),
                    new SQLiteParameter(":message", msg.Message),
                    new SQLiteParameter(":sentiment", msg.Sentiment)
                    
                };
           
                // Insert
                SharedChatMessage result = msg;
                try
                {
                    lock (this)
                    {
                        string sql = InsertChatMessageSQL;
                        m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                    }

                        int resultid = m_context.Database.SqlQuery<int>(GetLastInsertedChatMessageSQL, new SQLiteParameter[] { }).FirstOrDefault();
                        result.MId = resultid;
                }
                catch
                { }
                
                return result;
            
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
        // ~SQLLiteTwichChatData() {
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
