using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SQLWorker.BLL;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class ExecuteScriptTests
    {
        private readonly ScriptWorker _scriptWorker;
        private readonly IScriptRepository _repository;

        private const string DB_CONNECTION_STRING =
            "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=test";

        public ExecuteScriptTests()
        {

            ILoggerFactory factory = new LoggerFactory();
            ILogger<PostgreSqlScriptRepository> log = factory.CreateLogger<PostgreSqlScriptRepository>();
            
            _repository = new PostgreSqlScriptRepository(DB_CONNECTION_STRING, log); 
            _scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>(), _repository);
        }

        [Fact]
        public async Task AlwaysValidTest()
        {
            await _scriptWorker.ExecuteScript(new LaunchInfo
            {
                PathToScriptFile = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\testScript.sql",
                ParamInfos = new List<ParamInfo>
                {
                    new ParamInfo
                    {
                        Name = "{id}",
                        Value = "1"
                    }
                },
                FileType = "csv"
            });
        }
    }
}