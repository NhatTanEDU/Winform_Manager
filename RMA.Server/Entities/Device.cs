using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class Device
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [FirestoreProperty]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [FirestoreProperty]
        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [Required]
        [FirestoreProperty]
        public string ModelId { get; set; } = string.Empty;

        [ForeignKey(nameof(ModelId))]
        public Model Model { get; set; } = null!;

        [FirestoreProperty]
        public DateTime? PurchaseDate { get; set; }

        [FirestoreProperty]
        public DateTime? WarrantyExpiry { get; set; }

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
