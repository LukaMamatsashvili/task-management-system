using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Interfaces
{
    public interface IUserAuthorizationService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}
