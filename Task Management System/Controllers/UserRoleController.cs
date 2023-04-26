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
    [Authorize(Roles = "admin")]
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
            var userRolesResponse = await _userRoleService.GetUserRoles();
            return userRolesResponse;
        }

        [HttpGet("{id}")]
        public async Task<UserRoleResponse> GetUserRoleById(int id)
        {
            var UserRoleResponse = await _userRoleService.GetUserRoleById(id);

            return UserRoleResponse;
        }

        [HttpPost]
        public async Task<Response> AddUserRole([FromBody] UserRoleDTO UserRoleDTO)
        {
            var Response = await _userRoleService.AddUserRole(UserRoleDTO);

            return Response;
        }

        [HttpPut]
        public async Task<Response> UpdateUserRole([FromBody] UserRoleDTO updateUserRoleDTO)
        {
            var Response = await _userRoleService.UpdateUserRole(updateUserRoleDTO);

            return Response;
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteUserRole(int id)
        {
            var Response = await _userRoleService.DeleteUserRole(id);

            return Response;
        }
    }
}
