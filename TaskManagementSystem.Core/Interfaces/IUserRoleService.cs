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
    public interface IUserRoleService
    {
        Task<List<UserRoleDTO>> GetUserRoles();
        Task<UserRoleDTO> GetUserRoleById(int id);
        Task<string> AddUserRole(UserRoleDTO UserRoleDTO);
        Task<string> UpdateUserRole(UserRoleDTO UserRoleDTO);
        Task<string> DeleteUserRole(int id);
    }
}
