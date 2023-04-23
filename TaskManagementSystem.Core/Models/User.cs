using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UserRoleId { get; set; }
        public string Username { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
