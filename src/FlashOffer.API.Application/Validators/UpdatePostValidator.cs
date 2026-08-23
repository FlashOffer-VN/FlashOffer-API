using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.Validators;

public class UpdatePostValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Content)
            .MaximumLength(5000).WithMessage(localizer["Social_ContentMaxLength"])
            .When(x => !string.IsNullOrEmpty(x.Content));

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage(localizer["Social_TitleMaxLength"])
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage(localizer["Social_TypeInvalid"])
            .When(x => x.Type.HasValue);

        RuleFor(x => x.Privacy)
            .IsInEnum().WithMessage(localizer["Social_PrivacyInvalid"])
            .When(x => x.Privacy.HasValue);

        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.Count <= 5).WithMessage(localizer["Social_TagsMaxCount"])
            .When(x => x.Tags != null);

        RuleForEach(x => x.Tags)
            .MaximumLength(20).WithMessage(localizer["Social_TagMaxLength"])
            .When(x => x.Tags != null && x.Tags.Any());

        RuleFor(x => x.Images)
            .Must(images => images == null || images.Count <= 5).WithMessage(localizer["Social_ImagesMaxCount"])
            .When(x => x.Images != null);

        // Event validation
        When(x => x.Type == PostType.Event, () =>
        {
            RuleFor(x => x.EventDate)
                .GreaterThan(DateTime.UtcNow).WithMessage(localizer["Social_EventDateFuture"])
                .When(x => x.EventDate.HasValue);
        });

        // Announcement validation
        When(x => x.Type == PostType.Announcement, () =>
        {
            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage(localizer["Social_PriorityInvalid"])
                .When(x => x.Priority.HasValue);

            RuleFor(x => x.PinnedUntil)
                .GreaterThan(DateTime.UtcNow).WithMessage(localizer["Social_PinnedUntilFuture"])
                .When(x => x.PinnedUntil.HasValue);
        });
    }
}