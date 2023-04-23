using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.Models;
using Task = TaskManagementSystem.Core.Models.Task;

namespace TaskManagementSystem.Infrastructure
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext()
        {
        }
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-V34LAC9;Database=TaskManagementSystemDB;Trusted_Connection=True;TrustServerCertificate=true");
            }
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermissionLink> UserPermissionLinks { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
    }
}
