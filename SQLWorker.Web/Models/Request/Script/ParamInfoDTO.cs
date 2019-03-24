using Newtonsoft.Json;

namespace SQLWorker.Web.Models.Request.Script
{
    public class ParamInfoDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}