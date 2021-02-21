using Funda.Tasks.Core;
using Funda.Tasks.Infrastructure.TableStorage.Mappers;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage.Repositories
{
    public class UserRepository : IUser
    {
        private readonly IDbContext<UserEntity> _dbContext;
        private readonly IUserMapper _userMapper;

        public UserRepository(IDbContext<UserEntity> dbContext, IUserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public Task AddUserAsync(User user, CancellationToken token)
        {
            var entity = _userMapper.Map(user);
            return _dbContext.SetAsync(entity, token);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token)
        {
            var entities = await _dbContext.SelectAsync(token);
            var models = entities?.Select(_userMapper.Map);
            return models;
        }
    }
}
