using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using SQLWorker.DAL.Repositories.Interfaces;

namespace SQLWorker.DAL.Repositories.Implementations
{
    public class PostgreSqlScriptRepository : IScriptRepository
    {
        private readonly ILogger _log;
        private readonly string _connectionString;
        
        public PostgreSqlScriptRepository(ILogger log, string connectionString)
        {
            _log = log;
            _connectionString = connectionString;
        }
        public object ExecuteAndGetResult(string script)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var command = new NpgsqlCommand(script);
                return command.ExecuteScalarAsync();
            }
        }
    }
}