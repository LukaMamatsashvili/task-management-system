using System;
using System.Collections.Generic;
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
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
    }
}
