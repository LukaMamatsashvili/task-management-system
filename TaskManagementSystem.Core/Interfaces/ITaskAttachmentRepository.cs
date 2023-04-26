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
        Task<List<TaskAttachment>> GetTaskAttachmentsByTaskIdAsync(int taskId);
        Task<TaskAttachment> GetTaskAttachmentByIdAsync(int id);
        Task<int> AddTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task<int> UpdateTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task<int> DeleteTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task DeleteTaskAttachmentsByTaskIdAsync(List<TaskAttachment> TaskAttachment);
    }
}
