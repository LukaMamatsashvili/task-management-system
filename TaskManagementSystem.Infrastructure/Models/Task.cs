using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Infrastructure.Models
{
    public class Task
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
    }
}
