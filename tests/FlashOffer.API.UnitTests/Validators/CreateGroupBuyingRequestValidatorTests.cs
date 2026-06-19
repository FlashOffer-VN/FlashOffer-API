using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Validators;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;
using Moq;

namespace FlashOffer.API.UnitTests.Validators;

public class CreateGroupBuyingRequestValidatorTests
{
	private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
	private readonly CreateGroupBuyingRequestValidator _validator;

	public CreateGroupBuyingRequestValidatorTests()
	{
		_localizerMock = new Mock<IStringLocalizer<SharedResource>>();

		_localizerMock.Setup(l => l["ProductNameRequired"]).Returns(new LocalizedString("ProductNameRequired", "Product name is required"));
		_localizerMock.Setup(l => l["ProductNameMaxLength"]).Returns(new LocalizedString("ProductNameMaxLength", "Product name max 500"));
		_localizerMock.Setup(l => l["TargetPeopleCountInvalid"]).Returns(new LocalizedString("TargetPeopleCountInvalid", "Target people count must be between 2 and 100"));
		_localizerMock.Setup(l => l["FullNameRequired"]).Returns(new LocalizedString("FullNameRequired", "Full name is required"));
		_localizerMock.Setup(l => l["FullNameMaxLength"]).Returns(new LocalizedString("FullNameMaxLength", "Full name max 200"));
		_localizerMock.Setup(l => l["PhoneRequired"]).Returns(new LocalizedString("PhoneRequired", "Phone is required"));
		_localizerMock.Setup(l => l["PhoneInvalid"]).Returns(new LocalizedString("PhoneInvalid", "Phone is invalid"));
		_localizerMock.Setup(l => l["TargetPriceInvalid"]).Returns(new LocalizedString("TargetPriceInvalid", "Target price must be greater than 0"));
		_localizerMock.Setup(l => l["NoteMaxLength"]).Returns(new LocalizedString("NoteMaxLength", "Note max 1000"));

		_validator = new CreateGroupBuyingRequestValidator(_localizerMock.Object);
	}

	[Fact]
	public void Validate_ValidDto_ShouldSucceed()
	{
		var dto = new CreateGroupBuyingRequestDto
		{
			ProductName = "Laptop",
			TargetPeopleCount = 5,
			FullName = "Test User",
			Phone = "0978123456"
		};
		var result = _validator.Validate(dto);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_ProductNameEmpty_ShouldFail()
	{
		var dto = new CreateGroupBuyingRequestDto
		{
			ProductName = "",
			TargetPeopleCount = 5
		};
		var result = _validator.Validate(dto);
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "ProductName");
	}

	[Fact]
	public void Validate_TargetPeopleCountZero_ShouldFail()
	{
		var dto = new CreateGroupBuyingRequestDto
		{
			ProductName = "Laptop",
			TargetPeopleCount = 0
		};
		var result = _validator.Validate(dto);
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "TargetPeopleCount");
	}

	[Fact]
	public void Validate_FullNameEmpty_ShouldFail()
	{
		var dto = new CreateGroupBuyingRequestDto
		{
			ProductName = "Laptop",
			TargetPeopleCount = 5,
			FullName = "",
			Phone = "0978123456"
		};
		var result = _validator.Validate(dto);
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "FullName");
	}

	[Fact]
	public void Validate_PhoneInvalid_ShouldFail()
	{
		var dto = new CreateGroupBuyingRequestDto
		{
			ProductName = "Laptop",
			TargetPeopleCount = 5,
			FullName = "Test User",
			Phone = "123"
		};
		var result = _validator.Validate(dto);
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "Phone");
	}
}