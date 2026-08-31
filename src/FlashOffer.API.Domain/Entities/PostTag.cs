namespace FlashOffer.API.Domain.Entities;

public class PostTag : BaseEntity
{
    public Guid PostId { get; set; }
    public virtual SocialPost Post { get; set; } = null!;

    public Guid TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
}