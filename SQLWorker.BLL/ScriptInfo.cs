using System.Collections.Generic;

namespace SQLWorker.BLL
{
    public class ScriptInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string VCS { get; set; }
        public List<string> Parameters { get; set; }
    }
}