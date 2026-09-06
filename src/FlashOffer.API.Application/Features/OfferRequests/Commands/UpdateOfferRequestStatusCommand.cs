// src/FlashOffer.API.Application/Features/OfferRequests/Commands/UpdateOfferRequestStatusCommand.cs
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Enums;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class UpdateOfferRequestStatusCommand : IRequest<OfferRequestStatusResponseDto>
{
	public Guid Id { get; set; }
	public OfferStatus Status { get; set; }
}