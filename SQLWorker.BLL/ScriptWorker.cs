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

            if (result.Tables.Count == 0)
            {
                _log.LogError("Немає таблиць в dataSet");
                return false;
            }

            return true;
        }

        public string ConvertToCsv(DataSet result)
        {       
            DataTable table = result.Tables[0];          
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", result.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName)) + "\n");
            foreach (DataRow row in table.Rows)
            {
                sb.Append(string.Join(",", row.ItemArray) + "\n");
            }

            return sb.ToString();
        }
        
        

        public List<string> GetParams(string src)
        {
            return new List<string>();
        }
    }
}