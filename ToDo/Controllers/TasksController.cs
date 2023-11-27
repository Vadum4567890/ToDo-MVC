using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Interfaces;
using ToDo.Models;
using ToDo.Services;
using Task = ToDo.Models.Task;


namespace ToDo.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskServices)
        {
            _taskService = taskServices;
        }

        // Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetTasks();
            return View(tasks);

        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,IsCompleted")] Task task)
        {
            if (ModelState.IsValid)
            {
                await _taskService.CreateTask(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }


        [HttpPost]
        public async Task<IActionResult> SortTasks(string sortOrder)
        {
            try
            {
                var tasks = await _taskService.GetSortedTasks(sortOrder);
                return View("Index", tasks);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception to see it in the console
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,IsCompleted")] Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updateResult = await _taskService.UpdateTask(id, task);
                if (!updateResult)
                {
                    return NotFound(); // Task not found
                }
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleteResult = await _taskService.DeleteTask(id);
            if (!deleteResult)
            {
                return NotFound(); // Task not found
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _taskService.GetTasks().Result.Any(e => e.Id == id);
        }
    }
}
