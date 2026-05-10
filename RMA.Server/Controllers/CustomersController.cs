using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMA.Server.Data;
using RMA.Server.Entities;
using RMA.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    Address = c.Address,
                    AvatarUrl = c.AvatarUrl,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var c = await _context.Customers.FindAsync(id);

            if (c == null)
            {
                return NotFound();
            }

            return new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                AvatarUrl = c.AvatarUrl,
                CreatedAt = c.CreatedAt
            };
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerCreateDto customerDto)
        {
            var customer = new Customer
            {
                Name = customerDto.Name,
                Phone = customerDto.Phone,
                Email = customerDto.Email,
                Address = customerDto.Address,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var resultDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, resultDto);
        }
    }
}
