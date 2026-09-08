using TaskManager.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.Mvc.Repository;
using TaskManager.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("TaskManagerDb"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("TaskManagerDb"))));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
