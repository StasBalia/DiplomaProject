using System;
using System.Data;

namespace SQLWorker.DAL.Models
{
    public class ScriptResult
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Error { get; set; }
        public DataSet DataSet { get; set; }
    }
}