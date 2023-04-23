using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetUserRolesAsync();
        Task<UserRole> GetUserRoleByIdAsync(int id);
        Task<int> AddUserRoleAsync(UserRole UserRole);
        Task UpdateUserRoleAsync(UserRole UserRole);
        Task DeleteUserRoleAsync(int id);
    }
}
