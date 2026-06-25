// OfferRequestQueryDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.Features.OfferRequests.Queries;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.requests;

public class OfferRequestQueryDto : IMapFrom<GetOfferRequestsQuery>
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public bool? IsOfferSent { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<OfferRequestQueryDto, GetOfferRequestsQuery>();
	}
}