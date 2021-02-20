using System;
using Funda.Tasks.Api;
using Funda.Tasks.Infrastructure.TableStorage;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Funda.Tasks.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddEnvironmentVariables()
               .Build();

            builder.Services.AddFundaTasksTableStorage(config);
        }
    }
}
