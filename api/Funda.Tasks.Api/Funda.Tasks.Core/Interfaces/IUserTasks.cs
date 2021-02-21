using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Core
{
    public interface IUserTasks
    {
        Task<IEnumerable<TaskLineItem>> GetUserTasksAsync(Guid userId, CancellationToken token);
        Task SetUserTasksAsync(Guid userId, TaskLineItem task, CancellationToken token);
        Task DeleteUserTasksAsync(Guid userId, TaskLineItem task, CancellationToken token);
    }
}
