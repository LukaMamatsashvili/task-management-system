using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(int id);
        Task<int> AddPermissionAsync(Permission Permission);
        Task UpdatePermissionAsync(Permission Permission);
        Task DeletePermissionAsync(int id);
    }
}
