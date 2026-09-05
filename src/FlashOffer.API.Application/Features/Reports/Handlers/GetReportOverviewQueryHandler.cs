using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Reports.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FlashOffer.API.Application.Features.Reports.Handlers;

public class GetReportOverviewQueryHandler : IRequestHandler<GetReportOverviewQuery, ReportOverviewDto>
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IRepository<Collaborator> _collaboratorRepo;
    private readonly IRepository<SocialPost> _postRepo;
    private readonly IRepository<PurchaseRequest> _purchaseRepo;
    private readonly IRepository<GroupBuyingRequest> _groupBuyingRepo;
    private readonly IRepository<OfferRequest> _offerRepo;
    private readonly ILogger<GetReportOverviewQueryHandler> _logger;

    public GetReportOverviewQueryHandler(
        IRepository<User> userRepo,
        IRepository<Partner> partnerRepo,
        IRepository<Collaborator> collaboratorRepo,
        IRepository<SocialPost> postRepo,
        IRepository<PurchaseRequest> purchaseRepo,
        IRepository<GroupBuyingRequest> groupBuyingRepo,
        IRepository<OfferRequest> offerRepo,
        ILogger<GetReportOverviewQueryHandler> logger)
    {
        _userRepo = userRepo;
        _partnerRepo = partnerRepo;
        _collaboratorRepo = collaboratorRepo;
        _postRepo = postRepo;
        _purchaseRepo = purchaseRepo;
        _groupBuyingRepo = groupBuyingRepo;
        _offerRepo = offerRepo;
        _logger = logger;
    }

    public async Task<ReportOverviewDto> Handle(GetReportOverviewQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin report overview requested [{From}] - [{To}]", request.FromDate, request.ToDate);

        var users = await _userRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var partners = await _partnerRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var collaborators = await _collaboratorRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var posts = await _postRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var purchases = await _purchaseRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var groupBuyings = await _groupBuyingRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var offers = await _offerRepo.FindAsync(x => !x.IsDeleted, cancellationToken);

        var (from, to) = NormalizeRange(request.FromDate, request.ToDate);

        var dto = new ReportOverviewDto
        {
            FromDate = from,
            ToDate = to,

            TotalUsers = users.Count(),
            NewUsers = users.Count(x => IsInRange(x.CreatedAt, from, to)),
            ActiveUsers = users.Count(x => x.IsActive),

            TotalPartners = partners.Count(),
            NewPartners = partners.Count(x => IsInRange(x.CreatedAt, from, to)),
            PendingPartners = partners.Count(x => x.Status == PartnerStatus.Pending),
            ApprovedPartners = partners.Count(x => x.Status is PartnerStatus.Approved or PartnerStatus.Active),

            TotalCollaborators = collaborators.Count(),
            NewCollaborators = collaborators.Count(x => IsInRange(x.CreatedAt, from, to)),
            PendingCollaborators = collaborators.Count(x => x.Status == CollaboratorStatus.Pending),
            ApprovedCollaborators = collaborators.Count(x => x.Status == CollaboratorStatus.Approved),

            TotalPosts = posts.Count(),
            NewPosts = posts.Count(x => IsInRange(x.CreatedAt, from, to)),
            PendingPosts = posts.Count(x => !x.IsApproved && !x.IsDeleted),
            ApprovedPosts = posts.Count(x => x.IsApproved && !x.IsDeleted),

            TotalPurchaseRequests = purchases.Count(),
            PendingPurchaseRequests = purchases.Count(x => x.Status == PurchaseRequestStatus.Pending),
            TotalGroupBuyingRequests = groupBuyings.Count(),
            PendingGroupBuyingRequests = groupBuyings.Count(x => x.Status == GroupBuyingStatus.Pending),
            TotalOfferRequests = offers.Count(),
            PendingOfferRequests = offers.Count(x => x.Status == OfferStatus.Pending)
        };

        return dto;
    }

    private static (DateTime?, DateTime?) NormalizeRange(DateTime? fromDate, DateTime? toDate)
    {
        var from = fromDate?.Date;
        var to = toDate?.Date.AddDays(1).AddTicks(-1);
        return (from, to);
    }

    private static bool IsInRange(DateTime value, DateTime? from, DateTime? to)
        => (!from.HasValue || value >= from.Value) && (!to.HasValue || value <= to.Value);
}