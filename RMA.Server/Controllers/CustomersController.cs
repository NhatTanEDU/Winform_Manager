using Microsoft.AspNetCore.Mvc;

namespace RMA.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    // Tạm thời vô hiệu hóa để chuyển đổi sang Firestore
    [HttpGet]
    public IActionResult Get() => Ok("Customer API đang chờ chuyển đổi sang Firestore");
}
