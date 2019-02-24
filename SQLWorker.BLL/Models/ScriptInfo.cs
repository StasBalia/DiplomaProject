using System.Collections.Generic;

namespace SQLWorker.BLL.Models
{
    public class ScriptInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Provider { get; set; }
        public List<string> Parameters { get; set; }
    }
}