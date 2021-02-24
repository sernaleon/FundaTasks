using Funda.Tasks.Core.Models;
using System;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public interface ITaskMapper
    {
        TaskEntity Map(Guid userId, NewTaskModel task);
        TaskEntity Map(Guid userId, UpdateTaskModel task);
        TaskModel Map(TaskEntity taskEntity);
    }

    public class TaskMapper : ITaskMapper
    {
        public TaskEntity Map(Guid userId, NewTaskModel task)
        {
            return new TaskEntity
            {
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = userId.ToString(),
                GroupId = task.GroupId.HasValue? task.GroupId.ToString() : Guid.NewGuid().ToString(),
                Name = task.Name,
                Description = task.Description
            };
        }

        public TaskEntity Map(Guid userId, UpdateTaskModel task)
        {
            return new TaskEntity
            {
                RowKey = task.Id.ToString(),
                PartitionKey = userId.ToString(),
                GroupId = task.GroupId.ToString(),
                Name = task.Name,
                Description = task.Description
            };
        }

        public TaskModel Map(TaskEntity taskEntity)
        {
            return new TaskModel
            {
                Id = new Guid(taskEntity.RowKey),
                GroupId = new Guid(taskEntity.GroupId),
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                Timestamp = taskEntity.Timestamp
            };
        }
    }
}
