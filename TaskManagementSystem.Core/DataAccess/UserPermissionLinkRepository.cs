﻿using Microsoft.EntityFrameworkCore;
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
    public class UserPermissionLinkRepository : IUserPermissionLinkRepository
    {
        private readonly TaskManagementDbContext _context;

        public UserPermissionLinkRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserPermissionLink>> GetUserPermissionLinksAsync()
        {
            return await _context.UserPermissionLinks.ToListAsync();
        }

        public async Task<List<UserPermissionLink>> GetUserPermissionLinksByIdAsync(int id)
        {
            var UserPermissionLinks = await _context.UserPermissionLinks.Where(t => t.Id == id).ToListAsync();

            if (UserPermissionLinks == null)
            {
                throw new NotFoundException($"User Permission Links with ID '{id}' not found.");
            }

            return UserPermissionLinks;
        }

        public async Task<int> AddUserPermissionLinkAsync(UserPermissionLink UserPermissionLink)
        {
            await _context.UserPermissionLinks.AddAsync(UserPermissionLink);
            await _context.SaveChangesAsync();

            return UserPermissionLink.Id;
        }

        public async Task UpdateUserPermissionLinkAsync(UserPermissionLink UserPermissionLink)
        {
            _context.Entry(UserPermissionLink).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserPermissionLinkAsync(int id)
        {
            var UserPermissionLink = await _context.UserPermissionLinks.FindAsync(id);

            if (UserPermissionLink == null)
            {
                throw new NotFoundException($"User role with ID '{id}' not found.");
            }

            _context.UserPermissionLinks.Remove(UserPermissionLink);
            await _context.SaveChangesAsync();
        }
    }
}
