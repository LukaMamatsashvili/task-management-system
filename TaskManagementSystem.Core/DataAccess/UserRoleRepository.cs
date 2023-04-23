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
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly TaskManagementDbContext _context;

        public UserRoleRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserRole>> GetUserRolesAsync()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task<UserRole> GetUserRoleByIdAsync(int id)
        {
            var UserRole = await _context.UserRoles.FirstOrDefaultAsync(t => t.Id == id);

            if (UserRole == null)
            {
                throw new NotFoundException($"User role with ID '{id}' not found.");
            }

            return UserRole;
        }

        public async Task<int> AddUserRoleAsync(UserRole UserRole)
        {
            await _context.UserRoles.AddAsync(UserRole);
            await _context.SaveChangesAsync();

            return UserRole.Id;
        }

        public async Task UpdateUserRoleAsync(UserRole UserRole)
        {
            _context.Entry(UserRole).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserRoleAsync(int id)
        {
            var UserRole = await _context.UserRoles.FindAsync(id);

            if (UserRole == null)
            {
                throw new NotFoundException($"User role with ID '{id}' not found.");
            }

            _context.UserRoles.Remove(UserRole);
            await _context.SaveChangesAsync();
        }
    }
}
