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

        RuleFor(x => x.CompanyAddress)
            .NotEmpty().WithMessage(localizer["PartnerCompanyAddressRequired"])
            .MinimumLength(5).WithMessage(localizer["PartnerCompanyAddressMinLength"]);

        RuleFor(x => x.CompanySize)
            .IsInEnum().WithMessage(localizer["PartnerCompanySizeInvalid"]);

        // Step 2: Sản phẩm & dịch vụ cung cấp (tối giản: name bắt buộc)
        RuleFor(x => x.Products)
            .NotEmpty().WithMessage(localizer["PartnerProductsRequired"])
            .Must(list => list.Count > 0).WithMessage(localizer["PartnerProductsRequired"]);

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.Name)
                .NotEmpty().WithMessage(localizer["PartnerProductNameRequired"])
                .MinimumLength(2).WithMessage(localizer["PartnerProductNameMinLength"]);
        });

        // Step 3: Confirmation
        RuleFor(x => x.AgreeTerms)
            .Equal(true).WithMessage(localizer["PartnerAgreeTermsRequired"]);
    }
}