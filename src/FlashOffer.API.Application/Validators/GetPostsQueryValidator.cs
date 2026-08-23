using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage(localizer["Social_PageNumberInvalid"]);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50).WithMessage(localizer["Social_PageSizeInvalid"]);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage(localizer["Social_TypeInvalid"])
            .When(x => x.Type.HasValue);

        RuleFor(x => x.Privacy)
            .IsInEnum().WithMessage(localizer["Social_PrivacyInvalid"])
            .When(x => x.Privacy.HasValue);

        RuleFor(x => x.Tag)
            .MaximumLength(50).WithMessage(localizer["Social_TagMaxLength"])
            .When(x => !string.IsNullOrEmpty(x.Tag));
    }
}