using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Data;

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
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
