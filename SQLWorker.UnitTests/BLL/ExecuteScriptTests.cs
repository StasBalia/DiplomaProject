using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console().CreateLogger();
            ILoggerFactory factory = new LoggerFactory();
            factory.AddSerilog(dispose: true);
            _repository = new PostgreSqlScriptRepository(DB_CONNECTION_STRING); //TODO: pls setup logger here 
            _scriptWorker = new ScriptWorker(_repository); // and here
        }

        [Fact]
        public async Task AlwaysValidTest()
        {
            await _scriptWorker.ExecuteScript(new LaunchInfo
            {
                PathToDirectory = @"E:\University\Diploma\DiplomaProject\Scripts\github\testScript.sql",
                ParamInfos = new List<ParamInfo>
                {
                    new ParamInfo
                    {
                        Name = "_id",
                        Value = "1"
                    }
                },
                FileType = "csv"
            });
        }
    }
}