using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL.ProvidersRepositories.Github;
using SQLWorker.Web.Models.Request.Github;

namespace SQLWorker.Web.Controllers
{
    public class GithubController : Controller
    {
        private readonly ILogger<GithubController> _log;
        private readonly GithubPuller _puller;

        public GithubController(ILogger<GithubController> log)
        {
            _log = log;
            _puller = new GithubPuller();
        }

        [HttpPost]
        public async Task<IActionResult> Payload([FromBody]PushEvent pushEvent)
        {
            _log.LogInformation("JSON from webhook: {obj}", pushEvent);
            await _puller.PullFromRepo(pushEvent.Repository.Name + "\\");
            return Ok();
        }
    }
}