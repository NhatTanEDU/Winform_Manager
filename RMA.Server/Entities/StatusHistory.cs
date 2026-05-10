using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class StatusHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RmaTicketId { get; set; }

        [ForeignKey(nameof(RmaTicketId))]
        public RmaTicket RmaTicket { get; set; } = null!;

        public int? LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Location? Location { get; set; }

        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}
