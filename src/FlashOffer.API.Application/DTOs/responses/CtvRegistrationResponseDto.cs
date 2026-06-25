// CtvRegistrationResponseDto.cs
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.responses;

public class CtvRegistrationResponseDto : IMapFrom<CtvRegistration>
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Zalo { get; set; }
	public string? Email { get; set; }
	public string? SalesChannel { get; set; }
	public string? Experience { get; set; }
	public bool IsApproved { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? ApprovedAt { get; set; }

	public void Mapping(Profile profile)
		=> profile.CreateMap<CtvRegistration, CtvRegistrationResponseDto>();
}