using FlashOffer.API.Shared.Exceptions;
using FlashOffer.API.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Shared.Exceptions;

public static class UserException
{
    public static FlashOfferException PhoneRequired(IStringLocalizer<ExceptionMessages> localizer)
        => new FlashOfferException(
            statusCode: "USER_PHONE_REQUIRED",
            message: localizer["User_PhoneRequired"]
        );

    public static FlashOfferException PhoneAlreadyExists(IStringLocalizer<ExceptionMessages> localizer, string phone)
        => new FlashOfferException(
            statusCode: "USER_PHONE_EXISTS",
            message: localizer["User_PhoneAlreadyExists", phone],
            additionalData: new { Phone = phone }
        );

    public static FlashOfferException EmailAlreadyExists(IStringLocalizer<ExceptionMessages> localizer, string email)
        => new FlashOfferException(
            statusCode: "USER_EMAIL_EXISTS",
            message: localizer["User_EmailAlreadyExists", email],
            additionalData: new { Email = email }
        );
}