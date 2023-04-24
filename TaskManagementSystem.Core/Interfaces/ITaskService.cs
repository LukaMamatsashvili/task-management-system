using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDTO>> GetTasks();
        Task<TaskDTO> GetTaskById(int id);
        Task<string> AddTask(TaskDTO TaskDTO);
        Task<string> UpdateTask(TaskDTO TaskDTO);
        Task<string> DeleteTask(int id);
    }
}
