using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class Location
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [FirestoreProperty]
        public string Name { get; set; } = string.Empty; // Tại CTy, Tại Hãng, Kho A...

        // Navigation properties
        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
    }
}
