using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using Funda.Tasks.Core;
using System;
using AzureFunctions.OidcAuthentication;
using System.Security.Claims;
using System.Linq;
using Funda.Tasks.Core.Models;

namespace Funda.Tasks.Api
{
    public class TasksFunctions
    {
        private readonly ILogger<TasksFunctions> _log;
        private readonly ITaskRepository _task;
        private readonly IApiAuthentication _apiAuthentication;

        public TasksFunctions(ILogger<TasksFunctions> log, ITaskRepository task, IApiAuthentication apiAuthentication)
        {
            _log = log;
            _task = task;
            _apiAuthentication = apiAuthentication;
        }

        [FunctionName("TasksGet")]
        public async Task<IActionResult> TasksGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            return await ExecuteTaskFunctionAsync(req, token);
        }

        [FunctionName("TasksAdd")]
        public async Task<IActionResult> TasksAdd(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            return await ExecuteTaskFunctionAsync(req, token, async (Guid userId) =>
            {
                var newTask = await req.GetBodyAsync<NewTaskModel>();
                await _task.SetAsync(userId, newTask, token);
            });
        }

        [FunctionName("TasksUpdate")]
        public async Task<IActionResult> TasksUpdate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            return await ExecuteTaskFunctionAsync(req, token, async (Guid userId) =>
            {
                var updateTask = await req.GetBodyAsync<UpdateTaskModel>();
                await _task.SetAsync(userId, updateTask, token);
            });
        }

        [FunctionName("TasksDelete")]
        public async Task<IActionResult> TasksDelete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            return await ExecuteTaskFunctionAsync(req, token, async (Guid userId) =>
            {
                var taskId = await req.GetBodyAsync<Guid>();
                await _task.DeleteAsync(userId, taskId, token);
            });
        }


        private async Task<JsonResult> ExecuteTaskFunctionAsync(HttpRequest req, CancellationToken token, Func<Guid, Task> actionAsync = null)
        {
            try
            {
                var authResult = await _apiAuthentication.AuthenticateAsync(req.Headers);
                if (authResult.Failed) return new JsonResult(authResult) { StatusCode = StatusCodes.Status401Unauthorized };

                var userId = new Guid(authResult.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);

                if (actionAsync != null)
                {
                    await actionAsync(userId);
                }

                var result = await _task.GetAllAsync(userId, token);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e);

                return new JsonResult(e) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
