// src/FlashOffer.API.Application/Validators/CreateGroupBuyingRequestValidator.cs
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace FlashOffer.API.Application.Validators;

public class CreateGroupBuyingRequestValidator : AbstractValidator<CreateGroupBuyingRequestDto>
{
    public CreateGroupBuyingRequestValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_ProductNameRequired"])
            .MinimumLength(3).WithMessage(localizer["GroupBuyingRequest_ProductNameMinLength"]);

        RuleFor(x => x.ProductLink)
            .Must(link => string.IsNullOrEmpty(link) || Uri.IsWellFormedUriString(link, UriKind.Absolute))
            .WithMessage(localizer["GroupBuyingRequest_ProductLinkInvalid"]);

        RuleFor(x => x.TargetPrice)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_TargetPriceRequired"])
            .GreaterThanOrEqualTo(1000).WithMessage(localizer["GroupBuyingRequest_TargetPriceMin"]);

        RuleFor(x => x.TargetPeopleCount)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_TargetPeopleCountRequired"])
            .GreaterThanOrEqualTo(2).WithMessage(localizer["GroupBuyingRequest_TargetPeopleCountMin"]);

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_FullNameRequired"])
            .MinimumLength(2).WithMessage(localizer["GroupBuyingRequest_FullNameMinLength"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_PhoneRequired"])
            .Must(phone => Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
            .WithMessage(localizer["GroupBuyingRequest_PhoneInvalid"]);

        RuleFor(x => x.Zalo)
            .Must(zalo => string.IsNullOrEmpty(zalo) || Regex.IsMatch(zalo, @"^[0-9]{10}$"))
            .WithMessage(localizer["GroupBuyingRequest_ZaloInvalid"]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["GroupBuyingRequest_EmailRequired"])
            .EmailAddress().WithMessage(localizer["GroupBuyingRequest_EmailInvalid"]);
    }
}