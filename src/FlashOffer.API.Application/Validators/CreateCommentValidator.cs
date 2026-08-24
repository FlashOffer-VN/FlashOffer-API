using FluentValidation;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(localizer["SocialComment_ContentRequired"])
            .MaximumLength(1000).WithMessage(localizer["SocialComment_ContentMaxLength"]);
    }
}