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
            try
            {
                return await _context.TaskAttachments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TaskAttachment>> GetTaskAttachmentsByTaskIdAsync(int taskId)
        {
            try
            {
                return await _context.TaskAttachments.Where(t => t.TaskId == taskId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TaskAttachment> GetTaskAttachmentByIdAsync(int id)
        {
            try
            {
                var TaskAttachment = await _context.TaskAttachments.FirstOrDefaultAsync(t => t.Id == id);

                return TaskAttachment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddTaskAttachmentAsync(TaskAttachment TaskAttachment)
        {
            try
            {
                await _context.TaskAttachments.AddAsync(TaskAttachment);
                await _context.SaveChangesAsync();

                return TaskAttachment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateTaskAttachmentAsync(TaskAttachment TaskAttachment)
        {
            try
            {
                var taskAttachment = (await _context.TaskAttachments.FirstOrDefaultAsync(taskAttachment => taskAttachment.Id == TaskAttachment.Id));

                taskAttachment.TaskId = TaskAttachment.TaskId > 0 ? TaskAttachment.TaskId : taskAttachment.TaskId;
                taskAttachment.FileName = !string.IsNullOrEmpty(TaskAttachment.FileName) ? TaskAttachment.FileName : taskAttachment.FileName;
                taskAttachment.ContentType = !string.IsNullOrEmpty(TaskAttachment.ContentType) ? TaskAttachment.ContentType : taskAttachment.ContentType;
                taskAttachment.FileData = TaskAttachment.FileData != null ? TaskAttachment.FileData : taskAttachment.FileData;

                await _context.SaveChangesAsync();

                return TaskAttachment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteTaskAttachmentAsync(TaskAttachment TaskAttachment)
        {
            try
            {
                _context.TaskAttachments.Remove(TaskAttachment);
                await _context.SaveChangesAsync();

                return TaskAttachment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTaskAttachmentsByTaskIdAsync(List<TaskAttachment> TaskAttachments)
        {
            try
            {
                _context.TaskAttachments.RemoveRange(TaskAttachments);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
