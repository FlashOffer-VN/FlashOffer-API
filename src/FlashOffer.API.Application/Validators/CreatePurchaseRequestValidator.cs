using FlashOffer.API.Application.DTOs.requests;
using FluentValidation;
using Microsoft.Extensions.Localization;
using FlashOffer.API.Application.Resources;

namespace FlashOffer.API.Application.Validators; 

public class CreatePurchaseRequestValidator : AbstractValidator<CreatePurchaseRequestDto>
{
	public CreatePurchaseRequestValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.ProductName)
			.NotEmpty().WithMessage(localizer["ProductNameRequired"])
			.MaximumLength(500).WithMessage(localizer["ProductNameMaxLength"]);

		RuleFor(x => x.Quantity)
			.GreaterThanOrEqualTo(1).WithMessage(localizer["QuantityMin"]);

		RuleFor(x => x.FullName)
			.NotEmpty().WithMessage(localizer["FullNameRequired"])
			.MaximumLength(200).WithMessage(localizer["FullNameMaxLength"]);

		RuleFor(x => x.Phone)
			.NotEmpty().WithMessage(localizer["PhoneRequired"])
			.Matches(@"^\d{10,11}$").WithMessage(localizer["PhoneInvalid"]);

		RuleFor(x => x.Email)
			.EmailAddress().WithMessage(localizer["EmailInvalid"])
			.When(x => !string.IsNullOrEmpty(x.Email));

		RuleFor(x => x.ExpectedPrice)
			.GreaterThan(0).WithMessage(localizer["ExpectedPricePositive"])
			.When(x => x.ExpectedPrice.HasValue);
	}
}