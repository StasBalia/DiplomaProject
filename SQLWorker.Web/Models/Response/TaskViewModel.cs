using System;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.Web.Models.Response
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public ScriptInfo ScriptSource { get; set; }
        public string ResultFileExtension { get; set; }
        public string TaskState { get; set; }
        public string DownloadPath { get; set; }
        public string DownloadName { get; set; }
        public string Errors { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan TimeInSeconds => EndTime - StartTime;
        public string[] ScriptParameters { get; set; }
    }
}