using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.Web.Models.Request.Script;

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
            
            LaunchInfo launchInfo = new LaunchInfo
            {
                FileType = request.FileType,
                PathToScriptFile = request.PathToDirectory
            };
            launchInfo.ParamInfos = new List<ParamInfo>();
            foreach (var parameter in request.Parameters)
            {
                launchInfo.ParamInfos.Add(new ParamInfo
                {
                    Name = parameter.Name,
                    Value = parameter.Value
                });
            }

            var scriptResult = await _scriptWorker.ExecuteScriptAsync(launchInfo);
            var script = ScriptSources.GetSingleScriptByFilePath(new DirectoryInfo(request.PathToDirectory).FullName);
            string fileName = Utilities.GenerateFileNameForResult(script.Name) + request.FileType.ToLower();
            string resultPath = $"Results\\{script.Provider}_Results";
            await _scriptWorker.ConvertResultAndSaveToFileAsync(scriptResult, resultPath, fileName,
                Utilities.GetFileExtension(request.FileType.ToLower()));
            return new JsonResult(JsonConvert.SerializeObject(new
            {
                SavedPath = Path.Combine(resultPath,fileName),
                FileName = fileName,
                FileType = request.FileType.ToLower()
            }));
        }

        [HttpGet]
        public async Task<List<string>> GetParams([FromQuery] string path)
        {
            return await Task.Run(() => _scriptWorker.GetParams(path));
        }

        [HttpGet]
        public async Task<ActionResult> Source([FromQuery] string src)
        {
            return await Task.Run(() => View("Source", System.IO.File.ReadAllText(
                ScriptSources.GetSingleScriptByFilePath(new DirectoryInfo(src).FullName).Path,
                Encoding.Default)));
        }
    }
}