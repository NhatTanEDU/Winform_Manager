using System;

namespace RMA.Shared.DTOs
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
    }

    public class DeviceCreateDto
    {
        public string SerialNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int ModelId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
    }
}
