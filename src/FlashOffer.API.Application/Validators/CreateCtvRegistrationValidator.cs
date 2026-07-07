using FluentValidation;
using Microsoft.Extensions.Localization;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;

namespace FlashOffer.API.Application.Validators;

public class CreateCtvRegistrationValidator : AbstractValidator<CreateCtvRegistrationDto>
{
	public CreateCtvRegistrationValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.FullName)
			.NotEmpty().WithMessage(localizer["FullNameRequired"])
			.MaximumLength(200).WithMessage(localizer["FullNameMaxLength"]);

		RuleFor(x => x.Phone)
			.NotEmpty().WithMessage(localizer["PhoneRequired"]);

		RuleFor(x => x.Phone)
			.Must(phone => string.IsNullOrEmpty(phone) || System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
			.WithMessage(localizer["PhoneInvalid"]);

		RuleFor(x => x.Zalo)
			.MaximumLength(50).WithMessage(localizer["ZaloMaxLength"])
			.When(x => !string.IsNullOrEmpty(x.Zalo));

		RuleFor(x => x.Email)
			.EmailAddress().WithMessage(localizer["EmailInvalid"])
			.MaximumLength(100).WithMessage(localizer["EmailMaxLength"])
			.When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.SalesChannel)
			.IsInEnum()
			.WithMessage(localizer["SalesChannelInvalid"]);

        RuleFor(x => x.Experience)
			.MaximumLength(1000).WithMessage(localizer["ExperienceMaxLength"])
			.When(x => !string.IsNullOrEmpty(x.Experience));
	}
}