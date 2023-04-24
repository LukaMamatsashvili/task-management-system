using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IUserPermissionLinkRepository
    {
        Task<List<UserPermissionLink>> GetUserPermissionLinksAsync();
        Task<List<UserPermissionLink>> GetUserPermissionLinksByIdAsync(int id);
        Task<int> AddUserPermissionLinkAsync(UserPermissionLink UserPermissionLink);
        Task UpdateUserPermissionLinkAsync(UserPermissionLink UserPermissionLink);
        Task DeleteUserPermissionLinkAsync(int id);
    }
}
