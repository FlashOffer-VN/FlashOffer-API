using FluentValidation;
using FlashOffer.API.Application.Features.PurchaseRequests.Queries;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class ExportPurchaseRequestsValidator : AbstractValidator<ExportPurchaseRequestsQuery>
{
	public ExportPurchaseRequestsValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.Status)
			.IsInEnum()
			.When(x => x.Status.HasValue)
			.WithMessage(localizer["StatusInvalid"]);

		RuleFor(x => x.FromDate)
			.LessThanOrEqualTo(x => x.ToDate)
			.When(x => x.FromDate.HasValue && x.ToDate.HasValue)
			.WithMessage(localizer["FromDateMustBeBeforeToDate"]);
	}
}