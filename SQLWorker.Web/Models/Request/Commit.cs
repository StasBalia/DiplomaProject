using System;
using Newtonsoft.Json;
using SQLWorker.Web.Controllers;

namespace SQLWorker.Web.Models.Request
{
    public class Commit
    {
        public string SHA { get; set; }
        public string Message { get; set; }
        public Author Author { get; set; }
        public string Url { get; set; }
        public bool Distinct { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}