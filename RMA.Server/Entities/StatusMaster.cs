using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMA.Server.Entities
{
    public class StatusMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string StatusName { get; set; } = string.Empty; // Chờ duyệt, Đã nhận sửa...

        [MaxLength(20)]
        public string? ColorCode { get; set; } // Mã màu Hex: #FF0000...

        // Navigation properties
        public ICollection<RmaTicket> RmaTickets { get; set; } = new List<RmaTicket>();
    }
}
