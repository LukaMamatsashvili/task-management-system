using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IUserRoleService
    {
        Task<UserRolesResponse> GetUserRoles();
        Task<UserRoleResponse> GetUserRoleById(int id);
        Task<Response> AddUserRole(UserRoleDTO UserRoleDTO);
        Task<Response> UpdateUserRole(UserRoleDTO UserRoleDTO);
        Task<Response> DeleteUserRole(int id);
    }
}
