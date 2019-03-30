using System;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public ScriptInfo ScriptSource { get; set; }
        public FileExtension ResultFileExtension { get; set; }
        public TaskState TaskState { get; set; }
        public string DownloadPath { get; set; }
        public string DownloadName { get; set; }
        public string Errors { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan TimeInSeconds => EndTime - StartTime;
        public string[] ScriptParameters { get; set; }
    }
}