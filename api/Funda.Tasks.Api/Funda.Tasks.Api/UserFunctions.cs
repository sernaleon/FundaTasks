using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using Funda.Tasks.Core;

namespace Funda.Tasks.Api
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _log;
        private readonly IUser _user;
        public UserFunctions(ILogger<UserFunctions> log, IUser userTasks)
        {
            _log = log;
            _user = userTasks;
        }

        [FunctionName("UsersGet")]
        public async Task<IActionResult> UsersGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req,
            CancellationToken token = default)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var result = await _user.GetAllUsersAsync(token);

            return new JsonResult(result);
        }

        [FunctionName("UserAdd")]
        public async Task<IActionResult> UserAdd(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req,
            CancellationToken token = default)
        {
            var user = await req.GetBodyAsync<User>();

            await _user.AddUserAsync(user, token);

            return new JsonResult(user);
        }
    }
}
