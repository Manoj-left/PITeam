namespace TaskManager.WebApi.Models;

public class TaskQueryParameter
{
    public TaskStatusType? Status { get; set; }
    public Priority? Priority { get; set; } 
    public string? SortBy { get; set; }
    public bool? IsAscending { get; set; }
    // omitted by admin searches to see every user's tasks; a regular user's client always supplies its own id
    public int? UserId { get; set; }
}
