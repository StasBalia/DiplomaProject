using System.Collections.Generic;

namespace SQLWorker.BLL.Models
{
    public class LaunchInfo
    {
        public string PathToScriptFile { get; set; }

        public List<ParamInfo> ParamInfos { get; set; }
        public string FileType { get; set; }
    }
}