using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Data;
using Prj.TaskManager.Filters;
using Prj.TaskManager.Models;
using Prj.TaskManager.Service;

namespace Prj.TaskManager.Controllers
{
    [CustomAuthorize(roles: "user")]
    public class TaskController : Controller
    {

        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _taskService.GetAllTasks();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItemModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _taskService.Create(model);
                TempData["message"] = "Task Added";
                return RedirectToAction("Index");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var task = await _taskService.GetTask(id);
            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTask(id);
            return View(task);
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.Delete(id);
            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _taskService.GetTask(id).Result;
            return View(task);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskItemModel model)
        {
            if (id != model.Id || ModelState.IsValid == false)
            {
                return View(model);
            }


            await _taskService.Update(model);
            TempData["message"] = "task updated";
            return RedirectToAction("Index");
        }

    }
}
