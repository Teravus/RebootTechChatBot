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
    public class SQLLiteTwitchUserData : IDisposable
    {
        
        private const string SelectUserSQLFields = " Id, UserId, UserName, DisplayName, UserType, IsTurbo, FirstTimeSeen, LastSeen, ChatTime, ReferringStreamer, TotalTimesSeen, TotalChatMessages, TotalWhisperMessages, UserScore, ProcessStatus ";
        private const string InsertUserSQLFields = " UserId, UserName, DisplayName, UserType, IsTurbo, FirstTimeSeen, LastSeen, ChatTime, ReferringStreamer, TotalTimesSeen, TotalChatMessages, TotalWhisperMessages, UserScore, ProcessStatus ";
        private const string DeleteUserSQL = "DELETE FROM TwitchUser WHERE Id=:id;";
        private const string UpdateUserSQL = "UPDATE TwitchUser SET UserId=COALESCE(:userid,UserId), UserName=COALESCE(:username,UserName), DisplayName=COALESCE(:displayname,DisplayName),UserType=COALESCE(:usertype,UserType),IsTurbo=COALESCE(:isturbo,IsTurbo),FirstTimeSeen=COALESCE(:firsttimeseen,FirstTimeSeen),LastSeen=COALESCE(:lastseen,LastSeen),ChatTime=COALESCE(:chattime,ChatTime),ReferringStreamer=COALESCE(:referringstreamer,ReferringStreamer),TotalTimesSeen=COALESCE(:totaltimesseen,TotalTimesSeen),TotalChatMessages=COALESCE(:totalchatmessages,TotalChatMessages),TotalWhisperMessages=COALESCE(:totalwhispermessages,TotalWhisperMessages),UserScore=COALESCE(:userscore, UserScore), ProcessStatus=COALESCE(:processstatus, ProcessStatus) WHERE ";
        private const string InsertUserSQL = "INSERT INTO TwitchUser (" + InsertUserSQLFields + ") VALUES " +            "(:userid, :username, :displayname, :usertype, :isturbo, :firsttimeseen, :lastseen, :chattime, :referringstreamer, :totaltimesseen, :totalchatmessages, :totalwhispermessages, :userscore, :processstatus);";
        private const string GetLastInsertedUserSQL = "SELECT seq FROM sqlite_sequence WHERE name='TwitchUser';";

        private const string SelectFollowSQLFields = " Id, FromUserId, ToUserId, FollowDate ";
        private const string InsertFollowSQLFields = " FromUserId, ToUserId, FollowDate ";
        private const string DeleteFollowSQL = "DELETE FROM TwitchFollow WHERE Id:id;";
        private const string UpdateFollowSQL = "UPDATE TwitchFollow SET FromUserId=:fromuserid, ToUserId=:touserid, FollowDate=:followdate WHERE ";
        private const string InsertFollowSQL = "INSERT INTO TwitchFollow (" + InsertFollowSQLFields + ") VALUES (:fromuserid, :touserid, :followdate);";
        private const string GetLastInsertedFollowSQL = "SELECT seq FROM sqlite_sequence WHERE name='TwitchFollow';";

        private const string SelectUserProcessSQLFields = " ProcessId, ProcessName ";
        private const string InsertUserProcessSQLFields = " ProcessName ";
        private const string DeleteUserProcessSQL = "DELETE from UserProcess WHERE ProcessId=:processid";
        private const string UpdateUserProcessSQL = "UPDATE UserProcess SET ProccessName=:processname WHERE ProcessId=:processid";
        private const string InsertUserProcessSQL = "INSERT INTO UserProcess (" + InsertUserProcessSQLFields + ") VALUES (:processname);";
        private const string GetLastInsertedUserProcessSQL = "SELECT seq FROM sqlite_sequence WHERE name='UserProcess';";

        private const string SelectUserProcessStatusSQLFields = " ProcessStatusId, ProcessId, PriorityId, ProcessStatusName ";
        private const string InsertUserProcessStatusSQLFields = " ProcessId, PriorityId, ProcessStatusName ";
        private const string DeleteUserProcessStatusSQL = "DELETE FROM UserProcessStatus WHERE ProcessStatusId=:processstatusid;";
        private const string UpdateUserProcessStatusSQL = "UPDATE UserProcessStatus SET ProcessId=:processid, PriorityId=:priorityid, ProcessStatusName=:processstatusname WHERE ProcessStatusId=:processstatusid;";
        private const string InsertUserProcessStatusSQL = "INSERT INTO UserProcessStatus (" + InsertUserProcessStatusSQLFields + ") VALUES (:processid, :priorityid, :processstatusname);";
        private const string GetLastInsertedUserProcessStatusSQL = "SELECT seq FROM sqlite_sequence WHERE name='UserProcessStatus';";

        private const string SelectUserProcessHistorySQLFields = " ProcessHistoryId, ProcessId, ProcessStatusId, CompletedDate, ChatTime, TotalChats, TotalWhispers, TotalTimesSeen, Category, StreamTitle, UserScore ";
        private const string InsertUserProcessHistorySQLFields = " ProcessId, ProcessStatusId, CompletedDate, ChatTime, TotalChats, TotalWhispers, TotalTimesSeen, Category, StreamTitle, UserScore ";
        private const string InsertUserProcessHistorySQL = "INSERT INTO UserProcessHistory (" + InsertUserProcessHistorySQLFields + ") VALUES (:processid, :processstatusid, :completeddate, :chattime, :totalchats, :totalwhispers, :totaltimesseen, :category, :streamtitle, :userscore);";

        private SQLiteConnection m_conn;
        private DbContext m_context;
        private object m_userLock = new object();
        private object m_followLock = new object();

        protected virtual Assembly Assembly
        {
            get { return GetType().Assembly; }
        }

        public void Initialise(string dbconnect)
        {


            if (dbconnect == string.Empty)
            {
                dbconnect = "URI=file:TwitchUser.db";
            }
            m_conn = new SQLiteConnection(dbconnect);
            m_conn.Open();

            Migration m = new Migration(m_conn, Assembly, "TwitchUser");
            m.Update();
            m_conn.Close();
            m_conn.Dispose();
            m_conn = null;
            m_context = new TwitchUserDBContext(TwitchUserDBContext.GetConnectionString());
            return;
        }

        public SharedUser GetUserByUserName (string UserName)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                return null;

            SharedUser user = null;
            lock(m_userLock)
            {
                try
                {
                    string sql = "select " + SelectUserSQLFields + " from TwitchUser where UserName=:username;";
                    user = m_context.Database.SqlQuery<SharedUser>(sql, new SQLiteParameter[] { new SQLiteParameter(":username", UserName) }).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
            return user;
        }

        public SharedUser GetUserById (int UserId)
        {
            if (UserId == 0)
                return null;
            SharedUser user = null;
            lock (m_userLock)
            {
                string sql = "select " + SelectUserSQLFields + " from TwitchUser where Id=:id;";
                user = m_context.Database.SqlQuery<SharedUser>(sql, new SQLiteParameter[] { new SQLiteParameter(":id", UserId) }).FirstOrDefault();
            }
            return user;
        }
        public SharedUser GetUserByTwitchUserId(string UserId)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrWhiteSpace(UserId))
                return null;

            SharedUser user = null;
            lock (m_userLock)
            {
                try
                {
                    string sql = "select " + SelectUserSQLFields + " from TwitchUser where UserId=:id and UserId <> '0';";
                    user = m_context.Database.SqlQuery<SharedUser>(sql, new SQLiteParameter[] { new SQLiteParameter(":id", UserId) }).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
            return user;
        }
        public IEnumerable<SharedUser> GetUserList()
        {
            IEnumerable<SharedUser> users = null;
            lock (m_userLock)
            {
                string sql = "select " + SelectUserSQLFields + " from TwitchUser;";
                users = m_context.Database.SqlQuery<SharedUser>(sql, new SQLiteParameter[] {  });
            }
            return users;
        }
        public SharedUser UserSave(SharedUser user)
        {

            SQLiteParameter[] sQLiteParameters = new SQLiteParameter[]
                {
                    new SQLiteParameter(":userid", user.UserId),
                    new SQLiteParameter(":username", user.UserName),
                    new SQLiteParameter(":displayname",user.DisplayName),
                    new SQLiteParameter(":usertype",user.UserType),
                    new SQLiteParameter(":isturbo",user.IsTurbo),
                    new SQLiteParameter(":firsttimeseen", user.FirstTimeSeen),
                    new SQLiteParameter(":lastseen", user.LastSeen),
                    new SQLiteParameter(":chattime", user.ChatTime),
                    new SQLiteParameter(":referringstreamer", user.ReferringStreamer),
                    new SQLiteParameter(":totaltimesseen", user.TotalTimesSeen),
                    new SQLiteParameter(":totalchatmessages", user.TotalChatMessages),
                    new SQLiteParameter(":totalwhispermessages", user.TotalWhisperMessages),
                    new SQLiteParameter(":userscore", user.UserScore),
                    new SQLiteParameter(":processstatus", user.ProcessStatus),
                    new SQLiteParameter(":id", user.Id)
                };
            if (user.Id != 0)
            {
                // Update
                SharedUser result = user;
                try
                {
                    lock (m_userLock)
                    {
                        string sql = UpdateUserSQL + " id=:id";
                        m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);
                    }

                    result = GetUserByUserName(user.UserName);
                }
                catch
                {
                    return user;
                }
                
                return result;
            }
            else 
            {
                // Insert
                SharedUser result = user;
                lock (m_userLock)
                {

                    try
                    {
                        string sql = InsertUserSQL;
                        m_context.Database.ExecuteSqlCommand(sql, sQLiteParameters);

                        int resultid = m_context.Database.SqlQuery<int>(GetLastInsertedUserSQL, new SQLiteParameter[] { }).FirstOrDefault();
                        result = GetUserById(resultid);
                    }
                    catch
                    { }
                }
                    return result;
            }
        }
        public void UserDelete(SharedUser user)
        {
            lock (m_userLock)
            {
                string sql = DeleteUserSQL;
                m_context.Database.ExecuteSqlCommand(sql, new SQLiteParameter[] { new SQLiteParameter(":id", user.Id)});
            }
        }

        public SharedFollow GetFollowById(int FollowId)
        {
            SharedFollow follow = null;
            lock (m_followLock)
            {
                string sql = "SELECT " + SelectFollowSQLFields + " FROM TwitchFollow WHERE Id=:followid;";
                follow = m_context.Database.SqlQuery<SharedFollow>(sql, new SQLiteParameter[] { new SQLiteParameter(":followid", FollowId) }).FirstOrDefault();
            }
            return follow;
        }
        public IEnumerable<SharedFollow> GetFollowsForFollowerUserId(int FollowerUserId, int StreamerUserId)
        {
            IEnumerable<SharedFollow> follows = null;
            lock (m_followLock)
            {
                string sql = "SELECT " + SelectFollowSQLFields + " FROM TwitchFollow WHERE FromUserId=:fromuserid AND ToUserId=:touserid;";
                follows = m_context.Database.SqlQuery<SharedFollow>(sql, new SQLiteParameter[] { new SQLiteParameter(":fromuserid", FollowerUserId), new SQLiteParameter(":touserid", StreamerUserId) });
            }
            return follows;
        }
        public IEnumerable<SharedFollow> GetFollowsToUserId(int ToUserId)
        {
            IEnumerable<SharedFollow> follows = null;
            lock (m_followLock)
            {
                string sql = "SELECT " + SelectFollowSQLFields + " FROM TwitchFollow WHERE ToUserId=:touserid;";
                follows = m_context.Database.SqlQuery<SharedFollow>(sql, new SQLiteParameter[] { new SQLiteParameter(":touserid", ToUserId) });
            }
            return follows;
        }
        public SharedFollow SaveFollow(SharedFollow follow)
        {
            SQLiteParameter[] parms = new SQLiteParameter[]
            {
                new SQLiteParameter(":id", follow.Id), 
                new SQLiteParameter(":fromuserid", follow.FromUserId),
                new SQLiteParameter(":touserid", follow.ToUserId), 
                new SQLiteParameter(":followdate", follow.FollowDate)
            };
            SharedFollow Result = follow;
            lock (m_followLock)
            {
                m_context.Database.ExecuteSqlCommand(InsertFollowSQL, parms);
                int lastid = m_context.Database.SqlQuery<int>(GetLastInsertedFollowSQL, new SQLiteParameter[] { }).FirstOrDefault();
                Result.Id = lastid;
            }

            return Result;
        }
        public SharedUserProcess GetUserProcessById(int ProcessId)
        {
            SharedUserProcess process = null;
            lock (m_context)
            {
                string sql = "SELECT " + SelectUserProcessSQLFields + " FROM UserProcess WHERE ProcessId=:processid;";
                process = m_context.Database.SqlQuery<SharedUserProcess>(sql, new SQLiteParameter[] { new SQLiteParameter(":processid", ProcessId) }).FirstOrDefault();
            }
            return process;
        }
        public IEnumerable<SharedUserProcess> GetUserProcesses()
        {
            IEnumerable<SharedUserProcess> process = null;
            lock (m_context)
            {
                string sql = "SELECT " + SelectUserProcessSQLFields + " FROM UserProcess;";
                process = m_context.Database.SqlQuery<SharedUserProcess>(sql, new SQLiteParameter[] {  });
            }
            return process;
        }
        public IEnumerable<SharedUserProcessHistoryEntry> GetUserProcessStatusesByProcessId( int ProcessId )
        {
            IEnumerable<SharedUserProcessHistoryEntry> processstatuses = null;
            lock (m_context)
            {
                string sql = "SELECT " + SelectUserProcessSQLFields + " FROM UserProcessStatus where ProcessId = :processid;";
                processstatuses = m_context.Database.SqlQuery<SharedUserProcessHistoryEntry>(sql, new SQLiteParameter[] { new SQLiteParameter(":processid", ProcessId) });
            }
            return processstatuses;
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
        // ~SQLLiteTwitchUserData() {
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
