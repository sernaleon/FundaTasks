using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using Funda.Tasks.Core;
using System.Collections.Generic;
using System;

namespace Funda.Tasks.Api
{
    public class Tasks
    {
        private readonly ILogger<Tasks> _log;
        private readonly IUserTasksRepository _repo;

        public Tasks(ILogger<Tasks> log, IUserTasksRepository repo)
        {
            _log = log;
            _repo = repo;
        }

        [FunctionName("GetTasks")]
        public async Task<IActionResult> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            CancellationToken token = default)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var result = await _repo.GetAllUserTasksAsync(token);

            return new JsonResult(result);
        }


        [FunctionName("Generate")]
        public async Task<IActionResult> Generate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/generate")] HttpRequest req,
            CancellationToken token = default)
        {
            var userTasks = new UserTasks
            {
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Arturi"
                },
                Tasks = new List<TaskLine>
                {
                    new TaskLine
                    {
                        TaskType = new TaskType
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sacar el perro"
                        },
                        Description = "this",
                        Timestamp = DateTimeOffset.Now
                    }
                }
            };

            await _repo.SetUserTasksAsync(userTasks, token);

            return new JsonResult(userTasks);
        }
    }
}
