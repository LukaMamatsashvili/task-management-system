using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.DataAccess
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly TaskManagementDbContext _context;

        public PermissionRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByIdAsync(int id)
        {
            var Permissions = await _context.Permissions.Where(t => t.Id == id).ToListAsync();

            if (Permissions == null)
            {
                throw new NotFoundException($"Permissions with ID '{id}' not found.");
            }

            return Permissions;
        }

        public async Task<int> AddPermissionAsync(Permission Permission)
        {
            await _context.Permissions.AddAsync(Permission);
            await _context.SaveChangesAsync();

            return Permission.Id;
        }

        public async Task UpdatePermissionAsync(Permission Permission)
        {
            _context.Entry(Permission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(int id)
        {
            var Permission = await _context.Permissions.FindAsync(id);

            if (Permission == null)
            {
                throw new NotFoundException($"Permission with ID '{id}' not found.");
            }

            _context.Permissions.Remove(Permission);
            await _context.SaveChangesAsync();
        }
    }
}
