namespace TaskManager.Mvc.Repository;
using TaskManager.Mvc.Models;
using System.Collections.Generic;

public class EfRepository: ITaskRepository
{
    private readonly ITaskRepository _taskreposiotry;
    
    public EfRepository(ITaskRepository taskrepository)
    {
        _taskreposiotry = taskrepository;
    }

    public TaskItem GetTaskItembyId(int id)
    {
        
        return null;

        
    }
    
    public IEnumerable<TaskItem> GetAllTaskItems()
    {
        return null;
        
    }
    public void AddTaskItem(TaskItem taskItem)
    {
        
    }
    public void UpdateTaskItem(TaskItem taskItem)
    {
        
    }
    public void DeleteTaskItem(int id)
    {
        
    }
    public IEnumerable<TaskItem> GetTaskItemsByStatus(string status)
    {
        return null;
    }
    public IEnumerable<TaskItem> GetTaskItemsByPriority(string priority)
    {
        return null;
    }
}