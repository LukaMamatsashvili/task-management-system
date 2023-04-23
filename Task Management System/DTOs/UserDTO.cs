namespace Task_Management_System.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRoleDTO UserRole { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
