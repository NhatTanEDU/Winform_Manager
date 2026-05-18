using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class RmaTicket
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [FirestoreProperty]
        public string DeviceId { get; set; } = string.Empty;

        [ForeignKey(nameof(DeviceId))]
        public Device Device { get; set; } = null!;

        [Required]
        [FirestoreProperty]
        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [Required]
        [FirestoreProperty]
        public string StatusId { get; set; } = string.Empty;

        [ForeignKey(nameof(StatusId))]
        public StatusMaster StatusMaster { get; set; } = null!;

        [FirestoreProperty]
        public string? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor? Vendor { get; set; }

        [Required]
        [MaxLength(2000)]
        [FirestoreProperty]
        public string ProblemDescription { get; set; } = string.Empty;

        [MaxLength(100)]
        [FirestoreProperty]
        public string? ServiceMode { get; set; } // Warranty hoặc Repair

        [FirestoreProperty]
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public DateTime? SentDate { get; set; } // Ngày gửi đi - Mốc 14 ngày

        [FirestoreProperty]
        public bool IsUrgent { get; set; } = false;

        [MaxLength(2000)]
        [FirestoreProperty]
        public string? StaffNote { get; set; }

        // Navigation properties
        public ComponentChecklist? ComponentChecklist { get; set; }
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
    }
}
