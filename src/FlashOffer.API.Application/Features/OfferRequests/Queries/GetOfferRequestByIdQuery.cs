// src/FlashOffer.API.Application/Features/OfferRequests/Queries/GetOfferRequestByIdQuery.cs
using FlashOffer.API.Application.DTOs.responses;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Queries;

public class GetOfferRequestByIdQuery : IRequest<OfferRequestResponseDto?>
{
	public Guid Id { get; set; }
}