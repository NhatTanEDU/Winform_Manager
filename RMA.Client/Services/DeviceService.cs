using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class DeviceService
{
    private readonly HttpClient _http;
    private static readonly List<DeviceDto> _mockData = new()
    {
        new DeviceDto { Id = 1, SerialNumber = "SN-1001", CustomerId = 1, CustomerName = "Nguyen Van A", ModelId = 1, ModelName = "Dell XPS 15", Brand = "Dell", PurchaseDate = DateTime.Now.AddYears(-1), WarrantyExpiry = DateTime.Now.AddYears(1) },
        new DeviceDto { Id = 2, SerialNumber = "SN-1002", CustomerId = 2, CustomerName = "Tran Thi B", ModelId = 2, ModelName = "MacBook Pro 14", Brand = "Apple", PurchaseDate = DateTime.Now.AddMonths(-6), WarrantyExpiry = DateTime.Now.AddMonths(6) }
    };
    private static int _nextId = 3;

    public DeviceService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DeviceDto>> GetDevicesAsync()
    {
        await Task.Delay(100);
        return _mockData.ToList();
    }

    public async Task<DeviceDto?> GetDeviceAsync(int id)
    {
        await Task.Delay(100);
        return _mockData.FirstOrDefault(d => d.Id == id);
    }

    public async Task<DeviceDto?> CreateDeviceAsync(DeviceDto device)
    {
        await Task.Delay(100);
        device.Id = _nextId++;
        _mockData.Add(device);
        return device;
    }

    public async Task<bool> UpdateDeviceAsync(int id, DeviceDto device)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(d => d.Id == id);
        if (existing != null)
        {
            existing.SerialNumber = device.SerialNumber;
            existing.CustomerId = device.CustomerId;
            existing.CustomerName = device.CustomerName;
            existing.ModelId = device.ModelId;
            existing.ModelName = device.ModelName;
            existing.Brand = device.Brand;
            existing.PurchaseDate = device.PurchaseDate;
            existing.WarrantyExpiry = device.WarrantyExpiry;
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(d => d.Id == id);
        if (existing != null)
        {
            _mockData.Remove(existing);
            return true;
        }
        return false;
    }
}
