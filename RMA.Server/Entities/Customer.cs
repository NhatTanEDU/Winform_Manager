using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMA.Server.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Device> Devices { get; set; } = new List<Device>();
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
