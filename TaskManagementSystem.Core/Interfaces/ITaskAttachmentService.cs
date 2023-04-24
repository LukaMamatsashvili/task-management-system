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
    public interface ITaskAttachmentService
    {
        Task<List<TaskAttachmentDTO>> GetTaskAttachments();
        Task<TaskAttachmentDTO> GetTaskAttachmentById(int id);
        Task<string> AddTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO);
        Task<string> UpdateTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO);
        Task<string> DeleteTaskAttachment(int id);
    }
}
