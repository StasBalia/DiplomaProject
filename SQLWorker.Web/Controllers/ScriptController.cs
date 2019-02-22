using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL;
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
        public IActionResult Launch([FromBody]LaunchInfoDTO request)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<List<string>> GetParams([FromQuery] string path)
        {
            return await Task.Run(() => _scriptWorker.GetParams(path));
        }
    }
}