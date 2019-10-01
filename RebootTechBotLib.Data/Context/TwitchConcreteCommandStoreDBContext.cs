using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Core.EntityClient;

namespace RebootTechBotLib.Data.Context
{
    public class TwitchConcreteCommandStoreDBContext : DbContext
    {
        //select seq from sqlite_sequence where name="table_name"
        private const string providerconnectionstring = "./TwitchConcreteCommand.db";
        public TwitchConcreteCommandStoreDBContext() : base("metadata=res://*;provider=System.Data.SQLite;provider connection string=\"data source =./ TwitchConcreteCommand.db\"")
        {

        }
        public TwitchConcreteCommandStoreDBContext(string connectionstring) : base(connectionstring)
        {
            Database.SetInitializer<TwitchConcreteCommandStoreDBContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public static string GetConnectionString()
        {
            //Build an Entity Framework connection string
            var connectionStringBuilder = new EntityConnectionStringBuilder();
            connectionStringBuilder.ProviderConnectionString = String.Format("data source={0}", providerconnectionstring);
            connectionStringBuilder.Metadata = "res://*";
            connectionStringBuilder.Provider = "System.Data.SQLite";

            return connectionStringBuilder.ConnectionString;
        }
    }
}
