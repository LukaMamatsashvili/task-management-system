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
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        public async Task<UsersResponse> GetUsers()
        {
            var response = await _UserService.GetUsers();

            return response;
        }

        [HttpGet("id/{id}")]
        public async Task<UserResponse> GetUserById(int id)
        {
            var response = await _UserService.GetUserById(id);

            return response;
        }

        [HttpGet("username/{username}")]
        public async Task<UserResponse> GetUserByUsername(string username)
        {
            var response = await _UserService.GetUserByUsername(username);

            return response;
        }

        [HttpPost]
        public async Task<Response> AddUser([FromBody] UserDTO UserDTO)
        {
            var response = await _UserService.AddUser(UserDTO);

            return response;
        }

        [HttpPut]
        public async Task<Response> UpdateUser([FromBody] UserDTO updateUserDTO)
        {
            var response = await _UserService.UpdateUser(updateUserDTO);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteUser(int id)
        {
            var response = await _UserService.DeleteUser(id);

            return response;
        }
    }
}
