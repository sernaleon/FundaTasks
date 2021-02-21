using Funda.Tasks.Core;
using System;

namespace Funda.Tasks.Infrastructure.TableStorage.Mappers
{
    public interface IUserMapper
    {
        UserEntity Map(User user);
        User Map(UserEntity userEntity);
    }

    public class UserMapper : IUserMapper
    {
        public User Map(UserEntity userEntity)
        {
            if (userEntity == null) return null;

            return new User
            {
                Id = new Guid(userEntity.RowKey),
                Name = userEntity.PartitionKey
            };
        }

        public UserEntity Map(User user)
        {
            if (user == null) return null;

            return new UserEntity(user.Id.ToString(), user.Name);
        }
    }
}
