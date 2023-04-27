using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IUserService _userService;

        public TaskService(ITaskRepository taskRepository, ITaskAttachmentService taskAttachmentService, IUserService userService)
        {
            _taskRepository = taskRepository;
            _taskAttachmentService = taskAttachmentService;
            _userService = userService;
        }

        public async Task<TasksResponse> GetTasks()
        {
            var response = new TasksResponse();
            try
            {
                var Tasks = await _taskRepository.GetTasksAsync();

                var TaskDTOs = new List<TaskDTO>();

                if (Tasks == null || Tasks?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Tasks Not Found!";

                    return response;
                }

                foreach (var Task in Tasks)
                {
                    var TaskDTO = new TaskDTO
                    {
                        Id = Task.Id,
                        CreatorId = Task.CreatorId,
                        AssignedUserId = Task.AssignedUserId,
                        Title = Task.Title,
                        ShortDescription = Task.ShortDescription,
                        Description = Task.Description,
                    };
                    TaskDTO.AttachedFiles = (await _taskAttachmentService.GetTaskAttachmentsByTaskId(TaskDTO.Id)).TaskAttachments;

                    TaskDTOs.Add(TaskDTO);
                };

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Tasks = TaskDTOs;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<TaskResponse> GetTaskById(int id)
        {
            var response = new TaskResponse();
            try
            {
                var Task = await _taskRepository.GetTaskByIdAsync(id);

                if (Task == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Not Found!";

                    return response;
                }

                var TaskDTO = new TaskDTO
                {
                    Id = Task.Id,
                    CreatorId = Task.CreatorId,
                    AssignedUserId = Task.AssignedUserId,
                    Title = Task.Title,
                    ShortDescription = Task.ShortDescription,
                    Description = Task.Description,
                };
                TaskDTO.AttachedFiles = (await _taskAttachmentService.GetTaskAttachmentsByTaskId(TaskDTO.Id)).TaskAttachments;

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Task = TaskDTO;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> AddTask(TaskDTO TaskDTO)
        {
            var response = new Response();
            try
            {
                if (TaskDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task is Required!";

                    return response;
                }

                if (TaskDTO.CreatorId == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Creator Id is Required!";

                    return response;
                }

                var creatorInDb = (await _userService.GetUserById((int)TaskDTO.CreatorId)).User;
                if (creatorInDb == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Creator Not Found!";

                    return response;
                }

                if (TaskDTO.AssignedUserId == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Assigned User is Required!";

                    return response;
                }

                var assignedUserInDb = (await _userService.GetUserById((int)TaskDTO.AssignedUserId)).User;
                if (assignedUserInDb == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Assigned User Not Found!";

                    return response;
                }

                if (TaskDTO.CreatorId == TaskDTO.AssignedUserId)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Creator and Assigned User can not be the Same!";

                    return response;
                }

                if (TaskDTO.Title == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Title is Required!";

                    return response;
                }

                var Task = new Infrastructure.Models.Task
                {
                    CreatorId = (int)TaskDTO.CreatorId,
                    AssignedUserId = (int)TaskDTO.AssignedUserId,
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

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Addition!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> UpdateTask(TaskDTO TaskDTO)
        {
            var response = new Response();
            var taskRequest = new Infrastructure.Models.Task();
            taskRequest.Id = TaskDTO.Id;

            try
            {
                if (TaskDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task is null!";

                    return response;
                }

                var Task = await _taskRepository.GetTaskByIdAsync(TaskDTO.Id);
                if (Task == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Not Found!";

                    return response;
                }

                if (TaskDTO.AssignedUserId != null)
                {
                    if (TaskDTO.AssignedUserId > 0 && Task.AssignedUserId != TaskDTO.AssignedUserId)
                    {
                        var assignedUserInDb = (await _userService.GetUserById((int)TaskDTO.AssignedUserId)).User;
                        if (assignedUserInDb == null)
                        {
                            response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                            response.Message = "Task Assigned User Not Found!";

                            return response;
                        }

                        taskRequest.AssignedUserId = (int)TaskDTO.AssignedUserId;
                    }
                }

                if (Task.CreatorId == TaskDTO.AssignedUserId)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Creator and Assigned User can not be the Same!";

                    return response;
                }

                if (!string.IsNullOrWhiteSpace(TaskDTO.Title) && Task.Title != TaskDTO.Title)
                    taskRequest.Title = TaskDTO.Title;

                if (!string.IsNullOrWhiteSpace(TaskDTO.ShortDescription) && Task.ShortDescription != TaskDTO.ShortDescription)
                    taskRequest.ShortDescription = TaskDTO.ShortDescription;

                if (!string.IsNullOrWhiteSpace(TaskDTO.Description) && Task.Description != TaskDTO.Description)
                    taskRequest.Description = TaskDTO.Description;

                if (TaskDTO.AttachedFiles != null)
                {
                    await _taskAttachmentService.DeleteTaskAttachmentsByTaskId(TaskDTO.Id);

                    foreach (var file in TaskDTO.AttachedFiles)
                    {
                        file.TaskId = TaskDTO.Id;
                        await _taskAttachmentService.AddTaskAttachment(file);
                    }
                }

                await _taskRepository.UpdateTaskAsync(taskRequest);

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Update!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> DeleteTask(int id)
        {
            var response = new Response();
            try
            {
                var Task = await _taskRepository.GetTaskByIdAsync(id);

                if (Task == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Not Found!";

                    return response;
                }

                await _taskRepository.DeleteTaskAsync(Task);
                await _taskAttachmentService.DeleteTaskAttachmentsByTaskId(id);

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful Deletion!";

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }
    }
}
