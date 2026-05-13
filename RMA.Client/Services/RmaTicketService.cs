using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class RmaTicketService
{
    private readonly HttpClient _http;
    private static readonly List<RmaTicketDto> _mockData = new()
    {
        new RmaTicketDto { Id = 1, DeviceId = 1, DeviceSerialNumber = "SN-1001", DeviceModelName = "Dell XPS 15", CustomerId = 1, CustomerName = "Nguyen Van A", CustomerPhone = "0901234567", StatusId = 1, StatusName = "New", StatusColorCode = "Blue", VendorId = null, VendorName = null, ProblemDescription = "Screen flickering", ServiceMode = "Carry-In", ReceivedDate = DateTime.Now.AddDays(-2), IsUrgent = false, StaffNote = "" },
        new RmaTicketDto { Id = 2, DeviceId = 2, DeviceSerialNumber = "SN-1002", DeviceModelName = "MacBook Pro 14", CustomerId = 2, CustomerName = "Tran Thi B", CustomerPhone = "0987654321", StatusId = 2, StatusName = "In Progress", StatusColorCode = "Orange", VendorId = 1, VendorName = "Apple Service", ProblemDescription = "Battery not charging", ServiceMode = "Pickup", ReceivedDate = DateTime.Now.AddDays(-1), IsUrgent = true, StaffNote = "Sent to vendor" }
    };
    private static int _nextId = 3;

    private static readonly List<StatusMasterDto> _mockStatuses = new()
    {
        new StatusMasterDto { Id = 1, StatusName = "New", ColorCode = "Blue" },
        new StatusMasterDto { Id = 2, StatusName = "In Progress", ColorCode = "Orange" },
        new StatusMasterDto { Id = 3, StatusName = "Completed", ColorCode = "Green" }
    };

    private static readonly List<VendorDto> _mockVendors = new()
    {
        new VendorDto { Id = 1, Name = "Apple Service" },
        new VendorDto { Id = 2, Name = "Dell Service" }
    };

    private static readonly List<ModelDto> _mockModels = new()
    {
        new ModelDto { Id = 1, ModelName = "Dell XPS 15", Brand = "Dell", CategoryId = 1 },
        new ModelDto { Id = 2, ModelName = "MacBook Pro 14", Brand = "Apple", CategoryId = 1 }
    };

    public RmaTicketService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RmaTicketDto>> GetRmaTicketsAsync()
    {
        await Task.Delay(100);
        return _mockData.ToList();
    }

    public async Task<RmaTicketDto?> GetRmaTicketAsync(int id)
    {
        await Task.Delay(100);
        return _mockData.FirstOrDefault(t => t.Id == id);
    }

    public async Task<RmaTicketDto?> CreateRmaTicketAsync(RmaTicketDto ticket)
    {
        await Task.Delay(100);
        ticket.Id = _nextId++;
        ticket.ReceivedDate = DateTime.Now;
        _mockData.Add(ticket);
        return ticket;
    }

    public async Task<bool> UpdateRmaTicketAsync(int id, RmaTicketDto ticket)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(t => t.Id == id);
        if (existing != null)
        {
            existing.DeviceId = ticket.DeviceId;
            existing.CustomerId = ticket.CustomerId;
            existing.StatusId = ticket.StatusId;
            existing.VendorId = ticket.VendorId;
            existing.ProblemDescription = ticket.ProblemDescription;
            existing.ServiceMode = ticket.ServiceMode;
            existing.IsUrgent = ticket.IsUrgent;
            existing.StaffNote = ticket.StaffNote;
            // Also need to mock some relationships ideally, but this is simple mock
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteRmaTicketAsync(int id)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(t => t.Id == id);
        if (existing != null)
        {
            _mockData.Remove(existing);
            return true;
        }
        return false;
    }
    
    // Additional methods for lookups
    public async Task<List<StatusMasterDto>> GetStatusesAsync()
    {
        await Task.Delay(100);
        return _mockStatuses.ToList();
    }

    public async Task<List<VendorDto>> GetVendorsAsync()
    {
        await Task.Delay(100);
        return _mockVendors.ToList();
    }

    public async Task<List<ModelDto>> GetModelsAsync()
    {
        await Task.Delay(100);
        return _mockModels.ToList();
    }
}
