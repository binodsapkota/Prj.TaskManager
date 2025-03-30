using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Data;
using Prj.TaskManager.Filters;
using Prj.TaskManager.Models;

namespace Prj.TaskManager.Controllers
{
    [CustomAuthorize(roles:"admin")]
    public class TaskController : Controller
    {
        readonly AppDbContext _context;
        public TaskController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _context.Tasks.ToListAsync();
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
                _context.Tasks.Add(model);
                var result = await _context.SaveChangesAsync();
                TempData["message"] = "Task Added";
                return RedirectToAction("Index");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var task = _context.Tasks.SingleOrDefault(  x => x.Id == id);
            return View(task);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.SingleOrDefault(x => x.Id == id);
            return View(task);
        }
        [HttpPost("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.Tasks.SingleOrDefault(x => x.Id == id);
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.SingleOrDefault(x => x.Id == id);
            return View(task);
        }
        [HttpPost]
        public IActionResult Edit(int id,TaskItemModel model)
        {
            if (id != model.Id ||ModelState.IsValid==false)
            {
                return View(model);
            }
            

            _context.Tasks.Update(model);   
            _context.SaveChanges();
            TempData["message"] = "task updated";
            return RedirectToAction("Index");
        }

    }
}
