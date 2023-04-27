using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;

namespace TaskManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskAttachmentController : ControllerBase
    {
        private readonly ITaskAttachmentService _TaskAttachmentService;

        public TaskAttachmentController(ITaskAttachmentService TaskAttachmentService)
        {
            _TaskAttachmentService = TaskAttachmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<TaskAttachmentsResponse> GetTaskAttachments()
        {
            var response = await _TaskAttachmentService.GetTaskAttachments();

            return response;
        }

        [HttpGet("taskId/{taskId}")]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<TaskAttachmentsResponse> GetTaskAttachmentsByTaskId(int taskId)
        {
            var response = await _TaskAttachmentService.GetTaskAttachmentsByTaskId(taskId);

            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<TaskAttachmentResponse> GetTaskAttachmentById(int id)
        {
            var response = await _TaskAttachmentService.GetTaskAttachmentById(id);

            return response;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> AddTaskAttachment([FromBody] TaskAttachmentDTO TaskAttachmentDTO)
        {
            var response = await _TaskAttachmentService.AddTaskAttachment(TaskAttachmentDTO);

            return response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> UpdateTaskAttachment([FromBody] TaskAttachmentDTO updateTaskAttachmentDTO)
        {
            var response = await _TaskAttachmentService.UpdateTaskAttachment(updateTaskAttachmentDTO);

            return response;
        }

        [HttpDelete("id/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> DeleteTaskAttachment(int id)
        {
            var response = await _TaskAttachmentService.DeleteTaskAttachment(id);

            return response;
        }

        [HttpDelete("taskId/{taskId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> DeleteTaskAttachmentsByTaskId(int taskId)
        {
            var response = await _TaskAttachmentService.DeleteTaskAttachmentsByTaskId(taskId);

            return response;
        }
    }
}
