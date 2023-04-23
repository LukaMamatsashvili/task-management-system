using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        public UserRoleDTO UserRole { get; set; }
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
