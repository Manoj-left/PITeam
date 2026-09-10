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

        // Admin accounts aren't task-owning users, so they don't belong in the browsable/searchable roster
        users = users.Where(u => u.Role != UserRole.Admin);

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

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}
