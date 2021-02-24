using Funda.Tasks.Core;
using Funda.Tasks.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Logic
{
    public interface ITaskService
    {
        Task<List<TaskModel>> GetAllAsync(Guid userId, CancellationToken token);
        Task<List<TaskModel>> AddAsync(Guid userId, NewTaskModel newTask, CancellationToken token);
        Task<List<TaskModel>> UpdateAsync(Guid userId, UpdateTaskModel updateTask, CancellationToken token);
        Task<List<TaskModel>> DeleteAsync(Guid userId, Guid taskId, CancellationToken token);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<List<TaskModel>> GetAllAsync(Guid userId, CancellationToken token)
        {
            return _taskRepository.GetAllAsync(userId, token);
        }

        public async Task<List<TaskModel>> AddAsync(Guid userId, NewTaskModel newTask, CancellationToken token)
        {
            var task = new TaskModel
            {
                Id = Guid.NewGuid(),
                GroupId = newTask.GroupId ?? Guid.NewGuid(),
                Name = newTask.Name,
                Description = newTask.Description,
                Timestamp = DateTimeOffset.Now
            };
            await _taskRepository.SetAsync(userId, task, token);
            return await _taskRepository.GetAllAsync(userId, token);
        }

        public async Task<List<TaskModel>> UpdateAsync(Guid userId, UpdateTaskModel updateTask, CancellationToken token)
        {
            var task = new TaskModel
            {
                Id = Guid.NewGuid(),
                GroupId = updateTask.GroupId,
                Name = updateTask.Name,
                Description = updateTask.Description,
                Timestamp = DateTimeOffset.Now,
            };
            await _taskRepository.SetAsync(userId, task, token);
            return await _taskRepository.GetAllAsync(userId, token);
        }

        public async Task<List<TaskModel>> DeleteAsync(Guid userId, Guid taskId, CancellationToken token)
        {
            await _taskRepository.DeleteAsync(userId, taskId, token);
            return await _taskRepository.GetAllAsync(userId, token);
        }
    }
}
