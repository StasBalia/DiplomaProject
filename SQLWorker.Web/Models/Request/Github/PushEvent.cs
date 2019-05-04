using System.Collections.Generic;

namespace SQLWorker.Web.Models.Request.Github
{
    public class PushEvent
    {
        public string Ref { get; set; }
        public List<CommitDTO> Commits { get; set; }
        public Repository Repository { get; set; }
        
    }
}