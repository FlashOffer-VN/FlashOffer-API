// OfferRequestQueryValidator.cs
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class OfferRequestQueryValidator : AbstractValidator<OfferRequestQueryDto>
{
	public OfferRequestQueryValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.Page)
			.GreaterThanOrEqualTo(1)
			.WithMessage(localizer["PageMin"]);

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100)
			.WithMessage(localizer["PageSizeRange"]);
	}
}