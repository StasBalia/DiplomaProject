using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SQLWorker.Web.Models.Request.Script
{
    public class LaunchInfoDTO
    {
        [JsonProperty("path")]
        public string PathToDirectory { get; set; }
        
        [JsonProperty("params")]
        public List<ParamInfoDTO> Parameters { get; set; }
        [JsonProperty("ext")]
        public string FileType { get; set; }
    }
}