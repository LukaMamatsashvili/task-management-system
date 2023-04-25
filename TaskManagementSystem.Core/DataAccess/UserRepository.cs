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
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementDbContext _context;

        public UserRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var User = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);

            if (User == null)
            {
                throw new NotFoundException($"User with ID '{id}' not found.");
            }

            return User;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var User = await _context.Users.FirstOrDefaultAsync(t => t.Username == username);

            return User;
        }

        public async Task<int> AddUserAsync(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();

            return User.Id;
        }

        public async Task UpdateUserAsync(User User)
        {
            _context.Entry(User).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                throw new NotFoundException($"User with ID '{id}' not found.");
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
        }
    }
}
