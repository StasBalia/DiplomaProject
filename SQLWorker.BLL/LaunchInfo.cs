using System.Collections.Generic;

namespace SQLWorker.BLL
{
    public class LaunchInfo
    {
        public string PathToDirectory { get; set; }

        public List<ParamInfo> ParamInfos { get; set; }
        public string FileType { get; set; }
    }
}