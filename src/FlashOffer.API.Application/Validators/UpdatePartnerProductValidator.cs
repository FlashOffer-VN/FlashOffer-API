// UpdatePartnerProductValidator.cs
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

/// <summary>
/// Validate cập nhật sản phẩm — partial update nên rule chỉ chạy khi field được gửi lên.
/// </summary>
public class UpdatePartnerProductValidator : AbstractValidator<UpdatePartnerProductDto>
{
    public UpdatePartnerProductValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["PartnerProductNameRequired"])
            .MinimumLength(2).WithMessage(localizer["PartnerProductNameMinLength"])
            .MaximumLength(200).WithMessage(localizer["PartnerProductNameMaxLength"])
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage(localizer["PartnerProductDescriptionMaxLength"])
            .When(x => x.Description != null);

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage(localizer["PartnerProductCategoryInvalid"])
            .When(x => x.Category.HasValue);

        RuleFor(x => x.RetailPrice)
            .GreaterThanOrEqualTo(0).WithMessage(localizer["PartnerProductPriceInvalid"])
            .When(x => x.RetailPrice.HasValue);

        RuleFor(x => x.WholesalePrice)
            .GreaterThanOrEqualTo(0).WithMessage(localizer["PartnerProductPriceInvalid"])
            .When(x => x.WholesalePrice.HasValue);

        RuleFor(x => x.MinOrderQuantity)
            .GreaterThanOrEqualTo(1).WithMessage(localizer["PartnerProductMinOrderQuantityInvalid"])
            .When(x => x.MinOrderQuantity.HasValue);
    }
}
