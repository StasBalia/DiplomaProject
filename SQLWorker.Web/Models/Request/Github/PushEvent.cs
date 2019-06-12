using System.Collections.Generic;
using Newtonsoft.Json;

namespace SQLWorker.Web.Models.Request.Github
{
    public class PushEvent
    {
        public string Ref { get; set; }
        public List<CommitDTO> Commits { get; set; }
        public Repository Repository { get; set; }
    }
}