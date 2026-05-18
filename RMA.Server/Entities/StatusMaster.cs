using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class StatusMaster
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [FirestoreProperty]
        public string StatusName { get; set; } = string.Empty; // Chờ duyệt, Đã nhận sửa...

        [MaxLength(20)]
        [FirestoreProperty]
        public string? ColorCode { get; set; } // Mã màu Hex: #FF0000...

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
