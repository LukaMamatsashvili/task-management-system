using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public UserRoleDTO? UserRole { get; set; }
        [MaxLength(20)]
        public string? Username { get; set; }
        [MaxLength(20)]
        public string? Password { get; set; }
    }
}
