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
        Task<List<PermissionDTO>> GetPermissionsById(int id);
        Task<int> AddPermission(PermissionDTO PermissionDTO);
        Task<string> UpdatePermission(PermissionDTO PermissionDTO);
        Task<string> DeletePermission(int id);
    }
}
