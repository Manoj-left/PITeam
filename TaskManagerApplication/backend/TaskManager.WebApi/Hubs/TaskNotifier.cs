using Microsoft.AspNetCore.SignalR;
using TaskManager.WebApi.Dto;
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

    public Task UserRegisteredAsync(UserDto user) =>
        _hubContext.Clients.All.SendAsync("UserRegistered", user);

    public Task UserDeletedAsync(int id) =>
        _hubContext.Clients.All.SendAsync("UserDeleted", id);
}
