using System;

namespace RMA.Shared.DTOs
{
    public class RmaTicketDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string DeviceSerialNumber { get; set; } = string.Empty;
        public string DeviceModelName { get; set; } = string.Empty;
        
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? StatusColorCode { get; set; }
        
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        
        public string ProblemDescription { get; set; } = string.Empty;
        public string? ServiceMode { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsUrgent { get; set; }
        public string? StaffNote { get; set; }
    }

    public class RmaTicketCreateDto
    {
        public int DeviceId { get; set; }
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public int? VendorId { get; set; }
        public string ProblemDescription { get; set; } = string.Empty;
        public string? ServiceMode { get; set; }
        public bool IsUrgent { get; set; }
        public string? StaffNote { get; set; }
    }

    public class RmaTicketUpdateStatusDto
    {
        public int StatusId { get; set; }
        public int LocationId { get; set; }
        public string? Note { get; set; }
    }
}
