using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.Web.Models.Request.Github;

namespace SQLWorker.Web.Controllers
{
    public class GithubController : Controller
    {
        private readonly ILogger<GithubController> _log;

        public GithubController(ILogger<GithubController> log)
        {
            _log = log;
        }

        [HttpPost]
        public IActionResult Payload([FromBody]PushEvent pushEvent)
        {
            _log.LogInformation("JSON from webhook: {obj}", pushEvent);
            return Ok();
        }
    }
}