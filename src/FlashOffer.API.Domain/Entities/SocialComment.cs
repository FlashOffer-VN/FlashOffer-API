using System;
using System.Collections.Generic;

namespace FlashOffer.API.Domain.Entities;

public class SocialComment : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual SocialPost Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual SocialComment? ParentComment { get; set; }
    public virtual ICollection<SocialComment> Replies { get; set; } = new List<SocialComment>();
}