// src/FlashOffer.API.Application/Validators/CreateOfferRequestValidator.cs
using FlashOffer.API.Application.DTOs.requests;
using FluentValidation;
using Microsoft.Extensions.Localization;
using FlashOffer.API.Application.Resources;

namespace FlashOffer.API.Application.Validators;

public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequestDto>
{
    public CreateOfferRequestValidator(IStringLocalizer<SharedResource> localizer)
    {
        // Product information
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage(localizer["OfferRequest_ProductNameRequired"])
            .MinimumLength(3).WithMessage(localizer["OfferRequest_ProductNameMinLength"]);

        RuleFor(x => x.ProductLink)
            .Must(link => string.IsNullOrEmpty(link) || Uri.IsWellFormedUriString(link, UriKind.Absolute))
            .WithMessage(localizer["OfferRequest_ProductLinkInvalid"])
            .When(x => !string.IsNullOrEmpty(x.ProductLink));

        RuleFor(x => x.CurrentPrice)
            .NotEmpty().WithMessage(localizer["OfferRequest_CurrentPriceRequired"])
            .GreaterThan(0).WithMessage(localizer["OfferRequest_CurrentPriceInvalid"]);

        RuleFor(x => x.ExpectedPrice)
            .GreaterThan(0).WithMessage(localizer["OfferRequest_ExpectedPriceInvalid"])
            .When(x => x.ExpectedPrice.HasValue);

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(localizer["OfferRequest_QuantityRequired"])
            .GreaterThanOrEqualTo(1).WithMessage(localizer["OfferRequest_QuantityMin"]);

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage(localizer["OfferRequest_UnitRequired"])
            .MinimumLength(1).WithMessage(localizer["OfferRequest_UnitMinLength"]);

        // User information
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["OfferRequest_FullNameRequired"])
            .MinimumLength(2).WithMessage(localizer["OfferRequest_FullNameMinLength"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["OfferRequest_PhoneRequired"])
            .Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
            .WithMessage(localizer["OfferRequest_PhoneInvalid"]);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(localizer["OfferRequest_EmailInvalid"])
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}