using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class RmaTicketService
{
    private readonly HttpClient _http;

    public RmaTicketService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<RmaTicketDto>> GetRmaTicketsAsync()
    {
        return await _http.GetFromJsonAsync<List<RmaTicketDto>>("api/rmatickets") ?? new List<RmaTicketDto>();
    }

    public async Task<RmaTicketDto?> GetRmaTicketAsync(int id)
    {
        return await _http.GetFromJsonAsync<RmaTicketDto>($"api/rmatickets/{id}");
    }

    public async Task<RmaTicketDto?> CreateRmaTicketAsync(RmaTicketDto ticket)
    {
        var response = await _http.PostAsJsonAsync("api/rmatickets", ticket);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<RmaTicketDto>();
        }
        return null;
    }

    public async Task<bool> UpdateRmaTicketAsync(int id, RmaTicketDto ticket)
    {
        var response = await _http.PutAsJsonAsync($"api/rmatickets/{id}", ticket);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteRmaTicketAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/rmatickets/{id}");
        return response.IsSuccessStatusCode;
    }
    
    // Additional methods for lookups
    public async Task<List<StatusMasterDto>> GetStatusesAsync()
    {
        return await _http.GetFromJsonAsync<List<StatusMasterDto>>("api/referencedata/statuses") ?? new List<StatusMasterDto>();
    }

    public async Task<List<VendorDto>> GetVendorsAsync()
    {
        return await _http.GetFromJsonAsync<List<VendorDto>>("api/referencedata/vendors") ?? new List<VendorDto>();
    }

    public async Task<List<ModelDto>> GetModelsAsync()
    {
        return await _http.GetFromJsonAsync<List<ModelDto>>("api/referencedata/models") ?? new List<ModelDto>();
    }
}
