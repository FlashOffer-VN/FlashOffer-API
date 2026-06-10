using Microsoft.AspNetCore.Mvc.Testing;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Responses;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class PurchaseRequestsControllerTests : BaseIntegrationTest
{
	public PurchaseRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

	[Fact]
	public async Task Create_ValidRequest_ReturnsOk()
	{
		// Arrange
		var request = new CreatePurchaseRequestDto
		{
			ProductName = "Máy lạnh Daikin",
			Quantity = 2,
			ExpectedPrice = 5000000,
			FullName = "Nguyễn Văn A",
			Phone = "0978123456",
			Email = "a@gmail.com",
			Note = "Giao hàng giờ hành chính"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/purchase-requests", request);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(result.Success);
		Assert.Equal(request.ProductName, result.Data.ProductName);
		Assert.Equal(request.Quantity, result.Data.Quantity);
	}

	[Fact]
	public async Task Create_MissingProductName_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreatePurchaseRequestDto
		{
			Quantity = 2,
			FullName = "Nguyễn Văn A",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/purchase-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_QuantityLessThan1_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreatePurchaseRequestDto
		{
			ProductName = "Test",
			Quantity = 0,
			FullName = "Test",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/purchase-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_InvalidPhone_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreatePurchaseRequestDto
		{
			ProductName = "Test",
			Quantity = 1,
			FullName = "Test",
			Phone = "123"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/purchase-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_InvalidEmail_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreatePurchaseRequestDto
		{
			ProductName = "Test",
			Quantity = 1,
			FullName = "Test",
			Phone = "0978123456",
			Email = "invalid-email"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/purchase-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}
}