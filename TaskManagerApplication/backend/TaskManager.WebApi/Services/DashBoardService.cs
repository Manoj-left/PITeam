namespace TaskManager.WebApi.Services;
using TaskManager.WebApi.Dto;
using TaskManager.WebApi.Repository;

public class DashBoardService : IDashboardService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public DashBoardService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }
    public async Task<DashBoardStatsDto> GetDashboardStats(int? userId, bool isAdmin)
    {
        var tasks = await _taskRepository.GetAllAsync();

        // regular users only ever see their own task breakdown but admins see everyone's
        if (!isAdmin)
        {
            tasks = tasks.Where(t => t.UserId == userId);
        }

        var statusCounts = tasks
            .GroupBy(t => t.Status)
            .ToDictionary(g => (int)g.Key, g => g.Count());

        var stats = new DashBoardStatsDto
        {
            TaskStats = new TaskStatsDto
            {
                TotalTasks = tasks.Count(),
                StatusCounts = statusCounts
            }
        };

        if (isAdmin)
        {
            var users = await _userRepository.GetAllAsync();
            stats.TotalUsers = users.Count(u => u.Role != Models.UserRole.Admin);
        }

        return stats;
    }
}