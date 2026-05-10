namespace RMA.Shared.DTOs
{
    public class StatusMasterDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? ColorCode { get; set; }
    }

    public class VendorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ModelDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public int CategoryId { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
    }
}
