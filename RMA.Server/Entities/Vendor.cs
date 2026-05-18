using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class Vendor
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [FirestoreProperty]
        public string Name { get; set; } = string.Empty; // Dell, HP, ASUS...

        [MaxLength(500)]
        [FirestoreProperty]
        public string? ContactInfo { get; set; } // Hotline, địa chỉ

        [MaxLength(500)]
        [FirestoreProperty]
        public string? WarrantyLink { get; set; } // Link tra cứu S/N hãng

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
