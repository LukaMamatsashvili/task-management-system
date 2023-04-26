using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Common;
using TaskManagementSystem.Core.DTOs;
using TaskManagementSystem.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IUserService
    {
        Task<UsersResponse> GetUsers();
        Task<UserResponse> GetUserById(int id);
        Task<UserResponse> GetUserByUsername(string username);
        Task<Response> AddUser(UserDTO UserDTO);
        Task<Response> UpdateUser(UserDTO UserDTO);
        Task<Response> DeleteUser(int id);
    }
}
