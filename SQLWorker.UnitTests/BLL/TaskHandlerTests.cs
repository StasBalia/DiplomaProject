using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using Xunit;
using Xunit.Abstractions;

namespace SQLWorker.UnitTests.BLL
{
    public class TaskHandlerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TaskHandlerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void AddTask_AddedTask_Valid()
        {
            TaskModel model = new TaskModel
            {
                Id = Guid.NewGuid(),
                ScriptSource = new ScriptInfo(),
                User = "User1",
                TaskState = TaskState.Started,
                ResultFileExtension = FileExtension.csv
            };

            var taskId = model.Id;
            TaskHandler.AddTask(model);
            var task = TaskHandler.GetAllTasks(x => x.Id.Equals(taskId)).FirstOrDefault();
            task.Should().BeEquivalentTo(model);
        }
    }
}