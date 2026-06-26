using MediatR;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Queries;

public class ExportPurchaseRequestsQuery : IRequest<byte[]>
{
	public PurchaseRequestStatus? Status { get; set; }
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }
}