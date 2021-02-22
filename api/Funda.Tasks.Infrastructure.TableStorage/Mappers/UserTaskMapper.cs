using Funda.Tasks.Core;
using System;

namespace Funda.Tasks.Infrastructure.TableStorage.Mappers
{
    public interface IUserTaskMapper
    {
        UserTaskEntity Map(Guid userId, TaskLineItem task);
        TaskLineItem Map(UserTaskEntity userTaskEntity, TaskEntity taskEntity);
    }

    public class UserTaskMapper : IUserTaskMapper
    {
        private readonly ITaskMapper _taskMapper;

        public UserTaskMapper(ITaskMapper taskMapper)
        {
            _taskMapper = taskMapper;
        }

        public UserTaskEntity Map(Guid userId, TaskLineItem task)
        {
            return new UserTaskEntity(task.Id.ToString(), userId.ToString(), task.Task.Id.ToString(), task.Description);
        }

        public TaskLineItem Map(UserTaskEntity userTaskEntity, TaskEntity taskEntity)
        {
            return new TaskLineItem
            { 
                Id = new Guid(userTaskEntity.RowKey),
                Task = _taskMapper.Map(taskEntity),
                Description = userTaskEntity.Description,
                Timestamp = userTaskEntity.Timestamp
            };
        }
    }
}
