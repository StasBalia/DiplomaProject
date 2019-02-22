using System.Collections.Generic;

namespace SQLWorker.Web.Models.Request.Script
{
    public class LaunchInfo
    {
        public string PathToDirectory { get; set; }
        public List<ParamInfo> Parameters { get; set; }
        public string FileType { get; set; }
    }
}