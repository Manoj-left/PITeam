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

    // TaskItem = Model class which we have in Models folder
    // Tasks = Collection representing the Tasks table in DB
    public DbSet<TaskItem> Tasks { get; set; }
}