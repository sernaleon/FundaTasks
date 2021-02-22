using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Core
{
    public interface ITasks
    {
        Task<List<TaskType>> GetAllAsync(Guid userId, CancellationToken token);
        Task AddTaskAsync(Guid userId, TaskType task, CancellationToken token);
    }
}
