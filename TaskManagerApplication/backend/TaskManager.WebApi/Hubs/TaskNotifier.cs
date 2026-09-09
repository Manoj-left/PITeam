using Microsoft.AspNetCore.SignalR;
using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Hubs;

public class TaskNotifier : ITaskNotifier
{
    private readonly IHubContext<TaskHub> _hubContext;

    public TaskNotifier(IHubContext<TaskHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task TaskCreatedAsync(TaskItem task) =>
        _hubContext.Clients.All.SendAsync("TaskCreated", task);

    public Task TaskUpdatedAsync(TaskItem task) =>
        _hubContext.Clients.All.SendAsync("TaskUpdated", task);

    public Task TaskDeletedAsync(int id) =>
        _hubContext.Clients.All.SendAsync("TaskDeleted", id);
}
