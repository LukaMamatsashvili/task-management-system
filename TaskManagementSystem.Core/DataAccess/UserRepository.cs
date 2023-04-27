using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementDbContext _context;

        public UserRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var User = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);

                return User;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                var User = await _context.Users.FirstOrDefaultAsync(t => t.Username == username);

                return User;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddUserAsync(User User)
        {
            try
            {
                await _context.Users.AddAsync(User);
                await _context.SaveChangesAsync();

                return User.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateUserAsync(User User)
        {
            try
            {
                var user = (await _context.Users.FirstOrDefaultAsync(user => user.Id == User.Id));

                user.UserRoleId = User.UserRoleId > 0 ? User.UserRoleId : user.UserRoleId;
                user.Username = !string.IsNullOrEmpty(User.Username) ? User.Username : user.Username;
                user.PasswordHash = User.PasswordHash != null ? User.PasswordHash : user.PasswordHash;
                user.PasswordSalt = User.PasswordSalt != null ? User.PasswordSalt : user.PasswordSalt;

                await _context.SaveChangesAsync();

                return User.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteUserAsync(User User)
        {
            try
            {
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();

                return User.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
