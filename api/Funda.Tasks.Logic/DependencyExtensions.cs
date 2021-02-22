using Microsoft.Extensions.DependencyInjection;

namespace Funda.Tasks.Logic
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddFundaTasksLogic(this IServiceCollection services)
        {
            services.AddTransient<INewTaskHandler, NewTaskHandler>();
            return services;
        }
    }
}
