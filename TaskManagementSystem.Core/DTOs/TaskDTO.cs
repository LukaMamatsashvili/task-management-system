using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public int AssignedUserId { get; set; }
        public int CreatorId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string? ShortDescription { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public List<TaskAttachmentDTO>? AttachedFiles { get; set; }
    }
}
