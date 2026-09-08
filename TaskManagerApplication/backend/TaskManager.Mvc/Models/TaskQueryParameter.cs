namespace TaskManager.Mvc.Models;

public class TaskQueryParameter
{
    public TaskStatusType? Status { get; set; }
    public Priority? Priority { get; set; } 
    public string? SortBy { get; set; }
    public bool? IsAscending { get; set; }
}