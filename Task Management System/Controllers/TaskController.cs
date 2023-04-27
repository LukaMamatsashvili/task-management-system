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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _TaskService;

        public TaskController(ITaskService TaskService)
        {
            _TaskService = TaskService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<TasksResponse> GetTasks()
        {
            var response = await _TaskService.GetTasks();

            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<TaskResponse> GetTaskById(int id)
        {
            var response = await _TaskService.GetTaskById(id);

            return response;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> AddTask([FromBody] TaskDTO TaskDTO)
        {
            var response = await _TaskService.AddTask(TaskDTO);

            return response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> UpdateTask([FromBody] TaskDTO updateTaskDTO)
        {
            var response = await _TaskService.UpdateTask(updateTaskDTO);

            return response;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response> DeleteTask(int id)
        {
            var response = await _TaskService.DeleteTask(id);

            return response;
        }
    }
}
