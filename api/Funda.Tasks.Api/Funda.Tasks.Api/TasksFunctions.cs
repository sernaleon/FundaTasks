using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using Funda.Tasks.Core;
using System;

namespace Funda.Tasks.Api
{
    public class TasksFunctions
    {
        private readonly ILogger<TasksFunctions> _log;
        private readonly IUserTasks _userTasks;
        private readonly ITasks _tasks;

        public TasksFunctions(ILogger<TasksFunctions> log, IUserTasks userTasks, ITasks tasks)
        {
            _log = log;
            _userTasks = userTasks;
            _tasks = tasks;
        }

        [FunctionName("GetTasks")]
        public async Task<IActionResult> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var userId = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a06");

            var result = await _userTasks.GetUserTasksAsync(userId, token);

            return new JsonResult(result);
        }


        [FunctionName("Generate")]
        public async Task<IActionResult> Generate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/generate")] HttpRequest req,
            CancellationToken token = default)
        {
            var userId = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a06");

            var task = new TaskType
            {
                Id = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a07"),
                Name = "Hacer cosas"
            };

            var taskLine = new TaskLineItem
            {
                Id = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a08"),
                Task = task,
                Description = "Hoy he hecho caca. Mucha."
            };

            await _tasks.AddTaskAsync(userId, task, token);
            await _userTasks.SetUserTasksAsync(userId, taskLine, token);

            return new JsonResult(taskLine);
        }
    }
}
