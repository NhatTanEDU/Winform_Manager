using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMA.Server.Data;
using RMA.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReferenceDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<IEnumerable<StatusMasterDto>>> GetStatuses()
        {
            var statuses = await _context.StatusMasters
                .Select(s => new StatusMasterDto { Id = s.Id, StatusName = s.StatusName, ColorCode = s.ColorCode })
                .ToListAsync();
            return Ok(statuses);
        }

        [HttpGet("vendors")]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendors()
        {
            var vendors = await _context.Vendors
                .Select(v => new VendorDto { Id = v.Id, Name = v.Name })
                .ToListAsync();
            return Ok(vendors);
        }

        [HttpGet("models")]
        public async Task<ActionResult<IEnumerable<ModelDto>>> GetModels()
        {
            var models = await _context.Models
                .Select(m => new ModelDto { Id = m.Id, ModelName = m.ModelName, Brand = m.Brand, CategoryId = m.CategoryId })
                .ToListAsync();
            return Ok(models);
        }
    }
}
