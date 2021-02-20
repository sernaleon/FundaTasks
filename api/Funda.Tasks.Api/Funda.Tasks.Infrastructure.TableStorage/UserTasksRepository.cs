using Funda.Tasks.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public class UserTasksRepository : IUserTasksRepository
    {
        private readonly IDbContext<UserTasksEntity> _dbContext;
        private readonly IUserTasksMapper _mapper;

        public UserTasksRepository(IDbContext<UserTasksEntity> dbContext, IUserTasksMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task DeleteUserTasksAsync(Guid userId, CancellationToken token)
        {
            var userTasks = await _dbContext.GetAsync(UserTasksEntityDefinitions.PartitionKey, userId.ToString(), token);
            await _dbContext.DeleteAsync(userTasks, token);
        }

        public async Task<UserTasks[]> GetAllUserTasksAsync(CancellationToken token)
        {
            var userTasksEntity = await _dbContext.GetAllAsync(token);
            var userTasks = userTasksEntity.Select(_mapper.Map).ToArray();
            return userTasks;
        }

        public async Task<UserTasks> GetUserTasksAsync(Guid userId, CancellationToken token)
        {
            var userTasksEntity = await _dbContext.GetAsync(UserTasksEntityDefinitions.PartitionKey, userId.ToString(), token);
            var userTasks = _mapper.Map(userTasksEntity);
            return userTasks;
        }

        public Task SetUserTasksAsync(UserTasks userTasks, CancellationToken token)
        {
            var userTasksEntity = _mapper.Map(userTasks);
            return _dbContext.SetAsync(userTasksEntity, token);
        }
    }
}
