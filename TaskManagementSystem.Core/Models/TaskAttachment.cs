﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Models
{
    internal class TaskAttachment
    {   
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
        public int TaskId { get; set; }
    }
}
