using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SQLWorker.Web.Models.Request.Github
{
    public class CommitDTO
    {
        public string Message { get; set; }
        [JsonProperty("author")]
        public AuthorDTO AuthorDto { get; set; }
        public string Url { get; set; }
        public bool Distinct { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
        
        public List<string> Added { get; set; }
        public List<string> Removed { get; set; }
        public List<string> Modified { get; set; }
    }
}