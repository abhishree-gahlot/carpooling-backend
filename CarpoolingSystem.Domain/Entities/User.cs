using System;
using System.Collections.Generic;
using System.Text;
using CarpoolingSystem.Domain.Enums;

namespace CarpoolingSystem.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public required string EmailId { get; set; } 
        public required string UserName { get; set; }
        public UserRole UserRole { get; set; }
        public string Pin { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
