using System;
using AzureFunctions.OidcAuthentication;
using Funda.Tasks.Api;
using Funda.Tasks.Infrastructure.TableStorage;
using Funda.Tasks.Logic;
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

            builder.Services.AddOidcApiAuthorization();
            builder.Services.AddFundaTasksLogic();
            builder.Services.AddFundaTasksTableStorage(config);
        }
    }
}
