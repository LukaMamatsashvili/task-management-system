using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Services;
using TaskManagementSystem.Infrastructure;
using TaskManagementSystem.Infrastructure.Settings;

namespace TaskManagementSystem.Shared.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITaskAttachmentRepository, TaskAttachmentRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskAttachmentService, TaskAttachmentService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
        }

        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskManagementDbContext>();
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        }
    }
}   
