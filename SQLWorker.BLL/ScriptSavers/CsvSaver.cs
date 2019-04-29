using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;

namespace SQLWorker.BLL.ScriptSavers
{
    public class CsvSaver : IScriptSaver
    {
        private readonly ILogger _log;
        private readonly CsvConverter _converter;

        public CsvSaver()
        {
            _converter = new CsvConverter();
        }

        public CsvSaver(ILogger<CsvSaver> log)
        {
            _log = log;
        }

        public async Task SaveAsync(DataSet dataSet, string pathToSave, string fileName)
        {
            try
            {
                string fullPath = Path.Combine(pathToSave, fileName);
                using (var writer = File.CreateText(fullPath))
                {
                    await writer.WriteAsync(_converter.ConvertToRightFormat(dataSet));
                }
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e);
            }
        }
    }
}