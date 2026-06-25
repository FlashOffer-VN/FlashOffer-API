// CtvRegistrationQueryValidator.cs
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CtvRegistrationQueryValidator : AbstractValidator<CtvRegistrationQueryDto>
{
	public CtvRegistrationQueryValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.Page)
			.GreaterThanOrEqualTo(1)
			.WithMessage(localizer["PageMin"]);

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100)
			.WithMessage(localizer["PageSizeRange"]);
	}
}