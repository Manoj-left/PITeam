namespace TaskManager.WebApi.Repository;
using TaskManager.WebApi.Models;

// this interface should have only persistence and no business logic.
// repository always returns the full set of data and does not filter based on business logic.
public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem taskItem);
    Task UpdateAsync(TaskItem taskItem);
    Task<bool> DeleteAsync(int id);
}
