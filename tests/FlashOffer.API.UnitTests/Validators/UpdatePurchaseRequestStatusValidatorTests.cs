// tests/FlashOffer.API.UnitTests/Validators/UpdatePurchaseRequestStatusValidatorTests.cs
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using FlashOffer.API.Domain.Enums;
using Microsoft.Extensions.Localization;
using Moq;

namespace FlashOffer.API.UnitTests.Validators;

public class UpdatePurchaseRequestStatusValidatorTests
{
	private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;
	private readonly UpdatePurchaseRequestStatusValidator _validator;

	public UpdatePurchaseRequestStatusValidatorTests()
	{
		_localizerMock = new Mock<IStringLocalizer<SharedResource>>();
		_localizerMock.Setup(l => l["StatusInvalid"])
			.Returns(new LocalizedString("StatusInvalid", "Status must be Pending, Contacted, or Completed"));

		_validator = new UpdatePurchaseRequestStatusValidator(_localizerMock.Object);
	}

	[Fact]
	public void Validate_ValidStatus_ShouldSucceed()
	{
		var dto = new UpdatePurchaseRequestStatusDto { Status = PurchaseRequestStatus.Contacted };
		var result = _validator.Validate(dto);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_InvalidStatus_ShouldFail()
	{
		var dto = new UpdatePurchaseRequestStatusDto { Status = (PurchaseRequestStatus)99 };
		var result = _validator.Validate(dto);
		Assert.False(result.IsValid);
		Assert.Contains(result.Errors, e => e.PropertyName == "Status");
	}
}