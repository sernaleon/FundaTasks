using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Core
{
    public interface IUserTasksRepository
    {
        Task<UserTasks[]> GetAllUserTasksAsync(CancellationToken token);
        Task<UserTasks> GetUserTasksAsync(Guid userId, CancellationToken token);
        Task SetUserTasksAsync(UserTasks userTasks, CancellationToken token);
        Task DeleteUserTasksAsync(Guid userId, CancellationToken token);
    }
}
