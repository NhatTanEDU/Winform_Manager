using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [MaxLength(100)]
        public string? Brand { get; set; }

        [Required]
        [MaxLength(255)]
        public string ModelName { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
