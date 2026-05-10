using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMA.Server.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // Tại CTy, Tại Hãng, Kho A...

        // Navigation properties
        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
    }
}
