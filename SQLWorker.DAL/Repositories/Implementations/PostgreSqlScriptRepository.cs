using System;
using System.Data;
using Npgsql;
using Serilog;
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
                _log.Error("Exception while executing script {e}:",e);
                return null;
            }
        }
    }
}