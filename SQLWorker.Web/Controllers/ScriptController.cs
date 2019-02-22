using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.Web.Models.Request.Script;

namespace SQLWorker.Web.Controllers
{
    public class ScriptController : Controller
    {
        private readonly ILogger<ScriptController> _log;
        
        public ScriptController(ILogger<ScriptController> log)
        {
            _log = log;
        }

        [HttpPost]
        public IActionResult Launch([FromBody]LaunchInfo request)
        {
            return Ok();
        }
    }
}