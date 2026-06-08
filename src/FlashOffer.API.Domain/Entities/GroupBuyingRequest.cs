using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class GroupBuyingRequest : BaseEntity
{
	public string ProductName { get; set; } = string.Empty;
	public int TargetPeopleCount { get; set; }
	public int CurrentPeopleCount { get; set; }
	public decimal? TargetPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Note { get; set; }
	public GroupBuyingStatus Status { get; set; } = GroupBuyingStatus.Pending;
}