using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
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
            await _puller.PullFromRepoAsync(repoName);
            ScriptSources.RemoveAll();
            
            List<Commit> commits = new List<Commit>();
            foreach (var commit in pushEvent.Commits)
            {
                commits.Add(new Commit
                {
                    Url = commit.Url,
                    Added = commit.Added,
                    Message = commit.Message,
                    Removed = commit.Removed,
                    Author = new Author
                    {
                        Name = commit.AuthorDto.Name,
                        Email = commit.AuthorDto.Email
                    },
                    Distinct = commit.Distinct,
                    Modified = commit.Modified,
                    TimeStamp = commit.TimeStamp,
                    SHA = commit.SHA
                });
            }
            
            await _scriptUpdater.StartUpdateScriptsAsync(repoName, commits);
            
            await _scriptLoader.LoadScriptsAsync("Scripts/");
            return Ok();
        }
    }
}