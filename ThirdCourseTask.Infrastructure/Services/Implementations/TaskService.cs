using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdCourseTask.Data.Entities;
using ThirdCourseTask.Infrastructure.Database.Repositories.Abstractions;
using ThirdCourseTask.Infrastructure.Services.Abstractions;

namespace ThirdCourseTask.Infrastructure.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> AddAsync(string title, string? description)
        {
           return await _taskRepository.CreateAsync(new TaskEntity
            {
                Title = title,
                Description = description,
                IsCompleted = false,
            });
        }

        public async Task<IReadOnlyList<TaskEntity>> GetAllAsync()
        {
            return await _taskRepository.GetAllAsync();
        }


        public async Task<bool> CompleteAsync(int id, bool isCompleted)
        {
            return await _taskRepository.SetCompletedAsync(id, isCompleted);
        }


        public async Task<bool> RemoveAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

    }
}
