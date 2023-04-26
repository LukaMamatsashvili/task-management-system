using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<TaskAttachmentsResponse> GetTaskAttachments()
        {
            var response = await _TaskAttachmentService.GetTaskAttachments();

            return response;
        }

        [HttpGet("taskid/{taskid}")]
        public async Task<TaskAttachmentsResponse> GetTaskAttachmentsByTaskId(int taskId)
        {
            var response = await _TaskAttachmentService.GetTaskAttachmentsByTaskId(taskId);

            return response;
        }

        [HttpGet("{id}")]
        public async Task<TaskAttachmentResponse> GetTaskAttachmentById(int id)
        {
            var response = await _TaskAttachmentService.GetTaskAttachmentById(id);

            return response;
        }

        [HttpPost]
        public async Task<Response> AddTaskAttachment([FromBody] TaskAttachmentDTO TaskAttachmentDTO)
        {
            var response = await _TaskAttachmentService.AddTaskAttachment(TaskAttachmentDTO);

            return response;
        }

        [HttpPut]
        public async Task<Response> UpdateTaskAttachment([FromBody] TaskAttachmentDTO updateTaskAttachmentDTO)
        {
            var response = await _TaskAttachmentService.UpdateTaskAttachment(updateTaskAttachmentDTO);

            return response;
        }

        [HttpDelete("id/{id}")]
        public async Task<Response> DeleteTaskAttachment(int id)
        {
            var response = await _TaskAttachmentService.DeleteTaskAttachment(id);

            return response;
        }

        [HttpDelete("taskid/{taskid}")]
        public async Task<Response> DeleteTaskAttachmentsByTaskId(int taskId)
        {
            var response = await _TaskAttachmentService.DeleteTaskAttachmentsByTaskId(taskId);

            return response;
        }
    }
}
