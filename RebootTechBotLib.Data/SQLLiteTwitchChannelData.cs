using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Data.SQLite.EF6;
using System.Linq;
using RebootTechBotLib.SharedTypes;
using RebootTechBotLib.Data.Context;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RebootTechBotLib.Data
{
    public class SQLLiteTwitchChannelData : IDisposable
    {
        private const string SelectChannelsSQL = "SELECT Id, ChannelId, Channel, CreatedDate, ModifiedDate, OwnerUserId from TwitchChannel;";
        private const string SelectChannelSQLByChannelName = "SELECT Id, ChannelId, Channel, CreatedDate, ModifiedDate, OwnerUserId from TwitchChannel where Channel=:channel;";
        private const string SelectChannelSQLByID = "SELECT Id, ChannelId, Channel, CreatedDate, ModifiedDate, OwnerUserId from TwitchChannel where Id=:id;";
        private const string DeleteChannelSQL = "DELETE FROM TwitchChannel where Channel=:channel;";
        private const string UpdateChannelSQL = "UPDATE TwitchChannel SET ChannelID=:channelid, CreatedDate=:createddate, ModifiedDate=:modifieddate, OwnerUserId=:owneruserid WHERE Channel=:channel;";
        private const string InsertChannelSQL = "INSERT INTO TwitchChannel (ChannelId, Channel, CreatedDate, ModifiedDate, OwnerUserId) VALUES (:channelid, :channel, :createddate, :modifieddate, :owneruserid); ";

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
                dbconnect = "URI=file:TwitchChannel.db";
            }
            m_conn = new SQLiteConnection(dbconnect);
            m_conn.Open();

            Migration m = new Migration(m_conn, Assembly, "TwitchChannel");
            m.Update();
            m_conn.Close();
            m_conn.Dispose();
            m_conn = null;

            m_context = new TwitchChannelDBContext(TwitchChannelDBContext.GetConnectionString());
            return;
        }

        public SharedChannel GetChannelByChannelName(string channelname)
        {
            
            // Must have a channel name.   A channel name is required.  Do nothing.  Get Nothing ಠ_ಠ
            if (string.IsNullOrEmpty(channelname) || string.IsNullOrWhiteSpace(channelname))
                return null;
            SharedChannel chan = null;
            lock (this)
            {
                chan = m_context.Database.SqlQuery<SharedChannel>(SelectChannelSQLByChannelName, new SQLiteParameter[] 
                {
                    new SQLiteParameter(":channel", channelname)
                }
                ).FirstOrDefault();

               
            }
            return chan;
        }

        public SharedChannel GetChannelById(int id)
        {
            if (id == 0)
                return null;
            SharedChannel chan = null;
            lock (this)
            {
                chan = m_context.Database.SqlQuery<SharedChannel>(SelectChannelSQLByChannelName, new SQLiteParameter[]
                   {
                    new SQLiteParameter(":id", id)
                   }
                   ).FirstOrDefault();

            }
            return chan;
        }
        public IEnumerable<SharedChannel> GetChannelList()
        {
            IEnumerable<SharedChannel> sharedChannels = null;
            lock (this)
            {
                sharedChannels = m_context.Database.SqlQuery<SharedChannel>(SelectChannelsSQL, new SQLiteParameter[0]);
            }
            return sharedChannels;
        }

        public SharedChannel SaveChannel(SharedChannel channel)
        {

            if (channel == null)
                return null;
            SharedChannel Result = null;
            SharedChannel channeltest = GetChannelByChannelName(channel.Channel);

            bool exists = channeltest != null;
            if (exists)
            {
                lock (this)
                {
                    //"UPDATE TwitchChannel SET ChannelID=:channelid, CreatedDate=:createddate, ModifiedDate=:modifieddate, OwnerUserId=:owneruserid WHERE Channel=:channel;";
                    using (SQLiteCommand cmd = new SQLiteCommand(UpdateChannelSQL, m_conn))
                    {
                        cmd.Parameters.Add(new SQLiteParameter(":channelid", channel.ChannelId));
                        cmd.Parameters.Add(new SQLiteParameter(":channel", channel.Channel));
                        cmd.Parameters.Add(new SQLiteParameter(":createddate", channel.CreatedDate)); // May have to set this to NULL if null
                        cmd.Parameters.Add(new SQLiteParameter(":modifieddate", channel.ModifiedDate)); // May have to set this to NULL if null
                        cmd.Parameters.Add(new SQLiteParameter(":owneruserid", channel.OwnerUserId));
                        cmd.ExecuteNonQuery();
                    }
                    Result = channel;
                }
            }
            else
            {
                lock (this)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(InsertChannelSQL, m_conn))
                    {
                        cmd.Parameters.Add(new SQLiteParameter(":channelid", channel.ChannelId));
                        cmd.Parameters.Add(new SQLiteParameter(":channel", channel.Channel));
                        cmd.Parameters.Add(new SQLiteParameter(":createddate", channel.CreatedDate)); // May have to set this to NULL if null
                        cmd.Parameters.Add(new SQLiteParameter(":modifieddate", channel.ModifiedDate)); // May have to set this to NULL if null
                        cmd.Parameters.Add(new SQLiteParameter(":owneruserid", channel.OwnerUserId));
                        cmd.ExecuteNonQuery();
                    }
                }
                Result = GetChannelByChannelName(channel.Channel);
            }
            return Result;
        }

        public void DeleteChannel(SharedChannel chan)
        {
            DeleteChannel(chan.Channel);
        }
        public void DeleteChannel(string channelName)
        {
            lock (this)
            {
                using (SQLiteCommand cmd = new SQLiteCommand(DeleteChannelSQL, m_conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter(":channel", channelName));
                    cmd.ExecuteNonQuery();
                }
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
                    if (m_conn != null)
                    {
                        m_conn.Close();
                        m_conn = null;
                    }
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
        // ~SQLLiteTwitchChannelData() {
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
