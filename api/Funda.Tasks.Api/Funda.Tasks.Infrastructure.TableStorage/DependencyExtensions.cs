using Funda.Tasks.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddFundaTasksTableStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfig = new AzureSettings
            {
                StorageConectionString = configuration["AzureWebJobsStorage"]
            };
            services.AddSingleton(apiConfig);
            services.AddTransient<IUserTasksMapper, UserTasksMapper>();
            services.AddTransient<IUserTasksRepository, UserTasksRepository>();
            services.AddTransient<IDbContext<UserTasksEntity>, DbContext<UserTasksEntity>>(
                x => new DbContext<UserTasksEntity>(
                        x.GetService<AzureSettings>(),
                        x.GetService<ILogger<UserTasksEntity>>(),
                        UserTasksEntityDefinitions.TableName)
                );

            return services;
        }
    }
}
