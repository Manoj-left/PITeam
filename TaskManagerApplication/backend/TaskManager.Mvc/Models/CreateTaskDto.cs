namespace TaskManager.Mvc.Models;

// this is hwat the client can provide when creating a new task(safetynet for the server)
public class CreateTaskDto
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTime DueAt { get; set; }
    public Priority Priority { get; set; }
}