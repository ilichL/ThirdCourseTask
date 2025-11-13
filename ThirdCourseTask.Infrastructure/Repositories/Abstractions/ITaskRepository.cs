using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdCourseTask.Data.Entities;

namespace ThirdCourseTask.Infrastructure.Database.Repositories.Abstractions
{
    public interface ITaskRepository
    {
        Task<int> CreateAsync(TaskEntity entity);
        Task<IReadOnlyList<TaskEntity>> GetAllAsync();
        Task<bool> SetCompletedAsync(int taskId, bool isCompleted);
        Task<bool> DeleteAsync(int taskId);
    }
}
