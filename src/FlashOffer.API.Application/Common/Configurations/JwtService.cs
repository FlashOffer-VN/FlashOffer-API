using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlashOffer.API.Application.Common.Configurations;

public class JwtService : IJwtService
{
	private readonly JwtSettings _jwtSettings;

	public JwtService(IOptions<JwtSettings> jwtSettings)
	{
		_jwtSettings = jwtSettings.Value;
	}

	public string GenerateToken(string userId, string username, IEnumerable<string> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userId),
			new Claim(ClaimTypes.Name, username),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(JwtRegisteredClaimNames.Email, username)
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public ClaimsPrincipal? ValidateToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

		try
		{
			var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidIssuer = _jwtSettings.Issuer,
				ValidateAudience = true,
				ValidAudience = _jwtSettings.Audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			}, out _);

			return principal;
		}
		catch
		{
			return null;
		}
	}
}
