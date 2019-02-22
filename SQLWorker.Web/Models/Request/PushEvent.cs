using System.Collections.Generic;
using SQLWorker.Web.Controllers;

namespace SQLWorker.Web.Models.Request
{
    public class PushEvent
    {
        public string Ref { get; set; }
        public List<Commit> Commits { get; set; }
        public Repository Repository { get; set; }
        
    }
}