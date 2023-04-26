using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface ITaskAttachmentService
    {
        Task<TaskAttachmentsResponse> GetTaskAttachments();
        Task<TaskAttachmentsResponse> GetTaskAttachmentsByTaskId(int taskId);
        Task<TaskAttachmentResponse> GetTaskAttachmentById(int id);
        Task<Response> AddTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO);
        Task<Response> UpdateTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO);
        Task<Response> DeleteTaskAttachment(int id);
        Task<Response> DeleteTaskAttachmentsByTaskId(int taskId);
    }
}
