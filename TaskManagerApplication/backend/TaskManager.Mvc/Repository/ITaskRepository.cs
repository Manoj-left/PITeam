namespace TaskManager.Mvc.Repository;
using TaskManager.Mvc.Models;

public interface ITaskRepository
{
    TaskItem GetTaskItembyId(int id);
    IEnumerable<TaskItem> GetAllTaskItems();
    void AddTaskItem(TaskItem taskItem);
    void UpdateTaskItem(TaskItem taskItem);
    void DeleteTaskItem(int id);
    IEnumerable<TaskItem> GetTaskItemsByStatus(string status);
    IEnumerable<TaskItem> GetTaskItemsByPriority(string priority);
}