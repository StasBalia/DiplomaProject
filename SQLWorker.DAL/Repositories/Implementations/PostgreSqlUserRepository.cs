using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.DAL.Repositories.Records;

namespace SQLWorker.DAL.Repositories.Implementations
{
    public class PostgreSqlUserRepository : IUserRepository
    {
        private readonly ILogger _log;
        private readonly string _connectionString;
        
        public PostgreSqlUserRepository(ILogger<PostgreSqlUserRepository> log, IOptions<DatabaseSettings> dbSettings) //TODO: make test constructor pls
        {
            _log = log;
            _connectionString = dbSettings.Value.TestDatabase;
        }

        public PostgreSqlUserRepository(string connectionString, ILogger<PostgreSqlUserRepository> log)
        {
            _log = log;
            _connectionString = connectionString;
        }
        public async Task<long> SaveUserAsync(User userData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    return await connection.ExecuteScalarAsync<long>(@"INSERT INTO users (name, email, password) VALUES (@name, @email, @password) RETURNING id;", new
                    {
                        name = userData.Name,
                        email = userData.Email,
                        password = userData.Password
                    });
                }
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
                return 0;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    return await connection.QueryFirstAsync<User>(@"SELECT * FROM users WHERE email = @email", new
                    {
                        email
                    });
                }
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
                return new User();
            }
        }
    }
}