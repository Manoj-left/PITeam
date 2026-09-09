namespace TaskManager.WebApi.Services;

using TaskManager.WebApi.Models;
using TaskManager.WebApi.Repository;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> SearchUsersAsync(string? search)
    {
        var users = await _userRepository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            users = users.Where(u => u.Username.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role
        });
    }
}
