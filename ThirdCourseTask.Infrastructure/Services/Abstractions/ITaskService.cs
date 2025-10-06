using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdCourseTask.Data.Entities;

namespace ThirdCourseTask.Infrastructure.Services.Abstractions
{
    public interface ITaskService
    {
        Task<int> AddAsync(string title, string? description);
        Task<IReadOnlyList<TaskEntity>> GetAllAsync();
        Task<bool> CompleteAsync(int id, bool isCompleted);
        Task<bool> RemoveAsync(int id);
    }
}
