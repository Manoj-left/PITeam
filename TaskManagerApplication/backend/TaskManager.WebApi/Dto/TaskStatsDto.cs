using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Dto;

public class TaskStatsDto
{
    public int TotalTasks{get;set;}
    public Dictionary<TaskStatusType, int> StatusCounts{get;set;}

}