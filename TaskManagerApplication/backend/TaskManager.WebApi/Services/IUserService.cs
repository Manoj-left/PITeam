namespace TaskManager.WebApi.Services;

using TaskManager.WebApi.Models;

public interface IUserService
{
    // search is matched against Username (case-insensitive, partial match); null/empty returns everyone
    Task<IEnumerable<UserDto>> SearchUsersAsync(string? search);
    Task<bool> DeleteUserAsync(int id);
}
