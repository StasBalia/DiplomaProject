using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using Xunit;

namespace SQLWorker.UnitTests.DAL
{
    public class ScriptRepositoryTests
    {
        private readonly IScriptRepository _repository;
        private const string DB_CONNECTION_STRING =
            "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=test";
        public ScriptRepositoryTests()
        {
            ILoggerFactory factory = new LoggerFactory();
            _repository = new PostgreSqlScriptRepository(DB_CONNECTION_STRING, factory.CreateLogger<PostgreSqlScriptRepository>());
            //_scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        public void ExecuteScript_Valid()
        {
            var res = _repository.ExecuteAndGetResult("SELECT * FROM public.usertable WHERE id = 1");
            Assert.NotNull(res);
        }
    }
}