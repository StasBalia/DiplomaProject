using System.Collections.Generic;

namespace SQLWorker.Web.Models.Request.Script
{
    public class LaunchInfoDTO
    {
        public string PathToDirectory { get; set; }
        public List<ParamInfoDTO> Parameters { get; set; }
        public string FileType { get; set; }
    }
}