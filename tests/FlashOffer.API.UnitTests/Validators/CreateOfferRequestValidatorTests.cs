using FluentValidation.TestHelper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace FlashOffer.API.UnitTests.Validators;

public class CreateOfferRequestValidatorTests
{
	private readonly CreateOfferRequestValidator _validator;
	private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;

	public CreateOfferRequestValidatorTests()
	{
		_localizerMock = new Mock<IStringLocalizer<SharedResource>>();
		SetupLocalizer();
		_validator = new CreateOfferRequestValidator(_localizerMock.Object);
	}

	private void SetupLocalizer()
	{
		_localizerMock.Setup(l => l["SelectedOfferRequired"]).Returns(new LocalizedString("SelectedOfferRequired", "Offer được chọn là bắt buộc"));
		_localizerMock.Setup(l => l["SelectedOfferMaxLength"]).Returns(new LocalizedString("SelectedOfferMaxLength", "Offer không vượt quá 500 ký tự"));
		_localizerMock.Setup(l => l["FullNameRequired"]).Returns(new LocalizedString("FullNameRequired", "Họ tên không được để trống"));
		_localizerMock.Setup(l => l["FullNameMaxLength"]).Returns(new LocalizedString("FullNameMaxLength", "Họ tên tối đa 200 ký tự"));
		_localizerMock.Setup(l => l["PhoneRequired"]).Returns(new LocalizedString("PhoneRequired", "Số điện thoại không được để trống"));
		_localizerMock.Setup(l => l["PhoneInvalid"]).Returns(new LocalizedString("PhoneInvalid", "Số điện thoại phải có 10-11 chữ số"));
		_localizerMock.Setup(l => l["ZaloRequired"]).Returns(new LocalizedString("ZaloRequired", "Zalo là bắt buộc"));
		_localizerMock.Setup(l => l["ZaloMaxLength"]).Returns(new LocalizedString("ZaloMaxLength", "Zalo không vượt quá 50 ký tự"));
		_localizerMock.Setup(l => l["EmailInvalid"]).Returns(new LocalizedString("EmailInvalid", "Email không đúng định dạng"));
	}

	[Fact]
	public void Should_Have_Error_When_SelectedOffer_Empty()
	{
		var model = new CreateOfferRequestDto { SelectedOffer = "", FullName = "Test", Phone = "0978123456", Zalo = "test" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.SelectedOffer);
	}

	[Fact]
	public void Should_Have_Error_When_Zalo_Empty()
	{
		var model = new CreateOfferRequestDto { Zalo = "", SelectedOffer = "Test", FullName = "Test", Phone = "0978123456" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Zalo);
	}

	[Fact]
	public void Should_Have_Error_When_Phone_Invalid()
	{
		var model = new CreateOfferRequestDto { Phone = "123", SelectedOffer = "Test", FullName = "Test", Zalo = "test" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Should_Have_Error_When_Email_Invalid()
	{
		var model = new CreateOfferRequestDto { Email = "invalid-email", SelectedOffer = "Test", FullName = "Test", Zalo = "test", Phone = "0978123456" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email);
	}

	[Fact]
	public void Should_Not_Have_Error_When_Valid()
	{
		var model = new CreateOfferRequestDto
		{
			SelectedOffer = "Giảm 50%",
			FullName = "Lê Văn C",
			Phone = "0905123456",
			Zalo = "lec",
			Email = "c@gmail.com"
		};
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveAnyValidationErrors();
	}
}