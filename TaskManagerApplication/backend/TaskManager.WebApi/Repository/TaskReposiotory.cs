namespace TaskManager.WebApi.Repository;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Data;

public class TaskRepository: ITaskRepository
{
    private readonly TaskManagerDbContext _context;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    // Access the Tasks table through EF Core and use Entity Framework Core methods to perform CRUD operations.
    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem taskItem)
    {
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var taskItem = await _context.Tasks.FindAsync(id);
        if (taskItem is null)
        {
            return false;
        }

        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
