// src/FlashOffer.API.Application/Features/OfferRequests/Commands/CreateOfferRequestCommand.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class CreateOfferRequestCommand : IRequest<OfferRequestResponseDto>, IMapFrom<CreateOfferRequestDto>
{
    public string ProductName { get; set; } = string.Empty;
    public string? ProductLink { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal? ExpectedPrice { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOfferRequestDto, CreateOfferRequestCommand>();
    }
}