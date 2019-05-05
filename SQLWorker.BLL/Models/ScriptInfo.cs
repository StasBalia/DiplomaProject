using System.Collections.Generic;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL.Models
{
    public class ScriptInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ScriptProvider Provider { get; set; }
        public List<string> Parameters { get; set; }
    }
}