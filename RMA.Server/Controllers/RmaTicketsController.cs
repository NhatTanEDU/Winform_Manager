using Microsoft.AspNetCore.Mvc;

namespace RMA.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RmaTicketsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("RmaTickets API đang chờ chuyển đổi sang Firestore");
}
