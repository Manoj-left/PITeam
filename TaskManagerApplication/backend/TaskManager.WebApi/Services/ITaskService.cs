namespace TaskManager.WebApi.Services;

using TaskManager.WebApi.Repository;
using TaskManager.WebApi.Models;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasksAsync(TaskQueryParameter queryParameter);
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<TaskItem> CreateTaskAsync(CreateTaskDto dto);
    Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
}
