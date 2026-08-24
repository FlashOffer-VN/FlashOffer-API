using System;

namespace FlashOffer.API.Domain.Entities;

public class SocialLike : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    public virtual SocialPost Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}