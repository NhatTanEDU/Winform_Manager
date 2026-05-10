using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMA.Server.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // PC, Laptop, UPS, Printer...

        // Navigation properties
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
