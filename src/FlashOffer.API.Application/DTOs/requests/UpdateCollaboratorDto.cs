using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

public class UpdateCollaboratorDto : IMapFrom<Collaborator>
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public string? Position { get; set; }
    public string? Skills { get; set; }
    public string? Interests { get; set; }
    public string? Goals { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string? Experience { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateCollaboratorDto, Collaborator>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
}