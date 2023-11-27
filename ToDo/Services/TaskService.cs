using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Interfaces;
using Task = ToDo.Models.Task;

namespace ToDo.Services
{
    public class TaskService : ITaskService
    {
        private readonly MyDbContext _context;

        public TaskService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Task>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Task> GetTaskById(int? id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Task>> GetSortedTasks(string sortOrder)
        {
            var query = _context.Tasks.AsQueryable();

            switch (sortOrder)
            {
                case "asc":
                    query = query.OrderBy(task => task.Title);
                    break;
                case "desc":
                    query = query.OrderByDescending(task => task.Title);
                    break;
                case "oldest":
                    query = query.OrderBy(task => task.StartDate);
                    break;
                case "newest":
                    query = query.OrderByDescending(task => task.StartDate);
                    break;

                default:
                    break;
            }

            return await query.ToListAsync();
        }



        public async Task<bool> CreateTask(Task task)
        {
            try
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTask(int? id, Task updatedTask)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return false; // Task not found
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.StartDate = updatedTask.StartDate;
            existingTask.EndDate = updatedTask.EndDate;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            try
            {
                _context.Update(existingTask);
                await _context.SaveChangesAsync();
                return true; // Update successful
            }
            catch
            {
                return false; // Update failed
            }
        }


        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
