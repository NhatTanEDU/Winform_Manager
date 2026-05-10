using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMA.Server.Entities
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RmaTicketId { get; set; }

        [ForeignKey(nameof(RmaTicketId))]
        public RmaTicket RmaTicket { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string FileUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FileType { get; set; } = string.Empty; // SN_PHOTO, CONDITION_PHOTO

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
