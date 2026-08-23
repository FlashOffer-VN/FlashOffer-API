using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.requests;

public class UpdatePostRequest : IMapFrom<SocialPost>
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public PostType? Type { get; set; }
    public PrivacyType? Privacy { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? Images { get; set; }

    // For Question
    public bool? IsAnswered { get; set; }

    // For Event
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public int? MaxParticipants { get; set; }
    public bool? IsOnline { get; set; }

    // For Announcement
    public PriorityType? Priority { get; set; }
    public DateTime? PinnedUntil { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePostRequest, SocialPost>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
