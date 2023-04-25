using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Core.Interfaces;

namespace TaskManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _PermissionService;

        public PermissionController(IPermissionService PermissionService)
        {
            _PermissionService = PermissionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionDTO>>> GetPermissions()
        {
            var Permissions = await _PermissionService.GetPermissions();
            return Ok(Permissions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermissionById(int id)
        {
            var Permission = await _PermissionService.GetPermissionById(id);

            if (Permission == null)
            {
                return NotFound();
            }

            return Ok(Permission);
        }

        [HttpPost]
        public async Task<ActionResult/*<PermissionDTO>*/> AddPermission([FromBody] PermissionDTO PermissionDTO)
        {
            var result = await _PermissionService.AddPermission(PermissionDTO);

            //return CreatedAtAction(nameof(GetPermissionById), new { id = result.Id }, task);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult/*<PermissionDTO>*/> UpdatePermission(int id, [FromBody] PermissionDTO updatePermissionDTO)
        {
            updatePermissionDTO.Id = id;
            var task = await _PermissionService.UpdatePermission(updatePermissionDTO);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePermission(int id)
        {
            var success = await _PermissionService.DeletePermission(id);

            return NoContent();
        }
    }
}
