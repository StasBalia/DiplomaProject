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