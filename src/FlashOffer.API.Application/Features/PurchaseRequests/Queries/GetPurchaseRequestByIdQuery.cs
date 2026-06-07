using MediatR;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Queries;

public class GetPurchaseRequestByIdQuery : IRequest<PurchaseRequestResponseDto>
{
	public Guid Id { get; set; }
}