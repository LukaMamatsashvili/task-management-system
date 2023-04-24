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
    public class TaskAttachmentRepository : ITaskAttachmentRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskAttachmentRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskAttachment>> GetTaskAttachmentsAsync()
        {
            return await _context.TaskAttachments.ToListAsync();
        }

        public async Task<TaskAttachment> GetTaskAttachmentByIdAsync(int id)
        {
            var TaskAttachment = await _context.TaskAttachments.FirstOrDefaultAsync(t => t.Id == id);

            if (TaskAttachment == null)
            {
                throw new NotFoundException($"Task attachment with ID '{id}' not found.");
            }

            return TaskAttachment;
        }

        public async Task<List<TaskAttachment>> GetTaskAttachmentsByTaskIdAsync(int taskId)
        {
            var TaskAttachments = await _context.TaskAttachments.Where(t => t.TaskId == taskId).ToListAsync();

            return TaskAttachments;
        }

        public async Task<int> AddTaskAttachmentAsync(TaskAttachment TaskAttachment)
        {
            await _context.TaskAttachments.AddAsync(TaskAttachment);
            await _context.SaveChangesAsync();

            return TaskAttachment.Id;
        }

        public async Task UpdateTaskAttachmentAsync(TaskAttachment TaskAttachment)
        {
            _context.Entry(TaskAttachment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAttachmentAsync(int id)
        {
            var TaskAttachment = await _context.TaskAttachments.FindAsync(id);

            if (TaskAttachment == null)
            {
                throw new NotFoundException($"Task attachment with ID '{id}' not found.");
            }

            _context.TaskAttachments.Remove(TaskAttachment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAttachmentsByTaskIdAsync(int taskId)
        {
            var taskAttachments = await _context.TaskAttachments.Where(ta => ta.TaskId == taskId).ToListAsync();

            if (taskAttachments == null || !taskAttachments.Any())
            {
                throw new NotFoundException($"Task attachment with ID '{taskId}' not found.");
            }

            _context.TaskAttachments.RemoveRange(taskAttachments);
            await _context.SaveChangesAsync();
        }
    }
}
