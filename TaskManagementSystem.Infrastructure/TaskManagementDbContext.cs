using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskManagementSystem.Infrastructure.Models;
using TaskManagementSystem.Infrastructure.Settings;
using Task = TaskManagementSystem.Infrastructure.Models.Task;

namespace TaskManagementSystem.Infrastructure
{
    public class TaskManagementDbContext : DbContext
    {
        private readonly string _connectionSettings;
        public TaskManagementDbContext()
        {
        }
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options, IOptions<ConnectionStrings> connectionSettings)
            : base(options)
        {
            _connectionSettings = connectionSettings.Value.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionSettings);
            }
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
    }
}
