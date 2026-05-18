using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMA.Server.Controllers;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Tài khoản và mật khẩu mặc định để test
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("RMA_SongLinh_SecretKey_For_Local_Testing_Only_12345");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "admin"),
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "RMAServer",
                Audience = "RMAServer",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                message = "Đăng nhập thành công với Local JWT (Dùng cho API Test)."
            });
        }

        return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });
    }

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestAuth()
    {
        var userName = User.Identity?.Name ?? "Unknown";
        return Ok(new { message = $"Bạn đã truy cập thành công! User: {userName}" });
    }
}
