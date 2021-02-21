using Funda.Tasks.Core;
using Funda.Tasks.Infrastructure.TableStorage.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage.Repositories
{
    public class TasksRepository : ITasks
    {
        private readonly IDbContext<TaskEntity> _dbContext;
        private readonly ITaskMapper _mapper;

        public TasksRepository(IDbContext<TaskEntity> dbContext, ITaskMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task AddTaskAsync(Guid userId, TaskType task, CancellationToken token)
        {
            var entity = _mapper.Map(userId, task);
            return _dbContext.SetAsync(entity, token);
        }

        public async Task<List<TaskType>> GetAllAsync(Guid userId, CancellationToken token)
        {
            var entities = await _dbContext.SelectWhereAsync(x => x.PartitionKey == userId.ToString(),token);
            var models = entities.Select(_mapper.Map)?.ToList();
            return models;
        }
    }
}
