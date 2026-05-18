using Microsoft.AspNetCore.Mvc;
using RMA.Server.Entities;
using RMA.Server.Services;
using RMA.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReferenceDataController : ControllerBase
{
    private readonly FirestoreRepository<Vendor> _vendorRepo;
    private readonly FirestoreRepository<Model> _modelRepo;

    public ReferenceDataController(FirestoreRepository<Vendor> vendorRepo, FirestoreRepository<Model> modelRepo)
    {
        _vendorRepo = vendorRepo;
        _modelRepo = modelRepo;
    }

    [HttpGet("vendors")]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendors()
    {
        var vendors = await _vendorRepo.GetAllAsync();
        var dtos = vendors.Select(v => new VendorDto
        {
            Id = v.Id,
            Name = v.Name
        });
        return Ok(dtos);
    }

    [HttpPost("vendors")]
    public async Task<ActionResult<VendorDto>> PostVendor([FromBody] VendorDto dto)
    {
        var entity = new Vendor { Name = dto.Name };
        var newId = await _vendorRepo.AddAsync(entity);
        return CreatedAtAction(nameof(GetVendors), null, new VendorDto { Id = newId, Name = entity.Name });
    }

    [HttpPut("vendors/{id}")]
    public async Task<IActionResult> PutVendor(string id, [FromBody] VendorDto dto)
    {
        var entity = await _vendorRepo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        entity.Name = dto.Name;
        await _vendorRepo.UpdateAsync(id, entity);
        return NoContent();
    }

    [HttpDelete("vendors/{id}")]
    public async Task<IActionResult> DeleteVendor(string id)
    {
        await _vendorRepo.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("models")]
    public async Task<ActionResult<IEnumerable<ModelDto>>> GetModels()
    {
        var models = await _modelRepo.GetAllAsync();
        var dtos = models.Select(m => new ModelDto
        {
            Id = m.Id,
            ModelName = m.ModelName,
            Brand = m.Brand,
            CategoryId = m.CategoryId
        });
        return Ok(dtos);
    }
}
