using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using SQLWorker.BLL;
using Xunit;

namespace SQLWorker.Tests.UnitTests.BLL
{
    public class ExecuteScript
    {
        private ScriptWorker _scriptWorker;

        public ExecuteScript()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console().CreateLogger();
            ILoggerFactory factory = new LoggerFactory();
            factory.AddSerilog(dispose: true);
            _scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        public async Task AlwaysValidTest()
        {
            var result = await _scriptWorker.ExecuteScript(new LaunchInfo
            {
                PathToDirectory = @"E:\University\Diploma\DiplomaProject\github\testScript.sql",
                ParamInfos = new List<ParamInfo>
                {
                    new ParamInfo
                    {
                        Name = "id",
                        Value = "1"
                    }
                },
                FileType = "csv"
            });
        }
    }
}