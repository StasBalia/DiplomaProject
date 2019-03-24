using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using SQLWorker.DAL.Repositories.Interfaces;

namespace SQLWorker.DAL.Repositories.Implementations
{
    public class PostgreSqlScriptRepository : IScriptRepository
    {
        private readonly ILogger _log;
        private readonly string _connectionString;
        
        public PostgreSqlScriptRepository(ILogger<PostgreSqlScriptRepository> log, IOptions<DatabaseSettings> dbSettings) //TODO: make test constructor pls
        {
            _log = log;
            _connectionString = dbSettings.Value.TestDatabase;
        }

        public PostgreSqlScriptRepository(string connectionString, ILogger<PostgreSqlScriptRepository> log)
        {
            _log = log;
            _connectionString = connectionString;
        }
        public DataSet ExecuteAndGetResult(string script)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var command = new NpgsqlCommand(script, connection);
                    var npgsqlDataAdapter = new NpgsqlDataAdapter(command);
                    var ds = new DataSet();
                    npgsqlDataAdapter.Fill(ds);
                    return ds;
                }
            }
            catch (Exception e)
            {
                _log.LogError("Exception while executing script {e}:",e);
                return null;
            }
        }
    }
}