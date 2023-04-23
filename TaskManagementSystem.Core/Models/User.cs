using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
