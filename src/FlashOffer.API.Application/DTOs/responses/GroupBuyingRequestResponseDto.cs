using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class GroupBuyingRequestResponseDto : IMapFrom<GroupBuyingRequest>
{
	public Guid Id { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public int CurrentPeopleCount { get; set; }
	public int TargetPeopleCount { get; set; }
	public GroupBuyingStatus Status { get; set; } = GroupBuyingStatus.Pending;
	public DateTime CreatedAt { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<GroupBuyingRequest, GroupBuyingRequestResponseDto>();
	}
}