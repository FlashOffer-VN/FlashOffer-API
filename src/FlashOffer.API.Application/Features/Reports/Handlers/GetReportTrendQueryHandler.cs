using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Reports.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashOffer.API.Application.Features.Reports.Handlers;

public class GetReportTrendQueryHandler : IRequestHandler<GetReportTrendQuery, ReportTrendDto>
{
    private const int MaxDays = 90;

    private readonly IRepository<User> _userRepo;
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IRepository<Collaborator> _collaboratorRepo;
    private readonly IRepository<SocialPost> _postRepo;
    private readonly IRepository<PurchaseRequest> _purchaseRepo;
    private readonly IRepository<GroupBuyingRequest> _groupBuyingRepo;
    private readonly IRepository<OfferRequest> _offerRepo;
    private readonly ILogger<GetReportTrendQueryHandler> _logger;

    public GetReportTrendQueryHandler(
        IRepository<User> userRepo,
        IRepository<Partner> partnerRepo,
        IRepository<Collaborator> collaboratorRepo,
        IRepository<SocialPost> postRepo,
        IRepository<PurchaseRequest> purchaseRepo,
        IRepository<GroupBuyingRequest> groupBuyingRepo,
        IRepository<OfferRequest> offerRepo,
        ILogger<GetReportTrendQueryHandler> logger)
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

    public async Task<ReportTrendDto> Handle(GetReportTrendQuery request, CancellationToken cancellationToken)
    {
        var days = Math.Clamp(request.Days, 1, MaxDays);
        _logger.LogInformation("Admin report trend requested for {Days} days", days);

        var start = DateTime.UtcNow.Date.AddDays(-(days - 1));
        var end = DateTime.UtcNow.Date.AddDays(1); // exclusive upper bound

        // Lấy dữ liệu mới tạo trong khoảng thời gian
        var users = await _userRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var partners = await _partnerRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var collaborators = await _collaboratorRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var posts = await _postRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var purchases = await _purchaseRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var groupBuyings = await _groupBuyingRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);
        var offers = await _offerRepo.FindAsync(x => !x.IsDeleted && x.CreatedAt >= start && x.CreatedAt < end, cancellationToken);

        // Nhóm theo ngày
        var userByDay = GroupByDay(users);
        var partnerByDay = GroupByDay(partners);
        var collaboratorByDay = GroupByDay(collaborators);
        var postByDay = GroupByDay(posts);
        var purchaseByDay = GroupByDay(purchases);
        var groupBuyingByDay = GroupByDay(groupBuyings);
        var offerByDay = GroupByDay(offers);

        var points = new List<ReportTrendPointDto>();
        for (var date = start; date < end; date = date.AddDays(1))
        {
            var newRequests = purchaseByDay.GetValueOrDefault(date)
                + groupBuyingByDay.GetValueOrDefault(date)
                + offerByDay.GetValueOrDefault(date);

            points.Add(new ReportTrendPointDto
            {
                Date = date,
                NewUsers = userByDay.GetValueOrDefault(date),
                NewPartners = partnerByDay.GetValueOrDefault(date),
                NewCollaborators = collaboratorByDay.GetValueOrDefault(date),
                NewPosts = postByDay.GetValueOrDefault(date),
                NewRequests = newRequests
            });
        }

        return new ReportTrendDto
        {
            Days = days,
            Points = points
        };
    }

    private static Dictionary<DateTime, int> GroupByDay<T>(IEnumerable<T> items) where T : BaseEntity
        => items.GroupBy(x => x.CreatedAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());
}