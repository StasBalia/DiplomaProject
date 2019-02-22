using Newtonsoft.Json;

namespace SQLWorker.Web.Models.Request
{
    public class Repository
    {
        public string Name { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }    
    }
}