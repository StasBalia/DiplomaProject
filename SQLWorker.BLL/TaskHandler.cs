using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL
{
    public static class TaskHandler
    {
        private const int MAX_COUNT_FOR_USER = 3;
        private const int MAX_TASKS_COUNT = 10;

        private static readonly ConcurrentDictionary<Guid, TaskModel> _taskModels;

        static TaskHandler()
        {
            _taskModels = new ConcurrentDictionary<Guid, TaskModel>();
        }
        public static int TasksCount => _taskModels.Count;

        public static void AddTask(TaskModel model)
        {
            while (_taskModels.Count(x => x.Value.TaskState == TaskState.Started) >= MAX_TASKS_COUNT ||
                   _taskModels.Count(x => x.Value.User.Equals(model.User) && x.Value.TaskState == TaskState.Started) >=
                   MAX_COUNT_FOR_USER
            )
            {
            }

            _taskModels.AddOrUpdate(model.Id, model, (guid, taskModel) => model);
        }

        public static List<TaskModel> GetAllTasks() => _taskModels.Values.ToList();
    }
}