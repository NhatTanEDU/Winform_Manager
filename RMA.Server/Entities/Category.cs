using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace RMA.Server.Entities
{
    [FirestoreData]
    public class Category
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [FirestoreProperty]
        public string Name { get; set; } = string.Empty; // PC, Laptop, UPS, Printer...

        // Navigation properties
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
