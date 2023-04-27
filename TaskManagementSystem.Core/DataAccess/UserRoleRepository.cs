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
            try
            {
                return await _context.UserRoles.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRole> GetUserRoleByIdAsync(int id)
        {
            try
            {
                var UserRole = await _context.UserRoles.FirstOrDefaultAsync(t => t.Id == id);

                return UserRole;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRole> GetUserRoleByTypeAsync(string type)
        {
            try
            {
                var UserRole = await _context.UserRoles.FirstOrDefaultAsync(t => t.Type == type);

                return UserRole;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddUserRoleAsync(UserRole UserRole)
        {
            try
            {
                await _context.UserRoles.AddAsync(UserRole);
                await _context.SaveChangesAsync();

                return UserRole.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateUserRoleAsync(UserRole UserRole)
        {
            try
            {
                (await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Id == UserRole.Id)).Type = UserRole.Type;
                await _context.SaveChangesAsync();

                return UserRole.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUserRoleAsync(UserRole UserRole)
        {
            try
            {
                _context.UserRoles.Remove(UserRole);
                await _context.SaveChangesAsync();

                return UserRole.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
