using Microsoft.AspNetCore.Mvc;

namespace RMA.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReferenceDataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("ReferenceData API đang chờ chuyển đổi sang Firestore");
}
