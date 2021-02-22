using Funda.Tasks.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Logic
{
    public interface INewTaskHandler
    {
        Task<IEnumerable<TaskLineItem>> AddTaskAsync(Guid userId, NewTask newTask, CancellationToken token);
    }

    public class NewTaskHandler : INewTaskHandler
    {
        private readonly IUserTasks _userTasks;
        private readonly ITasks _tasks;

        public NewTaskHandler(IUserTasks userTasks, ITasks tasks)
        {
            _userTasks = userTasks;
            _tasks = tasks;
        }

        public async Task<IEnumerable<TaskLineItem>> AddTaskAsync(Guid userId, NewTask newTask, CancellationToken token)
        {
            var taskLine = new TaskLineItem
            {
                Id = Guid.NewGuid(),
                Task = new TaskType
                {
                    Id = newTask.TaskId ?? Guid.NewGuid(),
                    Name = newTask.Name
                },
                Description = newTask.Description,
                Timestamp = DateTimeOffset.Now
            };
            await _tasks.AddTaskAsync(userId, taskLine.Task, token);
            await _userTasks.SetUserTasksAsync(userId, taskLine, token);
            var result = await _userTasks.GetUserTasksAsync(userId, token);
            return result;
        }
    }
}
