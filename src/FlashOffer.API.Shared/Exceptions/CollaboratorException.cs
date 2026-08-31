using FlashOffer.API.Shared.Exceptions;
using FlashOffer.API.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Shared.Exceptions;

public static class CollaboratorException
{
    public static FlashOfferException NotFound(IStringLocalizer<ExceptionMessages> localizer, Guid id)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_NOT_FOUND",
            message: localizer["Collaborator_NotFound", id],
            additionalData: new { Id = id }
        );

    public static FlashOfferException EmailAlreadyExists(IStringLocalizer<ExceptionMessages> localizer, string email)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_EMAIL_EXISTS",
            message: localizer["Collaborator_EmailAlreadyExists", email],
            additionalData: new { Email = email }
        );

    public static FlashOfferException PhoneAlreadyExists(IStringLocalizer<ExceptionMessages> localizer, string phone)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_PHONE_EXISTS",
            message: localizer["Collaborator_PhoneAlreadyExists", phone],
            additionalData: new { Phone = phone }
        );

    public static FlashOfferException UserAlreadyExists(IStringLocalizer<ExceptionMessages> localizer, Guid userId)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_USER_EXISTS",
            message: localizer["Collaborator_UserAlreadyExists", userId],
            additionalData: new { UserId = userId }
        );

    public static FlashOfferException ParentNotFound(IStringLocalizer<ExceptionMessages> localizer, Guid parentId)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_PARENT_NOT_FOUND",
            message: localizer["Collaborator_ParentNotFound", parentId],
            additionalData: new { ParentId = parentId }
        );

    public static FlashOfferException ParentNotApproved(IStringLocalizer<ExceptionMessages> localizer, Guid parentId)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_PARENT_NOT_APPROVED",
            message: localizer["Collaborator_ParentNotApproved", parentId],
            additionalData: new { ParentId = parentId }
        );

    public static FlashOfferException LevelExceeded(IStringLocalizer<ExceptionMessages> localizer, int maxLevel)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_LEVEL_EXCEEDED",
            message: localizer["Collaborator_LevelExceeded", maxLevel],
            additionalData: new { MaxLevel = maxLevel }
        );

    public static FlashOfferException CircularReference(IStringLocalizer<ExceptionMessages> localizer, Guid parentId)
        => new FlashOfferException(
            statusCode: "COLLABORATOR_CIRCULAR_REFERENCE",
            message: localizer["Collaborator_CircularReference", parentId],
            additionalData: new { ParentId = parentId }
        );
}