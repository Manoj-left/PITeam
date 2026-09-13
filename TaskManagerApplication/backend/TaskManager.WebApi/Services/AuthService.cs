namespace TaskManager.WebApi.Services;

using Microsoft.AspNetCore.Identity;
using TaskManager.WebApi.Hubs;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Repository;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskNotifier _taskNotifier;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(IUserRepository userRepository, ITaskNotifier taskNotifier)
    {
        _userRepository = userRepository;
        _taskNotifier = taskNotifier;
    }

    public async Task<UserDto?> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existing is not null)
        {
            return null;
        }

        var user = new User
        {
            Username = dto.Username,
            Role = UserRole.User
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);
        var userDto = ToDto(user);
        await _taskNotifier.UserRegisteredAsync(userDto);
        return userDto;
    }

    public async Task<UserDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return ToDto(user);
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Role = user.Role
    };
}
