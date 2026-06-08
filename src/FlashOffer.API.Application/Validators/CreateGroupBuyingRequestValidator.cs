using FluentValidation;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreateGroupBuyingRequestValidator : AbstractValidator<CreateGroupBuyingRequestDto>
{
	public CreateGroupBuyingRequestValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.ProductName)
			.NotEmpty().WithMessage(localizer["ProductNameRequired"])
			.MaximumLength(500).WithMessage(localizer["ProductNameMaxLength"]);

		RuleFor(x => x.TargetPeopleCount)
			.InclusiveBetween(2, 100).WithMessage(localizer["TargetPeopleCountInvalid"]);

		RuleFor(x => x.TargetPrice)
			.GreaterThan(0).WithMessage(localizer["TargetPriceInvalid"])
			.When(x => x.TargetPrice.HasValue);

		RuleFor(x => x.FullName)
			.NotEmpty().WithMessage(localizer["FullNameRequired"])
			.MaximumLength(200).WithMessage(localizer["FullNameMaxLength"]);

		RuleFor(x => x.Phone)
			.NotEmpty().WithMessage(localizer["PhoneRequired"])
			.Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
			.WithMessage(localizer["PhoneInvalid"])
			.When(x => !string.IsNullOrEmpty(x.Phone));

		RuleFor(x => x.Note)
			.MaximumLength(1000).WithMessage(localizer["NoteMaxLength"])
			.When(x => !string.IsNullOrEmpty(x.Note));
	}
}