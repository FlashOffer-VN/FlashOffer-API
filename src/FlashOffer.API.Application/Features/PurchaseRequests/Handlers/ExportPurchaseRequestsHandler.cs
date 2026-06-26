using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Features.PurchaseRequests.Queries;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Handlers;

public class ExportPurchaseRequestsHandler : IRequestHandler<ExportPurchaseRequestsQuery, byte[]>
{
	private readonly IRepository<PurchaseRequest> _repository;
	private readonly IExcelService _excelService;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public ExportPurchaseRequestsHandler(
		IRepository<PurchaseRequest> repository,
		IExcelService excelService,
		IStringLocalizer<SharedResource> localizer)
	{
		_repository = repository;
		_excelService = excelService;
		_localizer = localizer;
	}

	public async Task<byte[]> Handle(ExportPurchaseRequestsQuery request, CancellationToken cancellationToken)
	{
		var predicate = BuildPredicate(request);
		var entities = await _repository.FindAsync(predicate, cancellationToken);
		var data = entities.OrderByDescending(x => x.CreatedAt).ToList();

		var columns = new Dictionary<string, Func<PurchaseRequest, object>>
		{
			["ExportPurchaseRequests_STT"] = x => data.IndexOf(x) + 1,
			["ExportPurchaseRequests_ProductName"] = x => x.ProductName,
			["ExportPurchaseRequests_Quantity"] = x => x.Quantity,
			["ExportPurchaseRequests_ExpectedPrice"] = x => x.ExpectedPrice.GetValueOrDefault(),
			["ExportPurchaseRequests_FullName"] = x => x.FullName,
			["ExportPurchaseRequests_Phone"] = x => x.Phone,
			["ExportPurchaseRequests_Email"] = x => x.Email ?? "",
			["ExportPurchaseRequests_Status"] = x => x.Status.ToString(),
			["ExportPurchaseRequests_CreatedAt"] = x => x.CreatedAt
		};

		return _excelService.ExportToExcel(
			data,
			columns,
			"PurchaseRequests",
			"ExportPurchaseRequestsTitle",
			_localizer);
	}

	private static Expression<Func<PurchaseRequest, bool>> BuildPredicate(ExportPurchaseRequestsQuery request)
	{
		Expression<Func<PurchaseRequest, bool>> predicate = x => !x.IsDeleted;

		if (request.Status.HasValue)
		{
			var status = request.Status.Value;
			predicate = predicate.AndAlso(x => x.Status == status);
		}

		if (request.FromDate.HasValue)
		{
			var from = request.FromDate.Value.Date;
			predicate = predicate.AndAlso(x => x.CreatedAt >= from);
		}

		if (request.ToDate.HasValue)
		{
			var to = request.ToDate.Value.Date.AddDays(1);
			predicate = predicate.AndAlso(x => x.CreatedAt < to);
		}

		return predicate;
	}
}