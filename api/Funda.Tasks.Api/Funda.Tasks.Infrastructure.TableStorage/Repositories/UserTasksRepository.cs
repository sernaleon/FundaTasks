using Funda.Tasks.Core;
using Funda.Tasks.Infrastructure.TableStorage.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage.Repositories
{
    public class UserTasksRepository : IUserTasks
    {
        private readonly IDbContext<UserTaskEntity> _userDbContext;
        private readonly IDbContext<TaskEntity> _taskDbContext;
        private readonly IUserTaskMapper _mapper;

        public UserTasksRepository(IDbContext<UserTaskEntity> userDbContext, IDbContext<TaskEntity> taskDbContext, IUserTaskMapper mapper)
        {
            _userDbContext = userDbContext;
            _taskDbContext = taskDbContext;
            _mapper = mapper;
        }

        public Task DeleteUserTasksAsync(Guid userId, TaskLineItem task, CancellationToken token)
        {
            var entity = _mapper.Map(userId, task);
            return _userDbContext.DeleteAsync(entity, token);
        }

        public async Task<IEnumerable<TaskLineItem>> GetUserTasksAsync(Guid userId, CancellationToken token)
        {
            var entities = await _userDbContext.SelectWhereAsync(x => x.PartitionKey == userId.ToString(), token);
            var tasks = await _taskDbContext.SelectWhereAsync(x => x.PartitionKey == userId.ToString(), token);

            var models = entities?.Select(e => _mapper.Map(e, tasks.Single(t => t.RowKey == e.TaskId)));
            return models;
        }

        public Task SetUserTasksAsync(Guid userId, TaskLineItem task, CancellationToken token)
        {
            var entity = _mapper.Map(userId, task);
            return _userDbContext.SetAsync(entity, token);
        }
    }
}
