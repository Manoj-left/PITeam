namespace TaskManager.Mvc.Controllers;
using TaskManager.Mvc.Services; 
using TaskManager.Mvc.Models; 
using Microsoft.AspNetCore.Mvc;
// controllers represent a resource collection.
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskById(int id)
    {
        var taskItem = await _taskService.GetTaskByIdAsync(id);
        if (taskItem is null)
        {
            return NotFound();
        }
        return Ok(taskItem);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskDto dto)
    {
        var taskItem = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetTaskById), new { id = taskItem.Id }, taskItem);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] TaskQueryParameter queryParameter)
    {
        var tasks = await _taskService.GetTasksAsync(queryParameter);
        if (tasks is null || !tasks.Any())
        {
            return NotFound();
        }
        return Ok(tasks);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TaskItem>> UpdateTask(int id, UpdateTaskDto dto)
    {
        var updatedTask = await _taskService.UpdateTaskAsync(id, dto);
        if(updatedTask is null)
        {
            return NotFound();
        }
        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTask(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}