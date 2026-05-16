using Microsoft.AspNetCore.Mvc;

namespace RMA.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Device API đang chờ chuyển đổi sang Firestore");
}
