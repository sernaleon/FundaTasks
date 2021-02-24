using Funda.Tasks.Core;
using Funda.Tasks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbContext<TaskEntity> _dbContext;
        private readonly ITaskMapper _mapper;

        public TaskRepository(IDbContext<TaskEntity> dbContext, ITaskMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<TaskModel>> GetAllAsync(Guid userId, CancellationToken token)
        {
            var entities = await _dbContext.SelectWhereAsync(x => x.PartitionKey == userId.ToString(), token);
            var models = entities.Select(_mapper.Map)?.ToList();
            return models;
        }

        public Task SetAsync(Guid userId, NewTaskModel task, CancellationToken token)
        {
            var entity = _mapper.Map(userId, task);
            return _dbContext.SetAsync(entity, token);
        }

        public Task SetAsync(Guid userId, UpdateTaskModel task, CancellationToken token)
        {
            var entity = _mapper.Map(userId, task);
            return _dbContext.SetAsync(entity, token);
        }

        public async Task DeleteAsync(Guid userId, Guid taskId, CancellationToken token)
        {
            var entity = await _dbContext.GetAsync(userId.ToString(), taskId.ToString(), token);
            await _dbContext.DeleteAsync(entity, token);
        }
    }
}
