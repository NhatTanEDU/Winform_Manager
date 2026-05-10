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
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
        {
            var devices = await _context.Devices
                .Include(d => d.Customer)
                .Include(d => d.Model)
                .Select(d => new DeviceDto
                {
                    Id = d.Id,
                    SerialNumber = d.SerialNumber,
                    CustomerId = d.CustomerId,
                    CustomerName = d.Customer.Name,
                    ModelId = d.ModelId,
                    ModelName = d.Model.ModelName,
                    Brand = d.Model.Brand,
                    PurchaseDate = d.PurchaseDate,
                    WarrantyExpiry = d.WarrantyExpiry
                })
                .ToListAsync();

            return Ok(devices);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> PostDevice(DeviceCreateDto dto)
        {
            var device = new Device
            {
                SerialNumber = dto.SerialNumber,
                CustomerId = dto.CustomerId,
                ModelId = dto.ModelId,
                PurchaseDate = dto.PurchaseDate,
                WarrantyExpiry = dto.WarrantyExpiry
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevices", new { id = device.Id }, dto);
        }
    }
}
