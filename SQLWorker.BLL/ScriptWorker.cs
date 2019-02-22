using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLWorker.DAL.Repositories.Interfaces;

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

        public ScriptWorker(IScriptRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteScript(LaunchInfo launchInfo)
        {
            //_log.LogInformation("Start executing script.");

            var scriptFile = File.ReadAllLines(launchInfo.PathToDirectory);

            string script = string.Join("", scriptFile);
            foreach (var paramInfo in launchInfo.ParamInfos)
            {
                script = script.Replace(paramInfo.Name, paramInfo.Value);
            }
            
            var result = await Task.FromResult(_repository.ExecuteAndGetResult(script));
            if (result == null)
            {
                _log.LogError("Не отримали dataSet");
                return;
            }

            if (result.Tables.Count == 0)
            {
                _log.LogError("Немає таблиць в dataSet");
                return;
            }
                
            DataTable table = result.Tables[0];          
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", result.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName)) + "\n");
            foreach (DataRow row in table.Rows)
            {
                sb.Append(string.Join(",", row.ItemArray) + "\n");
            }
        }

        public List<string> GetParams(string src)
        {
            return new List<string>();
        }
    }
}