using System.Net.Http.Json;
using RMA.Shared.DTOs;

namespace RMA.Client.Services;

public class CustomerService
{
    private readonly HttpClient _http;
    private static readonly List<CustomerDto> _mockData = new()
    {
        new CustomerDto { Id = 1, Name = "Nguyen Van A", Phone = "0901234567", Email = "a@example.com", Address = "Hanoi", CreatedAt = DateTime.Now.AddDays(-10) },
        new CustomerDto { Id = 2, Name = "Tran Thi B", Phone = "0987654321", Email = "b@example.com", Address = "HCMC", CreatedAt = DateTime.Now.AddDays(-5) }
    };
    private static int _nextId = 3;

    public CustomerService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        await Task.Delay(100); // Simulate network delay
        return _mockData.ToList();
    }

    public async Task<CustomerDto?> GetCustomerAsync(int id)
    {
        await Task.Delay(100);
        return _mockData.FirstOrDefault(c => c.Id == id);
    }

    public async Task<CustomerDto?> CreateCustomerAsync(CustomerDto customer)
    {
        await Task.Delay(100);
        customer.Id = _nextId++;
        customer.CreatedAt = DateTime.Now;
        _mockData.Add(customer);
        return customer;
    }

    public async Task<bool> UpdateCustomerAsync(int id, CustomerDto customer)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(c => c.Id == id);
        if (existing != null)
        {
            existing.Name = customer.Name;
            existing.Phone = customer.Phone;
            existing.Email = customer.Email;
            existing.Address = customer.Address;
            existing.AvatarUrl = customer.AvatarUrl;
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        await Task.Delay(100);
        var existing = _mockData.FirstOrDefault(c => c.Id == id);
        if (existing != null)
        {
            _mockData.Remove(existing);
            return true;
        }
        return false;
    }
}
