using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Models;
using SQLWorker.DAL.Repositories.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using JsonConverter = SQLWorker.BLL.ScriptConverters.JsonConverter;

namespace SQLWorker.BLL
{
    public class ScriptWorker
    {
        private readonly ILogger _log;
        private readonly IScriptRepository _repository;
        private readonly Dictionary<FileExtension, IScriptSaver> _savers;
        private const string PATH_TO_REPO = @"..\..\Repos\";
        private const string PATH_TO_SAVE = @"..\SQLWorker.Web\Scripts\";
        
        public ScriptWorker(ILogger<ScriptWorker> log, IScriptRepository repository)
        {
            _log = log;
            _repository = repository;
            _savers = new Dictionary<FileExtension, IScriptSaver>(new List<KeyValuePair<FileExtension, IScriptSaver>>
            {
                new KeyValuePair<FileExtension, IScriptSaver>(FileExtension.csv, new CsvSaver()),
                new KeyValuePair<FileExtension, IScriptSaver>(FileExtension.xml, new XmlSaver()),
                new KeyValuePair<FileExtension, IScriptSaver>(FileExtension.xlsx, new XlsSaver()),
                new KeyValuePair<FileExtension, IScriptSaver>(FileExtension.json, new JsonSaver()),
            });
        }


        public async Task<DataSet> ExecuteScriptAsync(LaunchInfo launchInfo, TaskModel taskModel)
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
            
                ScriptResult result = await Task.FromResult(_repository.ExecuteAndGetResult(script));
                taskModel.StartTime = result.Start;
                taskModel.EndTime = result.End;
                taskModel.Errors = result.Error;
                return result.DataSet;
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

        public async Task ConvertResultAndSaveToFileAsync(DataSet ds, string pathToSave, string fileName, FileExtension fileExtension) =>
            await _savers[fileExtension].SaveAsync(ds, pathToSave, fileName);


        public async Task<bool> CopyScripts(ScriptProvider provider, string repositoryName, string[] modifiedFiles)
        {
            try
            {
                if (string.IsNullOrEmpty(repositoryName))
                    return await Task.FromResult(false); //TODO: need to write error to log
                if(modifiedFiles == null || modifiedFiles.Length == 0)   
                    return await Task.FromResult(false); //TODO: need to write error to log

                string pathTo = Utilities.GetFullPath(PATH_TO_SAVE,  $@"{provider.ToString().ToLower()}\" + repositoryName);
                string pathFrom = Utilities.GetFullPath(PATH_TO_REPO, repositoryName);
                
                foreach (var file in modifiedFiles)
                {
                    var fileFrom = Directory.GetFiles(pathFrom, file, SearchOption.AllDirectories).FirstOrDefault(x => x.Contains(file));
                    string content = await File.ReadAllTextAsync(fileFrom);
                    string fileTo = Path.Combine(pathTo, file);
                    if (File.Exists(fileTo))
                        await File.AppendAllTextAsync(fileTo, content);
                    else
                    {
                        Directory.CreateDirectory(pathTo);
                        using (var wr = File.Create(fileTo))
                        {
                            await wr.WriteAsync(Encoding.UTF8.GetBytes(content));
                        }
                    }
                }
            
                return await Task.FromResult(false);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }
    }
}