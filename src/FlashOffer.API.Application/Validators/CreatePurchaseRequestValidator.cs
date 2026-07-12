using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace FlashOffer.API.Application.Validators;

public class CreatePurchaseRequestValidator : AbstractValidator<CreatePurchaseRequestDto>
{
    public CreatePurchaseRequestValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_ProductNameRequired"])
            .MinimumLength(3).WithMessage(localizer["PurchaseRequest_ProductNameMinLength"]);

        RuleFor(x => x.ProductCategory)
            .MaximumLength(100).WithMessage(localizer["PurchaseRequest_ProductCategoryLength"])
            .When(x => !string.IsNullOrEmpty(x.ProductCategory));

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_QuantityRequired"])
            .GreaterThan(0).WithMessage(localizer["PurchaseRequest_QuantityPositive"]);

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_UnitRequired"])
            .MaximumLength(50).WithMessage(localizer["PurchaseRequest_UnitLength"]);

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_FullNameRequired"])
            .MinimumLength(2).WithMessage(localizer["PurchaseRequest_FullNameMinLength"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_PhoneRequired"])
            .Must(phone => Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
            .WithMessage(localizer["PurchaseRequest_PhoneInvalid"]);

        RuleFor(x => x.Zalo)
            .Must(zalo => string.IsNullOrEmpty(zalo) || Regex.IsMatch(zalo, @"^0[0-9]{9,10}$"))
            .WithMessage(localizer["PurchaseRequest_ZaloInvalid"])
            .When(x => !string.IsNullOrEmpty(x.Zalo));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["PurchaseRequest_EmailRequired"])
            .EmailAddress().WithMessage(localizer["PurchaseRequest_EmailInvalid"]);
    }
}