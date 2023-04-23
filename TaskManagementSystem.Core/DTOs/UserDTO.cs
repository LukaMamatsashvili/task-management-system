namespace TaskManagementSystem.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public UserRoleDTO UserRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
