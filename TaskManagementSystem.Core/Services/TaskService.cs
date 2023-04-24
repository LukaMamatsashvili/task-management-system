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
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskAttachmentService _taskAttachmentService;

        public TaskService(ITaskRepository taskRepository, ITaskAttachmentService taskAttachmentService)
        {
            _taskRepository = taskRepository;
            _taskAttachmentService = taskAttachmentService;
        }

        public async Task<List<TaskDTO>> GetTasks()
        {
            var Tasks = await _taskRepository.GetTasksAsync();

            var TaskDTOs = new List<TaskDTO>();

            if (Tasks == null || Tasks?.Count == 0)
                return TaskDTOs;

            foreach(var Task in Tasks)
            {
                var TaskDTO = new TaskDTO
                {
                    Id = Task.Id,
                    AssignedUserId = Task.AssignedUserId,
                    CreatorId = Task.CreatorId,
                    Title = Task.Title,
                    ShortDescription = Task.ShortDescription,
                    Description = Task.Description,
                };
                TaskDTO.AttachedFiles = await _taskAttachmentService.GetTaskAttachmentsByTaskId(TaskDTO.Id);

                TaskDTOs.Add(TaskDTO);
            };

            return TaskDTOs;
        }

        public async Task<TaskDTO> GetTaskById(int id)
        {
            var Task = await _taskRepository.GetTaskByIdAsync(id);

            if (Task == null)
                return new TaskDTO();

            var TaskDTO = new TaskDTO
            {
                Id = Task.Id,
                AssignedUserId = Task.AssignedUserId,
                CreatorId = Task.CreatorId,
                Title = Task.Title,
                ShortDescription = Task.ShortDescription,
                Description = Task.Description,
            };
            TaskDTO.AttachedFiles = await _taskAttachmentService.GetTaskAttachmentsByTaskId(TaskDTO.Id);

            return TaskDTO;
        }

        public async Task<string> AddTask(TaskDTO TaskDTO)
        {
            if (TaskDTO == null)
                throw new Exception("Task is null!");

            if (TaskDTO.AssignedUserId == null)
                throw new Exception("Task assigned user Id is null!");

            if (TaskDTO.CreatorId == null)
                throw new Exception("Task creator Id is null!");

            if (TaskDTO.Title == null)
                throw new Exception("Task title is null!");

            var Task = new Infrastructure.Models.Task
            {
                AssignedUserId = TaskDTO.AssignedUserId,
                CreatorId = TaskDTO.CreatorId,
                Title = TaskDTO.Title,
                ShortDescription = TaskDTO.ShortDescription,
                Description = TaskDTO.Description,
            };

            int taskId = await _taskRepository.AddTaskAsync(Task);

            if (TaskDTO.AttachedFiles != null)
            {
                foreach (var AttachedFile in TaskDTO.AttachedFiles)
                {
                    AttachedFile.TaskId = taskId;
                    await _taskAttachmentService.AddTaskAttachment(AttachedFile);
                }
            }

            return "Successful addition!";
        }

        public async Task<string> UpdateTask(TaskDTO TaskDTO)
        {
            if (TaskDTO == null)
                throw new AppException("Task is null!");

            var Task = await _taskRepository.GetTaskByIdAsync(TaskDTO.Id);

            if (Task == null)
                throw new AppException("Task not found!");

            if (Task.AssignedUserId != TaskDTO.AssignedUserId)
                Task.AssignedUserId = TaskDTO.AssignedUserId;

            if (Task.Title != TaskDTO.Title)
                Task.Title = TaskDTO.Title;

            if (TaskDTO.ShortDescription != null)
                Task.ShortDescription = TaskDTO.ShortDescription;

            if (TaskDTO.Description != null)
                Task.Description = TaskDTO.Description;

            if (TaskDTO.AttachedFiles != null)
            {
                await _taskAttachmentService.DeleteTaskAttachmentsByTaskId(TaskDTO.Id);

                foreach (var file in TaskDTO.AttachedFiles)
                {
                    file.TaskId = TaskDTO.Id;
                    await _taskAttachmentService.AddTaskAttachment(file);
                }
            }

            await _taskRepository.UpdateTaskAsync(Task);

            return "Successful update!";
        }

        public async Task<string> DeleteTask(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);

            return "Successful deletion!";
        }
    }
}
