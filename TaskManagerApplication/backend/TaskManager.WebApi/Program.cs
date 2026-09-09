using TaskManager.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Repository;
using TaskManager.WebApi.Services;
using TaskManager.WebApi.Hubs;
using TaskManager.WebApi.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("TaskManagerDb"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("TaskManagerDb"))));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskNotifier, TaskNotifier>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
        // AllowCredentials is required for SignalR's negotiate/fallback transports to work with a specific origin.
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

// one-time seed so there's always at least one Admin account to log in with; no-op once one exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
    if (!db.Users.Any(u => u.Role == UserRole.Admin))
    {
        var admin = new User { Username = "admin", Role = UserRole.Admin };
        admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, "Admin@123");
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");
app.MapControllers();
app.MapHub<TaskHub>("/hubs/tasks");

app.Run();
