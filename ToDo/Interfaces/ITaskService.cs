using Microsoft.AspNetCore.Mvc;
using Task = ToDo.Models.Task;
namespace ToDo.Interfaces
{
    public interface ITaskService
    {
        Task<List<Task>> GetTasks();
        Task<Task> GetTaskById(int? id);
        Task<bool> CreateTask(Task task);
        Task<bool> UpdateTask(int? id, Task task);
        Task<bool> DeleteTask(int id);
        Task<List<Task>> GetSortedTasks(string sortOrder);
    }
}
