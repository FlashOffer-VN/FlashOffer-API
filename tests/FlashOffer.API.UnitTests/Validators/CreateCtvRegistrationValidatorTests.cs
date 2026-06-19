using FluentValidation.TestHelper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.Extensions.Localization;
using Moq;

namespace FlashOffer.API.UnitTests.Validators;

public class CreateCtvRegistrationValidatorTests
{
	private readonly CreateCtvRegistrationValidator _validator;

	public CreateCtvRegistrationValidatorTests()
	{
		var localizerMock = new Mock<IStringLocalizer<SharedResource>>();

		localizerMock.Setup(l => l["FullNameRequired"]).Returns(new LocalizedString("FullNameRequired", "Full name is required"));
		localizerMock.Setup(l => l["FullNameMaxLength"]).Returns(new LocalizedString("FullNameMaxLength", "Full name max 200"));
		localizerMock.Setup(l => l["PhoneRequired"]).Returns(new LocalizedString("PhoneRequired", "Phone is required"));
		localizerMock.Setup(l => l["PhoneInvalid"]).Returns(new LocalizedString("PhoneInvalid", "Phone is invalid"));
		localizerMock.Setup(l => l["ZaloMaxLength"]).Returns(new LocalizedString("ZaloMaxLength", "Zalo max 50"));
		localizerMock.Setup(l => l["EmailMaxLength"]).Returns(new LocalizedString("EmailMaxLength", "Email max 100"));
		localizerMock.Setup(l => l["EmailInvalid"]).Returns(new LocalizedString("EmailInvalid", "Email invalid"));
		localizerMock.Setup(l => l["SalesChannelMaxLength"]).Returns(new LocalizedString("SalesChannelMaxLength", "Sales channel max 100"));
		localizerMock.Setup(l => l["ExperienceMaxLength"]).Returns(new LocalizedString("ExperienceMaxLength", "Experience max 1000"));

		_validator = new CreateCtvRegistrationValidator(localizerMock.Object);
	}

	[Fact]
	public void Should_HaveError_When_FullNameIsEmpty()
	{
		var model = new CreateCtvRegistrationDto
		{
			FullName = "",
			Phone = "0933123456"  // Thêm dòng này
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.FullName);
	}

	[Fact]
	public void Should_HaveError_When_PhoneIsEmpty()
	{
		var model = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",  // Thêm FullName hợp lệ
			Phone = ""
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Should_HaveError_When_PhoneIsInvalid()
	{
		var model = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",  // Thêm dòng này
			Phone = "12345"
		};
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Should_NotHaveError_When_ValidRequest()
	{
		var model = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",
			Phone = "0933123456"
		};
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveAnyValidationErrors();
	}
}