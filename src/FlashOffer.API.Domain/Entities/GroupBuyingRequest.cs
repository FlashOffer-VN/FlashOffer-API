using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

public class GroupBuyingRequest : BaseEntity
{
	public Guid UserId { get; set; } // Thêm FK
	public string ProductName { get; set; } = string.Empty;
	public int TargetPeopleCount { get; set; }
	public int CurrentPeopleCount { get; set; }
	public decimal? TargetPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Note { get; set; }
	public GroupBuyingStatus Status { get; set; } = GroupBuyingStatus.Pending;

	public virtual User User { get; set; } = null!;
}