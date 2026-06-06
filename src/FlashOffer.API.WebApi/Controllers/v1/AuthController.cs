using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs;
using FlashOffer.API.Infrastructure.Configurations;

namespace FlashOffer.API.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
    {
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // TODO: Replace with actual user validation from database
        if (request.Username == "admin" && request.Password == "password")
        {
            var roles = new List<string> { "Admin", "User" };
            var token = _jwtService.GenerateToken("1", request.Username, roles);
            
            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Username = request.Username
            };
            
            return Ok(response);
        }

        return Unauthorized("Invalid username or password");
    }
}
