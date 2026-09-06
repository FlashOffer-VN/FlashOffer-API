using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Dashboard.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashOffer.API.Application.Features.Dashboard.Handlers;

public class GetCrmDashboardQueryHandler : IRequestHandler<GetCrmDashboardQuery, CrmDashboardStatsDto>
{
    private readonly IRepository<OfferRequest> _offerRepo;
    private readonly IRepository<PurchaseRequest> _purchaseRepo;
    private readonly IRepository<GroupBuyingRequest> _groupBuyingRepo;
    private readonly IRepository<Collaborator> _ctvRepo;
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IRepository<SocialPost> _socialRepo;
    private readonly ILogger<GetCrmDashboardQueryHandler> _logger;

    public GetCrmDashboardQueryHandler(
        IRepository<OfferRequest> offerRepo,
        IRepository<PurchaseRequest> purchaseRepo,
        IRepository<GroupBuyingRequest> groupBuyingRepo,
        IRepository<Collaborator> ctvRepo,
        IRepository<Partner> partnerRepo,
        IRepository<SocialPost> socialRepo,
        ILogger<GetCrmDashboardQueryHandler> logger)
    {
        _offerRepo = offerRepo;
        _purchaseRepo = purchaseRepo;
        _groupBuyingRepo = groupBuyingRepo;
        _ctvRepo = ctvRepo;
        _partnerRepo = partnerRepo;
        _socialRepo = socialRepo;
        _logger = logger;
    }

