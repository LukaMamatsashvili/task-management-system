using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Services
{
    public class TaskAttachmentService : ITaskAttachmentService
    {
        private readonly ITaskAttachmentRepository _taskAttachmentRepository;

        public TaskAttachmentService(ITaskAttachmentRepository taskAttachmentRepository)
        {
            _taskAttachmentRepository = taskAttachmentRepository;
        }

        public async Task<List<TaskAttachmentDTO>> GetTaskAttachments()
        {
            var taskAttachments = await _taskAttachmentRepository.GetTaskAttachmentsAsync();

            var TaskAttachmentDTOs = new List<TaskAttachmentDTO>();

            if (taskAttachments == null || taskAttachments?.Count == 0)
                return TaskAttachmentDTOs;

            TaskAttachmentDTOs = taskAttachments.Select(taskAttachment => new TaskAttachmentDTO
            {
                Id = taskAttachment.Id,
                TaskId = taskAttachment.TaskId,
                FileName = taskAttachment.FileName,
                ContentType = taskAttachment.ContentType,
                FileData = taskAttachment.FileData,
            }).ToList();

            return TaskAttachmentDTOs;
        }

        public async Task<TaskAttachmentDTO> GetTaskAttachmentById(int id)
        {
            var TaskAttachment = await _taskAttachmentRepository.GetTaskAttachmentByIdAsync(id);

            if (TaskAttachment == null)
                return new TaskAttachmentDTO();

            var TaskAttachmentDTO = new TaskAttachmentDTO
            {
                Id = TaskAttachment.Id,
                TaskId = TaskAttachment.TaskId,
                FileName = TaskAttachment.FileName,
                ContentType = TaskAttachment.ContentType,
                FileData = TaskAttachment.FileData,
            };

            return TaskAttachmentDTO;
        }

        public async Task<string> AddTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO)
        {
            if (TaskAttachmentDTO == null)
                throw new Exception("Task attachment is null!");

            if (TaskAttachmentDTO.TaskId == null)
                throw new Exception("TaskId of task attachment type is null!");

            if (TaskAttachmentDTO.ContentType == null)
                throw new Exception("ContentType of task attachment type is null!");

            if (TaskAttachmentDTO.FileData == null)
                throw new Exception("File data of task attachment type is null!");

            var TaskAttachment = new TaskAttachment
            {
                TaskId = TaskAttachmentDTO.TaskId,
                FileName = TaskAttachmentDTO.FileName,
                ContentType = TaskAttachmentDTO.ContentType,
                FileData = TaskAttachmentDTO.FileData,
            };

            await _taskAttachmentRepository.AddTaskAttachmentAsync(TaskAttachment);

            return "Successful addition!"; ;
        }

        public async Task<string> UpdateTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO)
        {
            if (TaskAttachmentDTO == null)
                throw new AppException("Task attachment is null!");

            var TaskAttachment = await _taskAttachmentRepository.GetTaskAttachmentByIdAsync(TaskAttachmentDTO.Id);

            if (TaskAttachment == null)
                throw new AppException("Task attachment not found!");

            if (TaskAttachment.TaskId != TaskAttachmentDTO.TaskId)
                TaskAttachment.TaskId = TaskAttachmentDTO.TaskId;

            if (TaskAttachmentDTO.FileName != null)
                TaskAttachment.FileName = TaskAttachmentDTO.FileName;

            if (TaskAttachment.ContentType != TaskAttachmentDTO.ContentType)
                TaskAttachment.ContentType = TaskAttachmentDTO.ContentType;

            if (TaskAttachment.FileData != TaskAttachmentDTO.FileData)
                TaskAttachment.FileData = TaskAttachmentDTO.FileData;

            await _taskAttachmentRepository.UpdateTaskAttachmentAsync(TaskAttachment);

            return "Successful update!";
        }

        public async Task<string> DeleteTaskAttachment(int id)
        {
            await _taskAttachmentRepository.DeleteTaskAttachmentAsync(id);

            return "Successful deletion!";
        }
    }
}
