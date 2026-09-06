// src/FlashOffer.API.Application/Features/OfferRequests/Commands/DeleteOfferRequestCommand.cs
using FlashOffer.API.Application.DTOs.responses;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class DeleteOfferRequestCommand : IRequest<OfferRequestResponseDto>
{
	public Guid Id { get; set; }
}