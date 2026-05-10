using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class DeviceService
{
    private readonly HttpClient _http;

    public DeviceService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DeviceDto>> GetDevicesAsync()
    {
        return await _http.GetFromJsonAsync<List<DeviceDto>>("api/devices") ?? new List<DeviceDto>();
    }

    public async Task<DeviceDto?> GetDeviceAsync(int id)
    {
        return await _http.GetFromJsonAsync<DeviceDto>($"api/devices/{id}");
    }

    public async Task<DeviceDto?> CreateDeviceAsync(DeviceDto device)
    {
        var response = await _http.PostAsJsonAsync("api/devices", device);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DeviceDto>();
        }
        return null;
    }

    public async Task<bool> UpdateDeviceAsync(int id, DeviceDto device)
    {
        var response = await _http.PutAsJsonAsync($"api/devices/{id}", device);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/devices/{id}");
        return response.IsSuccessStatusCode;
    }
}
