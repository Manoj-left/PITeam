namespace TaskManager.WebApi.Models;

public class TaskItem
{
    public int Id {get; set;}
    public string Title {get; set;} = String.Empty;
    public string Description {get; set;} = String.Empty;
    public DateTime CreatedAt {get; set;}
    public DateTime? UpdatedAt {get; set;}
    public DateOnly DueAt {get; set;}
    public TaskStatusType Status {get; set;}
    public Priority Priority {get; set;}
    public int UserId {get; set;}
    public DateTime? StatusChangedAt {get; set;}
}
