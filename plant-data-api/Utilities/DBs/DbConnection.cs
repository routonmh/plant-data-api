using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PlantDataAPI.Utilities.DBs
{
    public class DbConnection : IDisposable
    {
        public MySqlConnection Connection { get; set; }

        public DbConnection()
        {
            Connection = new MySqlConnection(Program.ConnectionString);
        }

        /// <summary>
        /// Opens a new connection asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<MySqlCommand> GetCommandAsync()
        {
            await Connection.OpenAsync();
            return Connection.CreateCommand();
        }

        public void Dispose()
        {
            Connection.CloseAsync();
        }
    }
}