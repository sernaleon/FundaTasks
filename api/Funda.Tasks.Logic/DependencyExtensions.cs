using Microsoft.Extensions.DependencyInjection;

namespace Funda.Tasks.Logic
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddFundaTasksLogic(this IServiceCollection services)
        {
            return services.AddTransient<ITaskService, TaskService>();
        }
    }
}
