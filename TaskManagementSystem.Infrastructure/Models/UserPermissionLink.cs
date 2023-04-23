using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Infrastructure.Models
{
    public class UserPermissionLink
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
}
