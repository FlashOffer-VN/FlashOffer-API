// CtvRegistrationResponseDto.cs
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using AutoMapper;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class CtvRegistrationResponseDto : IMapFrom<Collaborator>
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Zalo { get; set; }
	public string? Email { get; set; }
	public SalesChannel? SalesChannel { get; set; }
	public string? Experience { get; set; }
	public bool IsApproved { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? ApprovedAt { get; set; }

	public void Mapping(Profile profile)
		=> profile.CreateMap<Collaborator, CtvRegistrationResponseDto>();
}