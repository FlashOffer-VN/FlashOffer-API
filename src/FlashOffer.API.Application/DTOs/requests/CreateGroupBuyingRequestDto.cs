using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.requests;

public class CreateGroupBuyingRequestDto : IMapFrom<GroupBuyingRequest>
{
	public string ProductName { get; set; } = string.Empty;
	public int TargetPeopleCount { get; set; }
	public decimal? TargetPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Note { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<CreateGroupBuyingRequestDto, GroupBuyingRequest>();
	}
}