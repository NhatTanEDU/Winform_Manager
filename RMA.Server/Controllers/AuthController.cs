using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RMA.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("test")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return Ok(new { message = "Bạn đã truy cập thành công bằng Firebase JWT!" });
    }
}
