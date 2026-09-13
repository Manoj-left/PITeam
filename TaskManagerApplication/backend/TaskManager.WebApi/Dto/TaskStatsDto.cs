namespace TaskManager.WebApi.Dto;

public class TaskStatsDto
{
    public int TotalTasks{get;set;}
    public Dictionary<int, int> StatusCounts{get;set;} = new();

}