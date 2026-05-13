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
    public class RmaTicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RmaTicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RmaTicketDto>>> GetRmaTickets()
        {
            var tickets = await _context.RmaTickets
                .Include(r => r.Device).ThenInclude(d => d.Model)
                .Include(r => r.Customer)
                .Include(r => r.StatusMaster)
                .Include(r => r.Vendor)
                .Select(r => new RmaTicketDto
                {
                    Id = r.Id,
                    DeviceId = r.DeviceId,
                    DeviceSerialNumber = r.Device.SerialNumber,
                    DeviceModelName = r.Device.Model.ModelName,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer.Name,
                    CustomerPhone = r.Customer.Phone,
                    StatusId = r.StatusId,
                    StatusName = r.StatusMaster.StatusName,
                    StatusColorCode = r.StatusMaster.ColorCode,
                    VendorId = r.VendorId,
                    VendorName = r.Vendor != null ? r.Vendor.Name : null,
                    ProblemDescription = r.ProblemDescription,
                    ServiceMode = r.ServiceMode,
                    ReceivedDate = r.ReceivedDate,
                    SentDate = r.SentDate,
                    IsUrgent = r.IsUrgent,
                    StaffNote = r.StaffNote
                })
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpPost]
        public async Task<ActionResult<RmaTicketDto>> PostRmaTicket(RmaTicketCreateDto dto)
        {
            var ticket = new RmaTicket
            {
                DeviceId = dto.DeviceId,
                CustomerId = dto.CustomerId,
                StatusId = dto.StatusId,
                VendorId = dto.VendorId,
                ProblemDescription = dto.ProblemDescription,
                ServiceMode = dto.ServiceMode,
                IsUrgent = dto.IsUrgent,
                StaffNote = dto.StaffNote,
                ReceivedDate = System.DateTime.UtcNow
            };

            _context.RmaTickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRmaTickets), new { id = ticket.Id }, dto);
        }
    }
}
