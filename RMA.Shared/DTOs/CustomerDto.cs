using System;
using System.Collections.Generic;

namespace RMA.Shared.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CustomerCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
