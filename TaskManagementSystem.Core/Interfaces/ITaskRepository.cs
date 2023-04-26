using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Infrastructure.Models.Task>> GetTasksAsync();
        Task<Infrastructure.Models.Task> GetTaskByIdAsync(int id);
        Task<int> AddTaskAsync(Infrastructure.Models.Task Task);
        Task<int> UpdateTaskAsync(Infrastructure.Models.Task Task);
        Task<int> DeleteTaskAsync(Infrastructure.Models.Task Task);
    }
}
