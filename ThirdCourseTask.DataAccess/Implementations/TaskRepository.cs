using System.Data;
using Dapper;
using ThirdCourseTask.Data.Entities;
using ThirdCourseTask.DataAccess.Abstractions;

namespace ThirdCourseTask.DataAccess.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _factory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            _factory = connectionFactory;
        }

        public async Task<int> CreateAsync(TaskEntity entity)
        {
            const string sql = @"
                INSERT INTO dbo.Tasks(Title, [Description], IsCompleted, CreatedAt)
                VALUES (@Title, @Description, @IsCompleted, SYSUTCDATETIME());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                using var dbConnection = _factory.Create();
                int createdTaskId = await dbConnection.ExecuteScalarAsync<int>(sql, entity);
                return createdTaskId;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create a new task.", ex);
            }
        }

        public async Task<IReadOnlyList<TaskEntity>> GetAllAsync()
        {
            const string sql = @"SELECT Id, Title, [Description], IsCompleted, CreatedAt
                                 FROM dbo.Tasks ORDER BY CreatedAt DESC;";

            try
            {
                using var dbConnection = _factory.Create();
                var tasks = await dbConnection.QueryAsync<TaskEntity>(sql);
                return tasks.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve tasks.", ex);
            }
        }

        public async Task<bool> SetCompletedAsync(int id, bool isCompleted)
        {
            const string sql = @"UPDATE dbo.Tasks SET IsCompleted = @isCompleted WHERE Id = @id;";

            try
            {
                using var dbConnection = _factory.Create();
                int affectedRows = await dbConnection.ExecuteAsync(sql, new { id, isCompleted });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to update completion status for task Id=" + id + ".", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"DELETE FROM dbo.Tasks WHERE Id = @id;";

            try
            {
                using var dbConnection = _factory.Create();
                int affectedRows = await dbConnection.ExecuteAsync(sql, new { id });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete task Id=" + id + ".", ex);
            }
        }
    }
}
