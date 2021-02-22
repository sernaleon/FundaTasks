using Funda.Tasks.Core;
using Funda.Tasks.Infrastructure.TableStorage.Mappers;
using Funda.Tasks.Infrastructure.TableStorage.Repositories;
using Microsoft.Azure.Cosmos.Table;
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

            services.AddTransient<ITaskMapper, TaskMapper>();
            services.AddTransient<IUserTaskMapper, UserTaskMapper>();

            services.AddTransient<ITasks, TasksRepository>();
            services.AddTransient<IUserTasks, UserTasksRepository>();

            services.AddDbContext<TaskEntity>(TableNames.TasksTableName);
            services.AddDbContext<UserTaskEntity>(TableNames.UserTasksTableName);

            return services;
        }

        private static IServiceCollection AddDbContext<T>(this IServiceCollection services, string tablename) where T:TableEntity, new()
        {
            services.AddTransient<IDbContext<T>, DbContext<T>>(
                   x => new DbContext<T>(
                           x.GetService<AzureSettings>(),
                           x.GetService<ILogger<T>>(),
                           tablename)
                   );

            return services;
        }
    }
}
