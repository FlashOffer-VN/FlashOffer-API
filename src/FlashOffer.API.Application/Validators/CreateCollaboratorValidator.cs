using FluentValidation;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreateCollaboratorValidator : AbstractValidator<CreateCollaboratorDto>
{
    public CreateCollaboratorValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        RuleFor(x => x.AgreeTerms).Must(x => x == true);

        // ParentCollaboratorId KHÔNG bắt buộc
        RuleFor(x => x.ParentCollaboratorId)
            .Must(id => id == null || id != Guid.Empty)
            .When(x => x.ParentCollaboratorId.HasValue);
    }
}