using FluentValidation;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequestDto>
{
	public CreateOfferRequestValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.SelectedOffer)
			.NotEmpty().WithMessage(localizer["SelectedOfferRequired"])
			.MaximumLength(500).WithMessage(localizer["SelectedOfferMaxLength"]);

		RuleFor(x => x.FullName)
			.NotEmpty().WithMessage(localizer["FullNameRequired"])
			.MaximumLength(200).WithMessage(localizer["FullNameMaxLength"]);

		RuleFor(x => x.Phone)
			.NotEmpty().WithMessage(localizer["PhoneRequired"])
			.Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
			.WithMessage(localizer["PhoneInvalid"])
			.When(x => !string.IsNullOrEmpty(x.Phone));

		RuleFor(x => x.Zalo)
			.NotEmpty().WithMessage(localizer["ZaloRequired"])
			.MaximumLength(50).WithMessage(localizer["ZaloMaxLength"]);

		RuleFor(x => x.Email)
			.EmailAddress().WithMessage(localizer["EmailInvalid"])
			.When(x => !string.IsNullOrEmpty(x.Email));
	}
}