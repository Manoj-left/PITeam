namespace TaskManager.Mvc.Services;

using TaskManager.Mvc.Repository;
using TaskManager.Mvc.Models;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasksAsync(TaskQueryParameter queryParameter);
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<TaskItem> CreateTaskAsync(CreateTaskDto dto);
    Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
}
