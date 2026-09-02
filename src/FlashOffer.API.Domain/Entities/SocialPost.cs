using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class SocialPost : BaseEntity
{
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public PostType Type { get; set; } = PostType.Post;
    public PrivacyType Privacy { get; set; } = PrivacyType.Public;

    // Tags và Images
    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    public List<string> Images { get; set; } = new();

    // Counters (tối ưu hiệu năng)
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
    public int SharesCount { get; set; } = 0;

    // Navigation
    public virtual ICollection<SocialLike> Likes { get; set; } = new List<SocialLike>();
    public virtual ICollection<SocialComment> Comments { get; set; } = new List<SocialComment>();
    public virtual ICollection<SocialShare> Shares { get; set; } = new List<SocialShare>();

    // Helper
    public bool IsLikedBy(Guid userId) => Likes.Any(l => l.UserId == userId);

    // Author
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; } = null!;

    // Question
    public bool IsAnswered { get; set; } = false;

    // Event
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public int? MaxParticipants { get; set; }
    public int? CurrentParticipants { get; set; }
    public bool? IsOnline { get; set; }

    // Announcement
    public PriorityType Priority { get; set; } = PriorityType.Normal;
    public DateTime? PinnedUntil { get; set; }

    public bool IsApproved { get; set; } = false;
}