using Funda.Tasks.Core;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public interface IUserTasksMapper
    {
        UserTasksEntity Map(UserTasks userTasks);
        UserTasks Map(UserTasksEntity userTaskEntity);
    }

    public class UserTasksMapper : IUserTasksMapper
    {
        public UserTasksEntity Map(UserTasks userTasks)
        {
            return new UserTasksEntity
            {
                UserTasks = userTasks,
                RowKey = userTasks.User.Id.ToString(),
                PartitionKey = UserTasksEntityDefinitions.PartitionKey,
                ETag = "*"
            };
        }

        public UserTasks Map(UserTasksEntity userTaskEntity)
        {
            return userTaskEntity.UserTasks;
        }
    }
}
