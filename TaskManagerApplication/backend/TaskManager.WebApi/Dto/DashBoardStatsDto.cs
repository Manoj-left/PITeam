namespace TaskManager.WebApi.Dto;

using TaskManager.WebApi.Models;
public class DashBoardStatsDto
{
    public TaskStatsDto TaskStats{get;set;} = new();

    public int TotalUsers{get;set;}=0;
}