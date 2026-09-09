namespace TaskManager.WebApi.Repository;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Data;

public class UserRepository : IUserRepository
{
    private readonly TaskManagerDbContext _context;

    public UserRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
