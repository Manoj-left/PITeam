namespace TaskManager.WebApi.Services;

using TaskManager.WebApi.Hubs;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Repository;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskNotifier _taskNotifier;

    public UserService(IUserRepository userRepository, ITaskRepository taskRepository, ITaskNotifier taskNotifier)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _taskNotifier = taskNotifier;
    }

    public async Task<IEnumerable<UserDto>> SearchUsersAsync(string? search)
    {
        var users = await _userRepository.GetAllAsync();

        // Admin accounts aren't task-owning users, so they don't belong in the browsable/searchable roster
        users = users.Where(u => u.Role != UserRole.Admin);

        if (!string.IsNullOrWhiteSpace(search))
        {
            users = users.Where(u => u.Username.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var tasks = await _taskRepository.GetAllAsync();
        var taskCountsByUserId = tasks.GroupBy(t => t.UserId).ToDictionary(g => g.Key, g => g.Count());

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role,
            TaskCount = taskCountsByUserId.TryGetValue(u.Id, out var count) ? count : 0
        });
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        if (deleted)
        {
            await _taskNotifier.UserDeletedAsync(id);
        }
        return deleted;
    }
}
