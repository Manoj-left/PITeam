using TaskManager.WebApi.Dto;
namespace TaskManager.WebApi.Services;

public interface IDashboardService
{
    Task<DashBoardStatsDto> GetDashboardStats(int? userId, bool isAdmin);
}