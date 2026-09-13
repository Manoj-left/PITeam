using TaskManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Dto;
using TaskManager.WebApi.Services;

namespace TaskManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController: ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<DashBoardStatsDto>> GetDashboardStats([FromQuery] int? userId, [FromQuery] bool isAdmin = false)
    {
        var stats = await _dashboardService.GetDashboardStats(userId, isAdmin);
        return Ok(stats);
    }
    
}