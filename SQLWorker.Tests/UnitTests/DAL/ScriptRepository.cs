using Microsoft.Extensions.Logging;
using Serilog;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using Xunit;

namespace SQLWorker.Tests.UnitTests.DAL
{
    public class ScriptRepository
    {
        private readonly IScriptRepository _repository;
        public ScriptRepository()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console().CreateLogger();
            ILoggerFactory factory = new LoggerFactory();
            factory.AddSerilog(dispose: true);
            _repository = new PostgreSqlScriptRepository(factory.CreateLogger<PostgreSqlScriptRepository>(), "");
            //_scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        public void ExecuteScript_Valid()
        {
            _repository.ExecuteAndGetResult("");
        }
        
    }
}