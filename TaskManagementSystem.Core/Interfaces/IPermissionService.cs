using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDTO>> GetPermissions();
        Task<PermissionDTO> GetPermissionById(int id);
        Task<string> AddPermission(PermissionDTO PermissionDTO);
        Task<string> UpdatePermission(PermissionDTO PermissionDTO);
        Task<string> DeletePermission(int id);
    }
}
