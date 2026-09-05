using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Reports.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashOffer.API.Application.Features.Reports.Handlers;

public class GetRequestReportQueryHandler : IRequestHandler<GetRequestReportQuery, RequestReportDto>
{
    private readonly IRepository<PurchaseRequest> _purchaseRepo;
    private readonly IRepository<GroupBuyingRequest> _groupBuyingRepo;
    private readonly IRepository<OfferRequest> _offerRepo;
    private readonly ILogger<GetRequestReportQueryHandler> _logger;

    public GetRequestReportQueryHandler(
        IRepository<PurchaseRequest> purchaseRepo,
        IRepository<GroupBuyingRequest> groupBuyingRepo,
        IRepository<OfferRequest> offerRepo,
        ILogger<GetRequestReportQueryHandler> logger)
    {
        _purchaseRepo = purchaseRepo;
        _groupBuyingRepo = groupBuyingRepo;
        _offerRepo = offerRepo;
        _logger = logger;
    }

    public async Task<RequestReportDto> Handle(GetRequestReportQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin report requests requested [Type: {Type}]", request.Type);

        var type = request.Type?.Trim().ToLowerInvariant();
        var items = new List<RequestBreakdownDto>();

        if (string.IsNullOrEmpty(type) || type == "purchase")
        {
            var purchases = await _purchaseRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
            items.AddRange(purchases
                .GroupBy(x => x.Status)
                .Select(g => new RequestBreakdownDto { Type = "purchase", Status = g.Key.ToString(), Count = g.Count() }));
        }

        if (string.IsNullOrEmpty(type) || type == "groupbuying")
        {
            var groupBuyings = await _groupBuyingRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
            items.AddRange(groupBuyings
                .GroupBy(x => x.Status)
                .Select(g => new RequestBreakdownDto { Type = "groupbuying", Status = g.Key.ToString(), Count = g.Count() }));
        }

        if (string.IsNullOrEmpty(type) || type == "offer")
        {
            var offers = await _offerRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
            items.AddRange(offers
                .GroupBy(x => x.Status)
                .Select(g => new RequestBreakdownDto { Type = "offer", Status = g.Key.ToString(), Count = g.Count() }));
        }

        return new RequestReportDto { Items = items };
    }
}