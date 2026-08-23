using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(localizer["Social_ContentRequired"])
            .MaximumLength(5000).WithMessage(localizer["Social_ContentMaxLength"]);

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage(localizer["Social_TitleMaxLength"])
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage(localizer["Social_TypeInvalid"]);

        RuleFor(x => x.Privacy)
            .IsInEnum().WithMessage(localizer["Social_PrivacyInvalid"]);

        RuleFor(x => x.Tags)
            .Must(tags => tags.Count <= 5).WithMessage(localizer["Social_TagsMaxCount"])
            .When(x => x.Tags.Any());

        RuleForEach(x => x.Tags)
            .MaximumLength(20).WithMessage(localizer["Social_TagMaxLength"])
            .When(x => x.Tags.Any());

        RuleFor(x => x.Images)
            .Must(images => images.Count <= 5).WithMessage(localizer["Social_ImagesMaxCount"])
            .When(x => x.Images.Any());

        // Event validation
        When(x => x.Type == Domain.Enums.PostType.Event, () =>
        {
            RuleFor(x => x.EventDate)
                .NotNull().WithMessage(localizer["Social_EventDateRequired"])
                .GreaterThan(DateTime.UtcNow).WithMessage(localizer["Social_EventDateFuture"])
                .When(x => x.EventDate.HasValue);
        });

        // Announcement validation
        When(x => x.Type == Domain.Enums.PostType.Announcement, () =>
        {
            RuleFor(x => x.Priority)
                .NotNull().WithMessage(localizer["Social_PriorityRequired"])
                .IsInEnum().WithMessage(localizer["Social_PriorityInvalid"])
                .When(x => x.Priority.HasValue);

            RuleFor(x => x.PinnedUntil)
                .GreaterThan(DateTime.UtcNow).WithMessage(localizer["Social_PinnedUntilFuture"])
                .When(x => x.PinnedUntil.HasValue);
        });
    }
}