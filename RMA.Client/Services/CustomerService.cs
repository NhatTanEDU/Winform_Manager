using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class CustomerService
{
    private readonly HttpClient _http;

    public CustomerService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        return await _http.GetFromJsonAsync<List<CustomerDto>>("api/customers") ?? new List<CustomerDto>();
    }

    public async Task<CustomerDto?> GetCustomerAsync(int id)
    {
        return await _http.GetFromJsonAsync<CustomerDto>($"api/customers/{id}");
    }

    public async Task<CustomerDto?> CreateCustomerAsync(CustomerDto customer)
    {
        var response = await _http.PostAsJsonAsync("api/customers", customer);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerDto>();
        }
        return null;
    }

    public async Task<bool> UpdateCustomerAsync(int id, CustomerDto customer)
    {
        var response = await _http.PutAsJsonAsync($"api/customers/{id}", customer);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/customers/{id}");
        return response.IsSuccessStatusCode;
    }
}
