using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class PostResponse : IMapFrom<SocialPost>
{
    public Guid Id { get; set; }
    public string? SocialPostCode { get; set; }
    public AuthorDto Author { get; set; } = null!;
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public PostType Type { get; set; }
    public PrivacyType Privacy { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public int SharesCount { get; set; }
    public bool IsLiked { get; set; }
    public bool IsSaved { get; set; }
    public bool IsPinned { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }

    // For Question
    public bool IsAnswered { get; set; }

    // For Event
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public int? MaxParticipants { get; set; }
    public int? CurrentParticipants { get; set; }
    public bool? IsOnline { get; set; }

    // For Announcement
    public PriorityType? Priority { get; set; }
    public DateTime? PinnedUntil { get; set; }
    public string Message { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SocialPost, PostResponse>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.PostTags.Select(pt => pt.Tag.Name).ToList()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.IsLiked, opt => opt.Ignore())
            .ForMember(dest => dest.IsSaved, opt => opt.Ignore());
    }
}
