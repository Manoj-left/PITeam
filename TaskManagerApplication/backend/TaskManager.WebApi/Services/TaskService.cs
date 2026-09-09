namespace TaskManager.WebApi.Services;
using TaskManager.WebApi.Repository;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Hubs;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskNotifier _taskNotifier;

    public TaskService(ITaskRepository taskRepository, ITaskNotifier taskNotifier)
    {
        _taskRepository = taskRepository;
        _taskNotifier = taskNotifier;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAsync(TaskQueryParameter queryParameter)
    {
        var tasks = await _taskRepository.GetAllAsync();
// I can also add the DUeAt for filtering here
        if (queryParameter.UserId.HasValue)
        {
            tasks = tasks.Where(t => t.UserId == queryParameter.UserId.Value);
        }

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
            // ties broken by due date (soonest first) so same-priority tasks still have a meaningful order
            "priority" => queryParameter.IsAscending == false
                ? tasks.OrderByDescending(t => t.Priority).ThenBy(t => t.DueAt)
                : tasks.OrderBy(t => t.Priority).ThenBy(t => t.DueAt),
            // ties broken by priority (highest first) so same-due-date tasks still have a meaningful order
            "dueat" => queryParameter.IsAscending == false
                ? tasks.OrderByDescending(t => t.DueAt).ThenByDescending(t => t.Priority)
                : tasks.OrderBy(t => t.DueAt).ThenByDescending(t => t.Priority),
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
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            UserId = dto.UserId
        };

        await _taskRepository.AddAsync(taskItem);
        await _taskNotifier.TaskCreatedAsync(taskItem);
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
        await _taskNotifier.TaskUpdatedAsync(existingTask);
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var deleted = await _taskRepository.DeleteAsync(id);
        if (deleted)
        {
            await _taskNotifier.TaskDeletedAsync(id);
        }
        return deleted;
    }
}
