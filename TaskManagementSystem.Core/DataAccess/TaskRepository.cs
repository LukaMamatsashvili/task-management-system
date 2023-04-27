using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure;
using TaskManagementSystem.Infrastructure.Models;
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
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Infrastructure.Models.Task> GetTaskByIdAsync(int id)
        {
            try
            {
                var Task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

                return Task;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddTaskAsync(Infrastructure.Models.Task Task)
        {
            try
            {
                await _context.Tasks.AddAsync(Task);
                await _context.SaveChangesAsync();

                return Task.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateTaskAsync(Infrastructure.Models.Task Task)
        {
            try
            {
                var task = (await _context.Tasks.FirstOrDefaultAsync(task => task.Id == Task.Id));

                task.AssignedUserId = Task.AssignedUserId > 0 ? Task.AssignedUserId : task.AssignedUserId;
                task.CreatorId = Task.CreatorId > 0 ? Task.CreatorId : task.CreatorId;
                task.Title = !string.IsNullOrEmpty(Task.Title) ? Task.Title : task.Title;
                task.ShortDescription = !string.IsNullOrEmpty(Task.ShortDescription) ? Task.ShortDescription : task.ShortDescription;
                task.Description = !string.IsNullOrEmpty(Task.Description) ? Task.Description : task.Description;

                await _context.SaveChangesAsync();

                return Task.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteTaskAsync(Infrastructure.Models.Task Task)
        {
            try
            {
                _context.Tasks.Remove(Task);
                await _context.SaveChangesAsync();

                return Task.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
