using TaskManager.WebApi.Dto;
using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Hubs;

// Abstraction over IHubContext so TaskService/UserService/AuthService don't depend directly on SignalR types.
public interface ITaskNotifier
{
    Task TaskCreatedAsync(TaskItem task);
    Task TaskUpdatedAsync(TaskItem task);
    Task TaskDeletedAsync(int id);
    Task UserRegisteredAsync(UserDto user);
    Task UserDeletedAsync(int id);
}
