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
    public class TaskAttachmentService : ITaskAttachmentService
    {
        private readonly ITaskAttachmentRepository _taskAttachmentRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskAttachmentService(ITaskAttachmentRepository taskAttachmentRepository, ITaskRepository taskRepository)
        {
            _taskAttachmentRepository = taskAttachmentRepository;
            _taskRepository = taskRepository;
        }

        public async Task<TaskAttachmentsResponse> GetTaskAttachments()
        {
            var response = new TaskAttachmentsResponse();
            try
            {
                var taskAttachments = await _taskAttachmentRepository.GetTaskAttachmentsAsync();

                var TaskAttachmentDTOs = new List<TaskAttachmentDTO>();

                if (taskAttachments == null || taskAttachments?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachments Not Found!";

                    return response;
                }

                TaskAttachmentDTOs = taskAttachments.Select(taskAttachment => new TaskAttachmentDTO
                {
                    Id = taskAttachment.Id,
                    TaskId = taskAttachment.TaskId,
                    FileName = taskAttachment.FileName,
                    ContentType = taskAttachment.ContentType,
                    FileData = taskAttachment.FileData,
                }).ToList();

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.TaskAttachments = TaskAttachmentDTOs;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<TaskAttachmentsResponse> GetTaskAttachmentsByTaskId(int taskId)
        {
            var response = new TaskAttachmentsResponse();
            try
            {
                var taskAttachments = await _taskAttachmentRepository.GetTaskAttachmentsByTaskIdAsync(taskId);

                var TaskAttachmentDTOs = new List<TaskAttachmentDTO>();

                if (taskAttachments == null || taskAttachments?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachments Not Found!";

                    return response;
                }

                TaskAttachmentDTOs = taskAttachments.Select(taskAttachment => new TaskAttachmentDTO
                {
                    Id = taskAttachment.Id,
                    TaskId = taskAttachment.TaskId,
                    FileName = taskAttachment.FileName,
                    ContentType = taskAttachment.ContentType,
                    FileData = taskAttachment.FileData,
                }).ToList();

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.TaskAttachments = TaskAttachmentDTOs;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<TaskAttachmentResponse> GetTaskAttachmentById(int id)
        {
            var response = new TaskAttachmentResponse();
            try
            {
                var TaskAttachment = await _taskAttachmentRepository.GetTaskAttachmentByIdAsync(id);

                if (TaskAttachment == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachment Not Found!";

                    return response;
                }

                var TaskAttachmentDTO = new TaskAttachmentDTO
                {
                    Id = TaskAttachment.Id,
                    TaskId = TaskAttachment.TaskId,
                    FileName = TaskAttachment.FileName,
                    ContentType = TaskAttachment.ContentType,
                    FileData = TaskAttachment.FileData,
                };

                response.ResponseMessage.StatusCode = HttpStatusCode.OK;
                response.TaskAttachment = TaskAttachmentDTO;

                return response;
            }
            catch (Exception ex)
            {
                response.ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<Response> AddTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO)
        {
            var response = new Response();
            try
            {
                if (TaskAttachmentDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Attachment is Required!";

                    return response;
                }

                if (TaskAttachmentDTO.TaskId == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Id is Required!";

                    return response;
                }

                var taskInDb = await _taskRepository.GetTaskByIdAsync((int)TaskAttachmentDTO.TaskId);
                if (taskInDb == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Not Found!";

                    return response;
                }

                if (TaskAttachmentDTO.ContentType == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Content Type is Required!";

                    return response;
                }

                if (TaskAttachmentDTO.FileData == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "File Data is Required!";

                    return response;
                }

                var TaskAttachment = new TaskAttachment
                {
                    TaskId = (int)TaskAttachmentDTO.TaskId,
                    FileName = TaskAttachmentDTO.FileName,
                    ContentType = TaskAttachmentDTO.ContentType,
                    FileData = TaskAttachmentDTO.FileData,
                };

                await _taskAttachmentRepository.AddTaskAttachmentAsync(TaskAttachment);

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

        public async Task<Response> UpdateTaskAttachment(TaskAttachmentDTO TaskAttachmentDTO)
        {
            var response = new Response();
            var taskAttachmentRequest = new TaskAttachment();
            taskAttachmentRequest.Id = TaskAttachmentDTO.Id;

            try
            {
                if (TaskAttachmentDTO == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Task Attachment is null!";

                    return response;
                }

                var TaskAttachment = await _taskAttachmentRepository.GetTaskAttachmentByIdAsync(TaskAttachmentDTO.Id);

                if (TaskAttachment == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachment Not Found!";

                    return response;
                }

                if (TaskAttachmentDTO.TaskId != null)
                {
                    if (TaskAttachmentDTO.TaskId > 0 && TaskAttachment.TaskId != TaskAttachmentDTO.TaskId)
                    {
                        var taskInDb = await _taskRepository.GetTaskByIdAsync((int)TaskAttachmentDTO.TaskId);
                        if (taskInDb == null)
                        {
                            response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                            response.Message = "Task Not Found!";

                            return response;
                        }

                        taskAttachmentRequest.TaskId = (int)TaskAttachmentDTO.TaskId;
                    }
                }

                if (!string.IsNullOrWhiteSpace(TaskAttachmentDTO.FileName) && TaskAttachment.FileName != TaskAttachmentDTO.FileName)
                    taskAttachmentRequest.FileName = TaskAttachmentDTO.FileName;

                if (!string.IsNullOrWhiteSpace(TaskAttachmentDTO.ContentType) && TaskAttachment.ContentType != TaskAttachmentDTO.ContentType)
                    taskAttachmentRequest.ContentType = TaskAttachmentDTO.ContentType;

                if (TaskAttachmentDTO.FileData != null && TaskAttachment.FileData != TaskAttachmentDTO.FileData)
                    taskAttachmentRequest.FileData = TaskAttachmentDTO.FileData;

                await _taskAttachmentRepository.UpdateTaskAttachmentAsync(taskAttachmentRequest);

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

        public async Task<Response> DeleteTaskAttachment(int id)
        {
            var response = new Response();
            try
            {
                var TaskAttachment = await _taskAttachmentRepository.GetTaskAttachmentByIdAsync(id);

                if (TaskAttachment == null)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachment Not Found!";

                    return response;
                }

                await _taskAttachmentRepository.DeleteTaskAttachmentAsync(TaskAttachment);

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

        public async Task<Response> DeleteTaskAttachmentsByTaskId(int taskId)
        {
            var response = new Response();
            try
            {
                var TaskAttachments = await _taskAttachmentRepository.GetTaskAttachmentsByTaskIdAsync(taskId);

                if (TaskAttachments == null || TaskAttachments?.Count == 0)
                {
                    response.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Task Attachments Not Found!";

                    return response;
                }

                await _taskAttachmentRepository.DeleteTaskAttachmentsByTaskIdAsync(TaskAttachments);

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
