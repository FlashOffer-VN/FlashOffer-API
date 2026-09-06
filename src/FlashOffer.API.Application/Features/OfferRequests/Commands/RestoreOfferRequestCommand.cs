// src/FlashOffer.API.Application/Features/OfferRequests/Commands/RestoreOfferRequestCommand.cs
using FlashOffer.API.Application.DTOs.responses;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class RestoreOfferRequestCommand : IRequest<OfferRequestResponseDto>
{
	public Guid Id { get; set; }
}