// Application/Validators/UpdatePurchaseRequestStatusValidator.cs
using FluentValidation;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class UpdatePurchaseRequestStatusValidator : AbstractValidator<UpdatePurchaseRequestStatusDto>
{
	public UpdatePurchaseRequestStatusValidator(IStringLocalizer<SharedResource> localizer)
	{
		RuleFor(x => x.Status)
			.IsInEnum()
			.WithMessage(localizer["StatusInvalid"]);
	}
}