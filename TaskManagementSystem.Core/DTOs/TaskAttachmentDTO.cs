namespace TaskManagementSystem.Core.DTOs
{
    public class TaskAttachmentDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string? FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
    }
}
