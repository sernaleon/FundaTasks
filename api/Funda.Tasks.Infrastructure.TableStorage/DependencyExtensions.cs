using Funda.Tasks.Core;
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
            services.AddTransient<ITaskMapper, TaskMapper>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddDbContext<TaskEntity>(new DbContextSettings
            {
                StorageConectionString = configuration["AzureWebJobsStorage"],
                TableName = "tasks"
            });

            return services;
        }

        private static IServiceCollection AddDbContext<T>(this IServiceCollection services, DbContextSettings settings) where T:TableEntity, new()
        {
            return services.AddTransient<IDbContext<T>, DbContext<T>>( x => new DbContext<T>(settings,x.GetService<ILogger<T>>()));
        }
    }
}
