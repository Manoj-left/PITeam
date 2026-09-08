namespace TaskManager.Mvc.Services;
using TaskManager.Mvc.Repository;
using TaskManager.Mvc.Models;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAsync(TaskQueryParameter queryParameter)
    {
        var tasks = await _taskRepository.GetAllAsync();
// I can also add the DUeAt for filtering here
        if (queryParameter.Status.HasValue)
        {
            tasks = tasks.Where(t => t.Status == queryParameter.Status.Value);
        }

        if (queryParameter.Priority.HasValue)
        {
            tasks = tasks.Where(t => t.Priority == queryParameter.Priority.Value);
        }

        tasks = queryParameter.SortBy?.ToLowerInvariant() switch
        {
            "priority" => queryParameter.IsAscending == false
                ? tasks.OrderByDescending(t => t.Priority)
                : tasks.OrderBy(t => t.Priority),
            "dueat" => queryParameter.IsAscending == false
                ? tasks.OrderByDescending(t => t.DueAt)
                : tasks.OrderBy(t => t.DueAt),
            _ => tasks
        };

        return tasks;
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<TaskItem> CreateTaskAsync(CreateTaskDto dto)
    {
        var taskItem = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueAt = dto.DueAt,
            Priority = dto.Priority,
            Status = TaskStatusType.NotStarted,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(taskItem);
        return taskItem;
    }

    public async Task<TaskItem?> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var existingTask = await _taskRepository.GetByIdAsync(id);
        if (existingTask is null)
        {
            return null;
        }

        existingTask.Title = dto.Title;
        existingTask.Description = dto.Description;
        existingTask.DueAt = dto.DueAt;
        existingTask.Status = dto.Status;
        existingTask.Priority = dto.Priority;

        await _taskRepository.UpdateAsync(existingTask);
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepository.DeleteAsync(id);
    }
}