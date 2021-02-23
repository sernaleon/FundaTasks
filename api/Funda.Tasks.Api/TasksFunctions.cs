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
using Funda.Tasks.Logic;

namespace Funda.Tasks.Api
{
    public class TasksFunctions
    {
        private readonly ILogger<TasksFunctions> _log;
        private readonly IUserTasks _userTasks;
        private readonly INewTaskHandler _addTaskHandler;
        private readonly IApiAuthentication _apiAuthentication;

        public TasksFunctions(ILogger<TasksFunctions> log, IUserTasks userTasks, IApiAuthentication apiAuthentication, INewTaskHandler addTaskHandler)
        {
            _log = log;
            _userTasks = userTasks;
            _apiAuthentication = apiAuthentication;
            _addTaskHandler = addTaskHandler;
        }

        [FunctionName("TasksGet")]
        public async Task<IActionResult> TasksGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            try
            {
                var authResult = await _apiAuthentication.AuthenticateAsync(req.Headers);
                if (authResult.Failed) return new JsonResult(authResult) { StatusCode = StatusCodes.Status401Unauthorized };

                var userId = GetUserId(authResult.User);

                var result = await _userTasks.GetUserTasksAsync(userId, token);

                return new JsonResult(result);
            } 
            catch (Exception e)
            {
                _log.LogError(e.Message, e);

                return new JsonResult(e) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        [FunctionName("TasksAdd")]
        public async Task<IActionResult> TasksAdd(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            try
            {
                var authResult = await _apiAuthentication.AuthenticateAsync(req.Headers);
                if (authResult.Failed) return new UnauthorizedResult();

                var userId = GetUserId(authResult.User);
                var newTask = await req.GetBodyAsync<NewTask>();

                var result = await _addTaskHandler.AddTaskAsync(userId, newTask, token);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message, e);

                return new StatusCodeResult(500);
            }
        }


        private Guid GetUserId(ClaimsPrincipal user)
        {
            return new Guid(user.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
