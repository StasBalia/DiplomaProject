using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using JsonConverter = SQLWorker.BLL.ScriptConverters.JsonConverter;

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


        public async Task<DataSet> ExecuteScriptAsync(LaunchInfo launchInfo)
        {
            //_log.LogInformation("Start executing script.");
            try
            {
                var scriptFile = File.ReadAllLines(launchInfo.PathToScriptFile);

                string script = string.Join("", scriptFile);
                foreach (var paramInfo in launchInfo.ParamInfos)
                {
                    script = script.Replace(paramInfo.Name, paramInfo.Value);
                }
            
                return await Task.FromResult(_repository.ExecuteAndGetResult(script));
            }
            catch (Exception e)
            {
                _log.LogError(e.Message,e);
                return new DataSet();
            }
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
            return ScriptSources.GetSingleScriptByFilePath(src).Parameters;
        }

        public async Task ConvertResultAndSaveToFileAsync(DataSet ds, string pathToSave, string fileName, FileExtension fileExtension)
        {
            switch (fileExtension)
            {
                   case FileExtension.csv:
                        var converter = new CsvConverter();
                        var convertedResult = converter.ConvertToRightFormat(ds);
                        var saver = new CsvSaver();
                        await saver.SaveAsync(convertedResult, pathToSave, fileName);
                        break;
                   case FileExtension.xml:
                       var xmlConverter = new XmlConverter();
                       var xmlConvertedResult = xmlConverter.ConvertToRightFormat(ds);
                       var xmlSaver = new XmlSaver();
                       await xmlSaver.SaveAsync(xmlConvertedResult, pathToSave, fileName);
                       break;
                   case FileExtension.xlsx:
                       var xlsxConverter = new XlsConverter();
                       var xlsConvertedResult = xlsxConverter.ConvertToRightFormat(ds);
                       var xlsSaver = new XlsSaver();
                       await xlsSaver.SaveAsync(xlsConvertedResult, pathToSave, fileName);
                       break;
                   case FileExtension.json:
                       var jsonConverter = new JsonConverter();
                       var jsonConvertedResult = jsonConverter.ConvertToRightFormat(ds);
                       var jsonSaver = new JsonSaver();
                       await jsonSaver.SaveAsync(jsonConvertedResult, pathToSave, fileName);
                       break;
                   default:
                        return;
                    
            }
        }
    }
}