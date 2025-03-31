using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj.TaskManager.Service
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        public TaskService(AppDbContext appDbContext)
        {
            _context = appDbContext;

        }
        public async Task<TaskItemModel> Create(TaskItemModel model)
        {

            _context.Tasks.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var task = await GetTask(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItemModel>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskItemModel> GetTask(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItemModel> Update(TaskItemModel model)
        {
            _context.Tasks.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
