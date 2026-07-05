// PartnerRegisterValidator.cs
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class PartnerRegisterValidator : AbstractValidator<PartnerRegisterRequest>
{
    public PartnerRegisterValidator(IStringLocalizer<SharedResource> localizer)
    {
        // Step 1: Personal Info
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["PartnerFullNameRequired"])
            .MinimumLength(2).WithMessage(localizer["PartnerFullNameMinLength"])
            .MaximumLength(100).WithMessage(localizer["PartnerFullNameMaxLength"]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["PartnerEmailRequired"])
            .EmailAddress().WithMessage(localizer["PartnerEmailInvalid"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["PartnerPhoneRequired"])
            .Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9,10}$"))
            .WithMessage(localizer["PartnerPhoneInvalid"]);

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(localizer["PartnerPositionRequired"])
            .MinimumLength(2).WithMessage(localizer["PartnerPositionMinLength"]);

        // Step 2: Business Info
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage(localizer["PartnerCompanyNameRequired"])
            .MinimumLength(2).WithMessage(localizer["PartnerCompanyNameMinLength"]);

        RuleFor(x => x.CompanyTax)
            .NotEmpty().WithMessage(localizer["PartnerCompanyTaxRequired"])
            .Must(tax => System.Text.RegularExpressions.Regex.IsMatch(tax, @"^[0-9]{10,14}$"))
            .WithMessage(localizer["PartnerCompanyTaxInvalid"]);

        RuleFor(x => x.CompanyAddress)
            .NotEmpty().WithMessage(localizer["PartnerCompanyAddressRequired"])
            .MinimumLength(5).WithMessage(localizer["PartnerCompanyAddressMinLength"]);

        RuleFor(x => x.BusinessType)
            .IsInEnum().WithMessage(localizer["PartnerBusinessTypeInvalid"]);

        RuleFor(x => x.CompanySize)
            .IsInEnum().WithMessage(localizer["PartnerCompanySizeInvalid"]);

        RuleFor(x => x.CompanyWebsite)
            .Must(uri => string.IsNullOrEmpty(uri) ||
                System.Text.RegularExpressions.Regex.IsMatch(uri, @"^https?:\/\/.+\..+$"))
            .WithMessage(localizer["PartnerCompanyWebsiteInvalid"]);

        // Step 3: Products
        RuleFor(x => x.Products)
            .NotEmpty().WithMessage(localizer["PartnerProductsRequired"])
            .Must(list => list.Count > 0).WithMessage(localizer["PartnerProductsRequired"]);

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.Name)
                .NotEmpty().WithMessage(localizer["PartnerProductNameRequired"])
                .MinimumLength(2).WithMessage(localizer["PartnerProductNameMinLength"]);

            product.RuleFor(p => p.Category)
                .IsInEnum().WithMessage(localizer["PartnerProductCategoryInvalid"]);

            product.RuleFor(p => p.RetailPrice)
                .GreaterThanOrEqualTo(0).WithMessage(localizer["PartnerRetailPriceNonNegative"]);

            product.RuleFor(p => p.WholesalePrice)
                .GreaterThanOrEqualTo(0).WithMessage(localizer["PartnerWholesalePriceNonNegative"]);

            product.RuleFor(p => p.MinOrderQuantity)
                .GreaterThanOrEqualTo(1).WithMessage(localizer["PartnerMinOrderQuantityPositive"]);
        });

        // Step 3: Commission
        RuleFor(x => x.CommissionType)
            .IsInEnum().WithMessage(localizer["PartnerCommissionTypeInvalid"]);

        RuleFor(x => x.CommissionRate)
            .NotEmpty().WithMessage(localizer["PartnerCommissionRateRequired"])
            .InclusiveBetween(0, 100).WithMessage(localizer["PartnerCommissionRateRange"]);

        // Step 4: Confirmation
        RuleFor(x => x.AgreeTerms)
            .Equal(true).WithMessage(localizer["PartnerAgreeTermsRequired"]);
    }
}