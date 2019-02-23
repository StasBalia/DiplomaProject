using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using SQLWorker.DAL.Repositories.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SQLWorker.BLL
{
    public class ScriptWorker
    {
        private readonly ILogger _log;
        private readonly IScriptRepository _repository;
        
        public ScriptWorker(ILogger log, IScriptRepository repository)
        {
            _log = log;
            _repository = repository;
        }


        public async Task<DataSet> ExecuteScript(LaunchInfo launchInfo)
        {
            //_log.LogInformation("Start executing script.");

            var scriptFile = File.ReadAllLines(launchInfo.PathToScriptFile);

            string script = string.Join("", scriptFile);
            foreach (var paramInfo in launchInfo.ParamInfos)
            {
                script = script.Replace(paramInfo.Name, paramInfo.Value);
            }
            
            return await Task.FromResult(_repository.ExecuteAndGetResult(script));
        }

        public bool CheckForSucces(DataSet result)
        {
            if (result == null)
            {
                _log.LogError("Не отримали dataSet");
                return false;
            }

            if (result.Tables.Count != 0) return true;
            _log.LogError("Немає таблиць в dataSet");
            return false;

        }

        public List<string> GetParams(string src)
        {
            return new List<string>();
        }
    }
}