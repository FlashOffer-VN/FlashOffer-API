// Application/DTOs/requests/TokenRequest.cs
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.Application.DTOs.requests;

public class TokenRequest
{
	[FromHeader(Name = "Authorization")]
	public string Authorization { get; set; } = string.Empty;

	public string GetToken()
	{
		return Authorization?.Replace("Bearer ", "") ?? string.Empty;
	}
}