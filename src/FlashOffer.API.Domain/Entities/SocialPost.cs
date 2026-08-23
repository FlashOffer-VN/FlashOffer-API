using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class SocialPost : BaseEntity
{
    public string? Title { get; set; }                          // Tiêu đề (không bắt buộc)
    public string Content { get; set; } = string.Empty;        // Nội dung (bắt buộc)
    public PostType Type { get; set; } = PostType.Post;        // Loại bài viết
    public PrivacyType Privacy { get; set; } = PrivacyType.Public; // Quyền riêng tư

    // Tags và Images lưu dạng JSON
    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();            // Danh sách thẻ
    public List<string> Images { get; set; } = new();          // Danh sách URL ảnh

    // Thống kê
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
    public int SharesCount { get; set; } = 0;

    // Liên kết với User (Author)
    public Guid AuthorId { get; set; }
    public virtual User Author { get; set; } = null!;

    // Dành riêng cho Question
    public bool IsAnswered { get; set; } = false;

    // Dành riêng cho Event
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public int? MaxParticipants { get; set; }
    public int? CurrentParticipants { get; set; }
    public bool? IsOnline { get; set; }

    // Dành riêng cho Announcement
    public PriorityType Priority { get; set; } = PriorityType.Normal;
    public DateTime? PinnedUntil { get; set; }
}