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

        [FunctionName("TasksGet")]
        public async Task<IActionResult> TasksGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            try
            {
                _log.LogInformation("C# HTTP trigger function processed a request.");

                var userId = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a06");

                var result = await _userTasks.GetUserTasksAsync(userId, token);

                return new JsonResult(result);
            } 
            catch (Exception e)
            {
                return new JsonResult(e);
            }
        }


        [FunctionName("TasksAdd")]
        public async Task<IActionResult> TasksAdd(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var userId = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a06");

            var taskLine = await req.GetBodyAsync<TaskLineItem>();

            await _tasks.AddTaskAsync(userId, taskLine.Task, token);
            await _userTasks.SetUserTasksAsync(userId, taskLine, token);

            return new OkResult();
        }
    }
}
