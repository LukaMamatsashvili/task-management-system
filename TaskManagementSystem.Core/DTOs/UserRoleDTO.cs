using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class UserRoleDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
    }
}
