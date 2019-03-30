using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.Web.Models.Request.Script;
using SQLWorker.Web.Models.Response;

namespace SQLWorker.Web.Controllers
{
    public class ScriptController : Controller
    {
        private readonly ILogger<ScriptController> _log;
        private readonly ScriptWorker _scriptWorker;
        public ScriptController(ILogger<ScriptController> log, IScriptRepository repository)
        {
            _log = log;
            _scriptWorker = new ScriptWorker(log, repository);
        }

        [HttpPost]
        public async Task<IActionResult> Launch([FromBody]LaunchInfoDTO request)
        {
            //TODO: заюзай якийсь автомапер.
            List<ParamInfoDTO> parameters = JsonConvert.DeserializeObject<List<ParamInfoDTO>>(request.Parameters);
            LaunchInfo launchInfo = new LaunchInfo
            {
                FileType = request.FileType,
                PathToScriptFile = request.PathToDirectory
            };
            launchInfo.ParamInfos = new List<ParamInfo>();
            foreach (var parameter in parameters)
            {
                launchInfo.ParamInfos.Add(new ParamInfo
                {
                    Name = parameter.Name,
                    Value = parameter.Value
                });
            }
            
            var script = ScriptSources.GetSingleScriptByFilePath(new DirectoryInfo(request.PathToDirectory).FullName);
            var fileExtension = Utilities.GetFileExtension(request.FileType.ToLower());
            TaskModel taskModel = new TaskModel
            {
                Id = Guid.NewGuid(),
                User = "",
                TaskState = TaskState.Queued,
                ScriptSource = script,
                ScriptParameters = launchInfo.ParamInfos?.Select(x => x.Value).ToArray(),
                ResultFileExtension = fileExtension
            };
            TaskHandler.AddTask(taskModel);
            
            taskModel.TaskState = TaskState.Started;
            
            var scriptResult = await _scriptWorker.ExecuteScriptAsync(launchInfo, taskModel);

            if (scriptResult == null)
            {
                taskModel.TaskState = TaskState.Error;
                return new EmptyResult();
            }
            
            string fileName = Utilities.GenerateFileNameForResult(script.Name) + request.FileType.ToLower();
            string resultPath = $"Results\\{script.Provider}_Results\\";
            await _scriptWorker.ConvertResultAndSaveToFileAsync(scriptResult, resultPath, fileName,
                fileExtension);
            taskModel.TaskState = TaskState.Success;
            taskModel.DownloadPath = resultPath;
            taskModel.DownloadName = fileName;
            return new JsonResult(JsonConvert.SerializeObject(new DownloadInfo
            {
                SavedPath = Path.Combine(resultPath,fileName),
                FileName = fileName,
                FileType = request.FileType.ToLower()
            }));
        }

        [HttpGet]
        public async Task<List<string>> GetParams([FromQuery] string path)
        {
            return await Task.Run(() =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return _scriptWorker.GetParams(directoryInfo.FullName);
            });
        }

        [HttpGet]
        public async Task<ActionResult> Source([FromQuery] string src)
        {
            return await Task.Run(() => View("Source", System.IO.File.ReadAllText(
                ScriptSources.GetSingleScriptByFilePath(new DirectoryInfo(src).FullName).Path,
                Encoding.Default)));
        }

        [HttpGet]
        public async Task<IActionResult> Download([FromQuery] DownloadInfoDTO data)
        {
            return await ConvertResultToActionResultAsync(data, Utilities.GetFileExtension(data.FileType));
        }

        [HttpGet]
        public async Task<List<TaskViewModel>> GetTasksForUser() //TODO: In future, pls do validation for user.
        {
            return await Task.Run(() =>
            {
                var allTasks = TaskHandler.GetAllTasks();
                List<TaskViewModel> taskViewModels = new List<TaskViewModel>();
                foreach (var task in allTasks)
                {
                    taskViewModels.Add(new TaskViewModel //TODO: use automapper
                    {
                        Id = task.Id,
                        User = task.User,
                        Errors = task.Errors,
                        EndTime = task.EndTime,
                        StartTime = task.StartTime,
                        TaskState = task.TaskState.ToString(),
                        DownloadName = task.DownloadName,
                        DownloadPath = task.DownloadPath,
                        ScriptParameters = task.ScriptParameters,
                        ResultFileExtension = task.ResultFileExtension.ToString(),
                        ScriptSource = task.ScriptSource
                    });
                }
                return taskViewModels;
            }); 
        }


        public async Task<IActionResult> ConvertResultToActionResultAsync(DownloadInfoDTO data, FileExtension fileExtension)
        {
            if(string.IsNullOrEmpty(data.SavedPath))
                return new EmptyResult();
            switch (fileExtension)
            {
                case FileExtension.xlsx:
                case FileExtension.csv:
                {
                    var content = new FileStream(data.SavedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    return File(content, "application/octet-stream", data.FileName);
                }
                case FileExtension.xml:
                {
                    return Content(await System.IO.File.ReadAllTextAsync(data.SavedPath),
                        MediaTypeHeaderValue.Parse("application/xml"));
                }
                case FileExtension.json:
                {
                    return Content(await System.IO.File.ReadAllTextAsync(data.SavedPath),
                        MediaTypeHeaderValue.Parse("application/json"));
                }
                default: return new EmptyResult();
            }
        }
    }
}