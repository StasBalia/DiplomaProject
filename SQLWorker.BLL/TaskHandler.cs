using System;
using System.Collections.Concurrent;
using System.Linq;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL
{
    public class TaskHandler
    {
        private const int MAX_COUNT_FOR_USER = 3;
        private const int MAX_TASKS_COUNT = 10;

        private readonly ConcurrentDictionary<Guid, TaskModel> _taskModels =
            new ConcurrentDictionary<Guid, TaskModel>();
        
        public TaskModel this[Guid i] => _taskModels[i];

        public int TasksCount => _taskModels.Count;

        public void AddTask(TaskModel model)
        {
            while (_taskModels.Count(x => x.Value.TaskState == TaskState.Started) >= MAX_TASKS_COUNT ||
                   _taskModels.Count(x => x.Value.User.Equals(model.User) && x.Value.TaskState == TaskState.Started) >=
                   MAX_COUNT_FOR_USER
            )
            {
            }

            _taskModels.AddOrUpdate(model.Id, model, (guid, taskModel) => model);
        }
    }
}