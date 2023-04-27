using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Services;

namespace TaskManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<UserRolesResponse> GetUserRoles()
        {
            var response = await _userRoleService.GetUserRoles();

            return response;
        }

        [HttpGet("{id}")]
        public async Task<UserRoleResponse> GetUserRoleById(int id)
        {
            var response = await _userRoleService.GetUserRoleById(id);

            return response;
        }

        [HttpPost]
        public async Task<Response> AddUserRole([FromBody] UserRoleDTO UserRoleDTO)
        {
            var response = await _userRoleService.AddUserRole(UserRoleDTO);

            return response;
        }

        [HttpPut]
        public async Task<Response> UpdateUserRole([FromBody] UserRoleDTO updateUserRoleDTO)
        {
            var response = await _userRoleService.UpdateUserRole(updateUserRoleDTO);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteUserRole(int id)
        {
            var response = await _userRoleService.DeleteUserRole(id);

            return response;
        }
    }
}
