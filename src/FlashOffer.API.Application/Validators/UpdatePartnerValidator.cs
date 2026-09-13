// UpdatePartnerValidator.cs
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

/// <summary>
/// Validate cập nhật đối tác — partial update nên mọi rule chỉ chạy khi field được gửi lên.
/// </summary>
public class UpdatePartnerValidator : AbstractValidator<UpdatePartnerDto>
{
    public UpdatePartnerValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.FullName)
            .MinimumLength(2).WithMessage(localizer["PartnerFullNameMinLength"])
            .MaximumLength(100).WithMessage(localizer["PartnerFullNameMaxLength"])
            .When(x => x.FullName != null);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["PartnerEmailRequired"])
            .EmailAddress().WithMessage(localizer["PartnerEmailInvalid"])
            .When(x => x.Email != null);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["PartnerPhoneRequired"])
            .Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone!, @"^(0|\+84)[0-9]{9,10}$"))
            .WithMessage(localizer["PartnerPhoneInvalid"])
            .When(x => x.Phone != null);

        RuleFor(x => x.Position)
            .MinimumLength(2).WithMessage(localizer["PartnerPositionMinLength"])
            .When(x => x.Position != null);

        RuleFor(x => x.CompanyName)
            .MinimumLength(2).WithMessage(localizer["PartnerCompanyNameMinLength"])
            .When(x => x.CompanyName != null);

        RuleFor(x => x.CompanyAddress)
            .MinimumLength(5).WithMessage(localizer["PartnerCompanyAddressMinLength"])
            .When(x => x.CompanyAddress != null);

        RuleFor(x => x.CompanySize)
            .IsInEnum().WithMessage(localizer["PartnerCompanySizeInvalid"])
            .When(x => x.CompanySize.HasValue);

        RuleFor(x => x.BusinessType)
            .IsInEnum().WithMessage(localizer["PartnerBusinessTypeInvalid"])
            .When(x => x.BusinessType.HasValue);

        // Sản phẩm không validate ở đây — có API và validator riêng.
    }
}
