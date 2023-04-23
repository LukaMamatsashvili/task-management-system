namespace TaskManagementSystem.Core.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public int AssignedUserId { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public List<TaskAttachmentDTO>? AttachedFiles { get; set; }
    }
}
