using FluentValidation.TestHelper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace FlashOffer.API.UnitTests.Validators;

public class CreatePurchaseRequestValidatorTests
{
	private readonly CreatePurchaseRequestValidator _validator;
	private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;

	public CreatePurchaseRequestValidatorTests()
	{
		_localizerMock = new Mock<IStringLocalizer<SharedResource>>();
		SetupLocalizer();
		_validator = new CreatePurchaseRequestValidator(_localizerMock.Object);
	}

	private void SetupLocalizer()
	{
		_localizerMock.Setup(l => l["ProductNameRequired"]).Returns(new LocalizedString("ProductNameRequired", "Tên sản phẩm không được để trống"));
		_localizerMock.Setup(l => l["ProductNameMaxLength"]).Returns(new LocalizedString("ProductNameMaxLength", "Tên sản phẩm tối đa 500 ký tự"));
		_localizerMock.Setup(l => l["QuantityMin"]).Returns(new LocalizedString("QuantityMin", "Số lượng phải lớn hơn hoặc bằng 1"));
		_localizerMock.Setup(l => l["FullNameRequired"]).Returns(new LocalizedString("FullNameRequired", "Họ tên không được để trống"));
		_localizerMock.Setup(l => l["FullNameMaxLength"]).Returns(new LocalizedString("FullNameMaxLength", "Họ tên tối đa 200 ký tự"));
		_localizerMock.Setup(l => l["PhoneRequired"]).Returns(new LocalizedString("PhoneRequired", "Số điện thoại không được để trống"));
		_localizerMock.Setup(l => l["PhoneInvalid"]).Returns(new LocalizedString("PhoneInvalid", "Số điện thoại phải có 10-11 chữ số"));
		_localizerMock.Setup(l => l["EmailInvalid"]).Returns(new LocalizedString("EmailInvalid", "Email không đúng định dạng"));
		_localizerMock.Setup(l => l["ExpectedPricePositive"]).Returns(new LocalizedString("ExpectedPricePositive", "Giá kỳ vọng phải lớn hơn 0"));
	}

	[Fact]
	public void Should_Have_Error_When_ProductName_Empty()
	{
		var model = new CreatePurchaseRequestDto { ProductName = "", FullName = "Test", Phone = "0978123456", Quantity = 1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.ProductName);
	}

	[Fact]
	public void Should_Have_Error_When_Quantity_LessThan1()
	{
		var model = new CreatePurchaseRequestDto { Quantity = 0, ProductName = "Test", FullName = "Test", Phone = "0978123456" };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Quantity);
	}

	[Fact]
	public void Should_Have_Error_When_Phone_Invalid()
	{
		var model = new CreatePurchaseRequestDto { Phone = "123", ProductName = "Test", FullName = "Test", Quantity = 1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Phone);
	}

	[Fact]
	public void Should_Have_Error_When_Email_Invalid()
	{
		var model = new CreatePurchaseRequestDto { Email = "invalid", ProductName = "Test", FullName = "Test", Phone = "0978123456", Quantity = 1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.Email);
	}

	[Fact]
	public void Should_Have_Error_When_ExpectedPrice_Negative()
	{
		var model = new CreatePurchaseRequestDto { ExpectedPrice = -100, ProductName = "Test", FullName = "Test", Phone = "0978123456", Quantity = 1 };
		var result = _validator.TestValidate(model);
		result.ShouldHaveValidationErrorFor(x => x.ExpectedPrice);
	}

	[Fact]
	public void Should_Not_Have_Error_When_Valid()
	{
		var model = new CreatePurchaseRequestDto
		{
			ProductName = "Máy lạnh",
			Quantity = 2,
			ExpectedPrice = 5000000,
			FullName = "Nguyễn Văn A",
			Phone = "0978123456",
			Email = "a@gmail.com"
		};
		var result = _validator.TestValidate(model);
		result.ShouldNotHaveAnyValidationErrors();
	}
}