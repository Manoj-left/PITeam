namespace TaskManager.WebApi.Repository;
using TaskManager.WebApi.Models;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
}
