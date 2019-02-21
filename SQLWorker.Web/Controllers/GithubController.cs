using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

    public class PushEvent
    {
        public string Ref { get; set; }
        public List<Commit> Commits { get; set; }
        public Repository Repository { get; set; }
        
    }

    public class Commit
    {
        public string SHA { get; set; }
        public string Message { get; set; }
        public Author Author { get; set; }
        public string Url { get; set; }
        public bool Distinct { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Repository
    {
        public string Name { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }    
    }
    
}