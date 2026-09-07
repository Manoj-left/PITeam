using Microsoft.EntityFrameworkCore;
using TaskManager.Mvc.Models;

namespace TaskManager.Mvc.Data;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(
        DbContextOptions<TaskManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }
}