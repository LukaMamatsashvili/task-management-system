using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Services;

namespace TaskManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserRoleDTO>>> GetUserRoles()
        {
            var userRoles = await _userRoleService.GetUserRoles();
            return Ok(userRoles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRoleDTO>> GetUserRoleById(int id)
        {
            var UserRole = await _userRoleService.GetUserRoleById(id);

            if (UserRole == null)
            {
                return NotFound();
            }

            return Ok(UserRole);
        }

        [HttpPost]
        public async Task<ActionResult/*<UserRoleDTO>*/> AddUserRole([FromBody] UserRoleDTO UserRoleDTO)
        {
            var result = await _userRoleService.AddUserRole(UserRoleDTO);

            //return CreatedAtAction(nameof(GetUserRoleById), new { id = result.Id }, task);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult/*<UserRoleDTO>*/> UpdateUserRole(int id, [FromBody] UserRoleDTO updateUserRoleDTO)
        {
            updateUserRoleDTO.Id = id;
            var task = await _userRoleService.UpdateUserRole(updateUserRoleDTO);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id)
        {
            var success = await _userRoleService.DeleteUserRole(id);

            return NoContent();
        }
    }
}
