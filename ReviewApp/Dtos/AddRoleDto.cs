﻿using System.ComponentModel.DataAnnotations;

namespace ReviewApp.Dtos
{
    public class AddRoleDto
    {
        [Required]
        public string  UserId { get; set; }
        [Required]
        public string  Role { get; set; }
    }
}
