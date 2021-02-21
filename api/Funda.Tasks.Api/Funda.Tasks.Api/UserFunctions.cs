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
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _log;
        private readonly IUser _user;
        public UserFunctions(ILogger<UserFunctions> log, IUser userTasks)
        {
            _log = log;
            _user = userTasks;
        }

        [FunctionName("GetUsers")]
        public async Task<IActionResult> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req,
            CancellationToken token = default)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var result = await _user.GetAllUsersAsync(token);

            return new JsonResult(result);
        }


        [FunctionName("AddUser")]
        public async Task<IActionResult> AddUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/add")] HttpRequest req,
            CancellationToken token = default)
        {
            var user = new User
            {
                Id = new Guid("c54033a9-3dc3-472b-9008-cd4a4cae2a06"),
                Name = "Arturi"
            };

            await _user.AddUserAsync(user, token);

            return new JsonResult(user);
        }



        //private static async Task NewMethod(HttpRequest req)
        //{
        //    string name = req.Query["userId"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;
        //}
    }
}
