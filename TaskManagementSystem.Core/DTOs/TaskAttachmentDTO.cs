using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class TaskAttachmentDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        [MaxLength(100)]
        public string? FileName { get; set; }
        [Required]
        [MaxLength(30)]
        public string ContentType { get; set; }
        [Required]
        public byte[] FileData { get; set; }
    }
}
