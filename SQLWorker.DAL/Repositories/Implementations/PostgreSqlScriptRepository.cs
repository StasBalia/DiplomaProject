using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using SQLWorker.DAL.Models;
using SQLWorker.DAL.Repositories.Interfaces;

namespace SQLWorker.DAL.Repositories.Implementations
{
    public class PostgreSqlScriptRepository : IScriptRepository
    {
        private readonly ILogger _log;
        private readonly string _connectionString;
        
        public PostgreSqlScriptRepository(ILogger<PostgreSqlScriptRepository> log, IOptions<DatabaseSettings> dbSettings)
        {
            _log = log;
            _connectionString = dbSettings.Value.TestDatabase;
        }

        public PostgreSqlScriptRepository(string connectionString, ILogger<PostgreSqlScriptRepository> log)
        {
            _log = log;
            _connectionString = connectionString;
        }
        public ScriptResult ExecuteAndGetResult(string script)
        {
            ScriptResult result = new ScriptResult();
            result.Start = DateTime.UtcNow;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var command = new NpgsqlCommand(script, connection);
                    var npgsqlDataAdapter = new NpgsqlDataAdapter(command);
                    var ds = new DataSet();
                    npgsqlDataAdapter.Fill(ds);
                    result.End = DateTime.UtcNow;
                    result.DataSet = ds;
                    return result;
                }
            }
            catch (Exception e)
            {
                _log.LogError("Exception while executing script {e}:",e);
                result.Error = e.ToString();
                result.End = DateTime.UtcNow;
                result.DataSet = null;
                return result;
            }
        }
    }
}