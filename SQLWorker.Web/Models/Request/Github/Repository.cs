using Newtonsoft.Json;

namespace SQLWorker.Web.Models.Request.Github
{
    public class Repository
    {
        public string Name { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}