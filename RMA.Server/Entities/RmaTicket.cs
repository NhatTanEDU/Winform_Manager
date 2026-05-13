using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class RmaTicket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        public Device Device { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [Required]
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusMaster StatusMaster { get; set; } = null!;

        public int? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor? Vendor { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ProblemDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ServiceMode { get; set; } // Warranty hoặc Repair

        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        public DateTime? SentDate { get; set; } // Ngày gửi đi - Mốc 14 ngày

        public bool IsUrgent { get; set; } = false;

        [MaxLength(2000)]
        public string? StaffNote { get; set; }

        // Navigation properties
        public ComponentChecklist? ComponentChecklist { get; set; }
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
    }
}
