using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.DataAccess
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Infrastructure.Models.Task>> GetTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Infrastructure.Models.Task> GetTaskByIdAsync(int id)
        {
            var Task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (Task == null)
            {
                throw new NotFoundException($"Task with ID '{id}' not found.");
            }

            return Task;
        }

        public async Task<int> AddTaskAsync(Infrastructure.Models.Task Task)
        {
            await _context.Tasks.AddAsync(Task);
            await _context.SaveChangesAsync();

            return Task.Id;
        }

        public async Task UpdateTaskAsync(Infrastructure.Models.Task Task)
        {
            _context.Entry(Task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var Task = await _context.Tasks.FindAsync(id);

            if (Task == null)
            {
                throw new NotFoundException($"Task with ID '{id}' not found.");
            }

            _context.Tasks.Remove(Task);
            await _context.SaveChangesAsync();
        }
    }
}
