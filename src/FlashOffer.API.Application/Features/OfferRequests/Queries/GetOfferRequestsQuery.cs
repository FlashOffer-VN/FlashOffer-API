// GetOfferRequestsQuery.cs
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Models;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Queries;

public class GetOfferRequestsQuery : IRequest<PagedList<OfferRequestResponseDto>>
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public bool? IsOfferSent { get; set; }
}