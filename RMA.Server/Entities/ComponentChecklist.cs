using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class ComponentChecklist
    {
        [Key]
        public int RmaTicketId { get; set; }

        [ForeignKey(nameof(RmaTicketId))]
        public RmaTicket RmaTicket { get; set; } = null!;

        public bool HasAdapter { get; set; }
        public bool HasPowerCable { get; set; }
        public bool HasBattery { get; set; }

        [MaxLength(500)]
        public string? OtherAccessories { get; set; }
    }
}
