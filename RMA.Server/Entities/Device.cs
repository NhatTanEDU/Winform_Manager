using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [Required]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public Model Model { get; set; } = null!;

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiry { get; set; }

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
