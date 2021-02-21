using Funda.Tasks.Core;
using System;

namespace Funda.Tasks.Infrastructure.TableStorage.Mappers
{
    public interface ITaskMapper
    {
        TaskType Map(TaskEntity taskEntity);
        TaskEntity Map(Guid userId, TaskType task);
    }

    public class TaskMapper : ITaskMapper
    {
        public TaskType Map(TaskEntity taskEntity)
        {
            if (taskEntity == null) return null;

            return new TaskType
            {
                Id = new Guid(taskEntity.RowKey),
                Name = taskEntity.Name
            };
        }

        public TaskEntity Map(Guid userId, TaskType task)
        {
            if (task == null) return null;

            return new TaskEntity(task.Id.ToString(), userId.ToString(), task.Name);
        }
    }
}
