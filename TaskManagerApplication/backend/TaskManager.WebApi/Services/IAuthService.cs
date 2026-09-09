namespace TaskManager.WebApi.Services;

using TaskManager.WebApi.Models;

public interface IAuthService
{
    // returns null when the username is already taken
    Task<UserDto?> RegisterAsync(RegisterDto dto);
    // returns null when the username doesn't exist or the password doesn't match
    Task<UserDto?> LoginAsync(LoginDto dto);
}
