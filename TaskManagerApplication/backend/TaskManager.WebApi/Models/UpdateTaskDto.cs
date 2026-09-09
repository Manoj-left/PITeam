namespace TaskManager.WebApi.Models;

public class UpdateTaskDto
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateOnly DueAt { get; set; }
    public TaskStatusType Status { get; set; }
    public Priority Priority { get; set; }
}
