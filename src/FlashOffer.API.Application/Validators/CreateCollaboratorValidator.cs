using FluentValidation;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreateCollaboratorValidator : AbstractValidator<CreateCollaboratorDto>
{
    public CreateCollaboratorValidator(IStringLocalizer<SharedResource> localizer)
    {
        // Thông tin cá nhân
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["Collaborator_FullNameRequired"])
            .MaximumLength(200).WithMessage(localizer["Collaborator_FullNameMaxLength"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["Collaborator_PhoneRequired"])
            .Must(phone => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[0-9]{9,10}$"))
            .WithMessage(localizer["Collaborator_PhoneInvalid"]);

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(localizer["Collaborator_EmailInvalid"]);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(localizer["Collaborator_UsernameRequired"])
            .MinimumLength(3).WithMessage(localizer["Collaborator_UsernameMinLength"])
            .MaximumLength(50).WithMessage(localizer["Collaborator_UsernameMaxLength"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["Collaborator_PasswordRequired"])
            .MinimumLength(6).WithMessage(localizer["Collaborator_PasswordMinLength"])
            .MaximumLength(100).WithMessage(localizer["Collaborator_PasswordMaxLength"])
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
            .WithMessage(localizer["Collaborator_PasswordComplexity"]);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage(localizer["Collaborator_PasswordMismatch"])
            .When(x => !string.IsNullOrEmpty(x.ConfirmPassword));

        RuleFor(x => x.AgreeTerms)
            .Must(x => x == true).WithMessage(localizer["Collaborator_AgreeTermsRequired"]);

        // Thông tin doanh nghiệp
        RuleFor(x => x.BusinessName)
            .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.BusinessName))
            .WithMessage(localizer["Collaborator_BusinessNameMaxLength"]);

        RuleFor(x => x.BusinessSize)
            .InclusiveBetween(1, 1000).When(x => x.BusinessSize.HasValue)
            .WithMessage(localizer["Collaborator_BusinessSizeInvalid"]);

        RuleFor(x => x.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage(localizer["Collaborator_WebsiteInvalid"]);

        // ParentCollaboratorId: cho phép null, chỉ validate khi có value
        RuleFor(x => x.ParentCollaboratorId)
            .Must(id => id == null || id != Guid.Empty)
            .When(x => x.ParentCollaboratorId.HasValue)
            .WithMessage(localizer["Collaborator_ParentIdInvalid"]);
    }
}