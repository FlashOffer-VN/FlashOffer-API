namespace FlashOffer.API.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int UsageCount { get; set; } = 0;

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}