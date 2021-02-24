using Funda.Tasks.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Core
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllAsync(Guid userId, CancellationToken token);
        Task SetAsync(Guid userId, NewTaskModel task, CancellationToken token);
        Task SetAsync(Guid userId, UpdateTaskModel task, CancellationToken token);
        Task DeleteAsync(Guid userId, Guid taskId, CancellationToken token);
    }
}
