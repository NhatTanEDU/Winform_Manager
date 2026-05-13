using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMA.Server.Entities
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // Dell, HP, ASUS...

        [MaxLength(500)]
        public string? ContactInfo { get; set; } // Hotline, địa chỉ

        [MaxLength(500)]
        public string? WarrantyLink { get; set; } // Link tra cứu S/N hãng

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