    public async Task<CrmDashboardStatsDto> Handle(GetCrmDashboardQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin CRM dashboard stats requested");

        var offers = (await _offerRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var purchases = (await _purchaseRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var groupBuyings = (await _groupBuyingRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var ctvs = (await _ctvRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var partners = (await _partnerRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var socials = (await _socialRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();

        return new CrmDashboardStatsDto
        {
            Summary = BuildSummary(offers, purchases, groupBuyings, ctvs, partners, socials),
            Trends = BuildTrends(offers, purchases, ctvs, partners),
            Distribution = BuildDistribution(offers, purchases, ctvs, partners),
            TopCategories = BuildTopCategories(purchases)
        };
    }

    private static CrmSummaryDto BuildSummary(
        List<OfferRequest> offers,
        List<PurchaseRequest> purchases,
        List<GroupBuyingRequest> groupBuyings,
        List<Collaborator> ctvs,
        List<Partner> partners,
        List<SocialPost> socials)
    {
        return new CrmSummaryDto
        {
            TotalOffers = offers.Count,
            PendingOffers = offers.Count(x => x.Status == OfferStatus.Pending),
            TotalPurchaseRequests = purchases.Count,
            PendingPurchaseRequests = purchases.Count(x => x.Status == PurchaseRequestStatus.Pending),
            TotalGroupBuyingRequests = groupBuyings.Count,
            PendingGroupBuyingRequests = groupBuyings.Count(x => x.Status == GroupBuyingStatus.Pending),
            TotalCtvRegistrations = ctvs.Count,
            PendingCtvRegistrations = ctvs.Count(x => x.Status == CollaboratorStatus.Pending),
            TotalPartners = partners.Count,
            PendingPartners = partners.Count(x => x.Status == PartnerStatus.Pending),
            TotalSocialPosts = socials.Count
        };
    }

    private static List<CrmTrendPointDto> BuildTrends(
        List<OfferRequest> offers,
        List<PurchaseRequest> purchases,
        List<Collaborator> ctvs,
        List<Partner> partners)
    {
        // 12 tháng gần nhất (bao gồm tháng hiện tại)
        var months = new List<string>();
        var now = DateTime.UtcNow;
        for (var i = 11; i >= 0; i--)
        {
            months.Add(now.AddMonths(-i).ToString("yyyy-MM"));
        }

        var offerByMonth = CountByMonth(offers.Select(x => x.CreatedAt));
        var purchaseByMonth = CountByMonth(purchases.Select(x => x.CreatedAt));
        var ctvByMonth = CountByMonth(ctvs.Select(x => x.CreatedAt));
        var partnerByMonth = CountByMonth(partners.Select(x => x.CreatedAt));

        return months.Select(month => new CrmTrendPointDto
        {
            Month = month,
            Offers = offerByMonth.GetValueOrDefault(month),
            PurchaseRequests = purchaseByMonth.GetValueOrDefault(month),
            CtvRegistrations = ctvByMonth.GetValueOrDefault(month),
            Partners = partnerByMonth.GetValueOrDefault(month),
            Revenue = 0
        }).ToList();
    }

    private static Dictionary<string, int> CountByMonth(IEnumerable<DateTime> dates)
    {
        return dates
            .GroupBy(d => d.ToString("yyyy-MM"))
            .ToDictionary(g => g.Key, g => g.Count());
    }

    private static List<CrmDistributionSliceDto> BuildDistribution(
        List<OfferRequest> offers,
        List<PurchaseRequest> purchases,
        List<Collaborator> ctvs,
        List<Partner> partners)
    {
        var result = new List<CrmDistributionSliceDto>();

        result.AddRange(offers
            .GroupBy(x => x.Status)
            .Select(g => new CrmDistributionSliceDto
            {
                Entity = "offer",
                Status = (int)g.Key,
                StatusLabel = GetStatusLabel(g.Key),
                Count = g.Count()
            }));

        result.AddRange(purchases
            .GroupBy(x => x.Status)
            .Select(g => new CrmDistributionSliceDto
            {
                Entity = "purchase",
                Status = (int)g.Key,
                StatusLabel = GetStatusLabel(g.Key),
                Count = g.Count()
            }));

        result.AddRange(ctvs
            .GroupBy(x => x.Status)
            .Select(g => new CrmDistributionSliceDto
            {
                Entity = "ctv",
                Status = (int)g.Key,
                StatusLabel = GetStatusLabel(g.Key),
                Count = g.Count()
            }));

        result.AddRange(partners
            .GroupBy(x => x.Status)
            .Select(g => new CrmDistributionSliceDto
            {
                Entity = "partner",
                Status = (int)g.Key,
                StatusLabel = GetStatusLabel(g.Key),
                Count = g.Count()
            }));

        return result;
    }

    private static string GetStatusLabel(Enum status)
    {
        return status switch
        {
            OfferStatus s => s switch
            {
                OfferStatus.Pending => "COMMON.STATUS.PENDING",
                OfferStatus.Approved => "COMMON.STATUS.APPROVED",
                OfferStatus.Rejected => "COMMON.STATUS.REJECTED",
                OfferStatus.Expired => "COMMON.STATUS.EXPIRED",
                _ => "COMMON.STATUS.PENDING"
            },
            PurchaseRequestStatus s => s switch
            {
                PurchaseRequestStatus.Pending => "COMMON.STATUS.PENDING",
                PurchaseRequestStatus.Contacted => "COMMON.STATUS.CONTACTED",
                PurchaseRequestStatus.Completed => "COMMON.STATUS.COMPLETED",
                _ => "COMMON.STATUS.PENDING"
            },
            CollaboratorStatus s => s switch
            {
                CollaboratorStatus.Pending => "COMMON.STATUS.PENDING",
                CollaboratorStatus.Approved => "COMMON.STATUS.APPROVED",
                CollaboratorStatus.Rejected => "COMMON.STATUS.REJECTED",
                _ => "COMMON.STATUS.PENDING"
            },
            PartnerStatus s => s switch
            {
                PartnerStatus.Pending => "COMMON.STATUS.PENDING",
                PartnerStatus.Approved => "COMMON.STATUS.APPROVED",
                PartnerStatus.Rejected => "COMMON.STATUS.REJECTED",
                PartnerStatus.Active => "COMMON.STATUS.ACTIVE",
                _ => "COMMON.STATUS.PENDING"
            },
            _ => "COMMON.STATUS.PENDING"
        };
    }

    private static List<CrmCategoryCountDto> BuildTopCategories(List<PurchaseRequest> purchases)
    {
        return purchases
            .Where(x => !string.IsNullOrWhiteSpace(x.ProductCategory))
            .GroupBy(x => x.ProductCategory!)
            .Select(g => new CrmCategoryCountDto
            {
                Name = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();
    }
}
