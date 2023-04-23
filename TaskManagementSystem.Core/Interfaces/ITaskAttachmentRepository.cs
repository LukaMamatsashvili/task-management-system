using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface ITaskAttachmentRepository
    {
        Task<List<TaskAttachment>> GetTaskAttachmentsAsync();
        Task<TaskAttachment> GetTaskAttachmentByIdAsync(int id);
        Task<int> AddTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task UpdateTaskAttachmentAsync(TaskAttachment TaskAttachment);
        Task DeleteTaskAttachmentAsync(int id);
    }
}
