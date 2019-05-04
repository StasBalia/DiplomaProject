using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ProvidersRepositories.Github;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.Web.Models.Request.Github;

namespace SQLWorker.Web.Controllers
{
    public class GithubController : Controller
    {
        private readonly ILogger<GithubController> _log;
        private readonly GithubPuller _puller;
        private readonly ScriptLoader _scriptLoader;
        private readonly ScriptUpdater _scriptUpdater;

        public GithubController(ILogger<GithubController> log)
        {
            _log = log;
            _puller = new GithubPuller();
            _scriptLoader = new ScriptLoader();
            _scriptUpdater = new ScriptUpdater();
        }

        [HttpPost]
        public async Task<IActionResult> Payload([FromBody]PushEvent pushEvent)
        {
            string repoName = pushEvent.Repository.Name + "\\";
            _log.LogInformation("JSON from webhook: {obj}", pushEvent);
            await _puller.PullFromRepo(repoName);
            ScriptSources.RemoveAll();
            await _scriptLoader.LoadScriptsAsync("Scripts/");
            await _scriptUpdater.CreateOrCopyScriptsAsync(ScriptProvider.Github, repoName, pushEvent.Commits.FirstOrDefault()?.Modified?.ToArray()); //TODO: need to check all of Added, Modifed, Deleted
            return Ok();
        }
    }
}