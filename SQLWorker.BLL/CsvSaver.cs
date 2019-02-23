using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SQLWorker.BLL
{
    public class CsvSaver : IScriptSaver<string>
    {
        private readonly ILogger _log;

        public CsvSaver()
        {
            
        }

        public CsvSaver(ILogger<CsvSaver> log)
        {
            _log = log;
        }
        public string ConvertToRightFormat(DataSet result)
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

        public async Task SaveAsync(string objectToSave, string pathToSave, string fileName)
        {
            try
            {
                string fullPath = Path.Combine(pathToSave, fileName);
                using (var writer = File.CreateText(fullPath))
                {
                    await writer.WriteAsync(objectToSave);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e);
            }
        }
    }
}