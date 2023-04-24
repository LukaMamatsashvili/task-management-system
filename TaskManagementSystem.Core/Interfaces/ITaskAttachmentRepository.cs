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
    public interface ITaskAttachmentRepository
    {
        Task<List<TaskAttachment>> GetTaskAttachmentsAsync();
        Task<TaskAttachment> GetTaskAttachmentByIdAsync(int id);
        Task<List<TaskAttachment>> GetTaskAttachmentsByTaskIdAsync(int taskId);
        Task<int> AddTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task UpdateTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task DeleteTaskAttachmentAsync(int id);
        Task DeleteTaskAttachmentsByTaskIdAsync(int taskId);
    }
}
