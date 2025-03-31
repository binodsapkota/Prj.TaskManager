using Prj.TaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj.TaskManager.Service
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemModel>> GetAllTasks();
        Task<TaskItemModel> GetTask(int id);
        Task<TaskItemModel> Create(TaskItemModel model);
        Task<TaskItemModel> Update(TaskItemModel model);
        Task<bool> Delete(int id);

    }
}
