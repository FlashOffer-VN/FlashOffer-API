using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.Extensions.Localization;
using Moq;

namespace FlashOffer.API.UnitTests.Validators;

public class OfferRequestQueryValidatorTests
{
	private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
	private readonly OfferRequestQueryValidator _validator;

	public OfferRequestQueryValidatorTests()
	{
		_localizerMock = new Mock<IStringLocalizer<SharedResource>>();
		SetupLocalizer();
		_validator = new OfferRequestQueryValidator(_localizerMock.Object);
	}

	private void SetupLocalizer()
	{
		_localizerMock.Setup(l => l["PageMin"])
			.Returns(new LocalizedString("PageMin", "Page number must be at least 1"));
		_localizerMock.Setup(l => l["PageSizeRange"])
			.Returns(new LocalizedString("PageSizeRange", "Page size must be between 1 and 100"));
	}

	[Fact]
	public void Validate_ValidQuery_ShouldSucceed()
	{
		// Arrange
		var query = new OfferRequestQueryDto { Page = 1, PageSize = 10, IsOfferSent = true };

		// Act
		var result = _validator.Validate(query);

		// Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_PageLessThan1_ShouldFail()
	{
		// Arrange
		var query = new OfferRequestQueryDto { Page = 0 };

		// Act
		var result = _validator.Validate(query);

		// Assert
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "Page");
	}

	[Fact]
	public void Validate_PageSizeGreaterThan100_ShouldFail()
	{
		// Arrange
		var query = new OfferRequestQueryDto { PageSize = 200 };

		// Act
		var result = _validator.Validate(query);

		// Assert
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "PageSize");
	}

	[Fact]
	public void Validate_PageSizeLessThan1_ShouldFail()
	{
		// Arrange
		var query = new OfferRequestQueryDto { PageSize = 0 };

		// Act
		var result = _validator.Validate(query);

		// Assert
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "PageSize");
	}
}