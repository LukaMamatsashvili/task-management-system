﻿using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Core.DTOs
{
    public class UserRoleDTO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
    }
}
