using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Infrastructure.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UserRoleId { get; set; }
        [MaxLength(20)]
        public string Username { get; set; }
        [MaxLength(64)]
        public byte[] PasswordHash { get; set; }

        [MaxLength(128)]
        public byte[] PasswordSalt { get; set; }
    }
}
