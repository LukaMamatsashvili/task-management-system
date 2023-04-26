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
    public interface IUserAuthorizationService
    {
        Task<Response> RegisterUser(UserDTO UserDTO);
        Task<TokenResponse> AuthorizeUser(UserDTO UserDTO);
    }
}
