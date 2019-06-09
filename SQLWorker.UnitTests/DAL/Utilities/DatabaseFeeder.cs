using System.IO;
using Dapper;
using Npgsql;

namespace SQLWorker.UnitTests.DAL.Utilities
{
    public class DatabaseFeeder
    {
        private readonly string _connectionString;

        public DatabaseFeeder(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void Up()
        {
            string sql = File.ReadAllText(@"..\..\..\DAL\Utilities\TestData.sql");
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(sql);
            }
        }

        public void Down()
        {
            string sql = "DROP TABLE public.users; DROP TABLE public.usertable;";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Execute(sql);
            }
        }
    }
}