using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Reports.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashOffer.API.Application.Features.Reports.Handlers;

public class GetMembersReportQueryHandler : IRequestHandler<GetMembersReportQuery, MembersReportDto>
{
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IRepository<Collaborator> _collaboratorRepo;
    private readonly IRepository<BusinessField> _businessFieldRepo;
    private readonly ILogger<GetMembersReportQueryHandler> _logger;

    public GetMembersReportQueryHandler(
        IRepository<Partner> partnerRepo,
        IRepository<Collaborator> collaboratorRepo,
        IRepository<BusinessField> businessFieldRepo,
        ILogger<GetMembersReportQueryHandler> logger)
    {
        _partnerRepo = partnerRepo;
        _collaboratorRepo = collaboratorRepo;
        _businessFieldRepo = businessFieldRepo;
        _logger = logger;
    }

    public async Task<MembersReportDto> Handle(GetMembersReportQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin report members requested");

        var partners = await _partnerRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var collaborators = await _collaboratorRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var businessFields = await _businessFieldRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var businessFieldDict = businessFields.ToDictionary(x => x.Id, x => x.Name);

        var partnerReport = new PartnerReportDto
        {
            Total = partners.Count(),
            ByStatus = partners
                .GroupBy(x => x.Status)
                .Select(g => new NameCountDto { Name = g.Key.ToString(), Count = g.Count() })
                .ToList(),
            ByBusinessType = partners
                .GroupBy(x => x.BusinessType)
                .Select(g => new NameCountDto { Name = g.Key.ToString(), Count = g.Count() })
                .ToList(),
            ByBusinessField = partners
                .GroupBy(x => x.BusinessFieldId)
                .Select(g => new NameCountDto
                {
                    Name = g.Key.HasValue && businessFieldDict.TryGetValue(g.Key.Value, out var name)
                        ? name
                        : "Chưa xác định",
                    Count = g.Count()
                })
                .ToList()
        };

        var collaboratorReport = new CollaboratorReportDto
        {
            Total = collaborators.Count(),
            ByStatus = collaborators
                .GroupBy(x => x.Status)
                .Select(g => new NameCountDto { Name = g.Key.ToString(), Count = g.Count() })
                .ToList(),
            ByLevel = collaborators
                .GroupBy(x => x.Level)
                .Select(g => new NameCountDto { Name = $"Cấp {g.Key}", Count = g.Count() })
                .ToList(),
            BySalesChannel = collaborators
                .GroupBy(x => x.SalesChannel)
                .Select(g => new NameCountDto
                {
                    Name = g.Key.HasValue ? g.Key.Value.ToString() : "Chưa xác định",
                    Count = g.Count()
                })
                .ToList()
        };

        return new MembersReportDto
        {
            Partners = partnerReport,
            Collaborators = collaboratorReport
        };
    }
}