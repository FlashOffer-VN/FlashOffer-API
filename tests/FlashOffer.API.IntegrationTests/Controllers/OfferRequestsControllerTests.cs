using Microsoft.AspNetCore.Mvc.Testing;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Responses;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class OfferRequestsControllerTests : BaseIntegrationTest
{
	public OfferRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

	[Fact]
	public async Task Create_ValidRequest_ReturnsOk()
	{
		// Arrange
		var request = new CreateOfferRequestDto
		{
			SelectedOffer = "Giảm 50% tai nghe Sony",
			FullName = "Lê Văn C",
			Phone = "0905123456",
			Zalo = "lec",
			Email = "c@gmail.com"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/offer-requests", request);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<OfferRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(result);
		Assert.True(result.Success);
		Assert.NotNull(result.Data);
		Assert.False(result.Data.IsOfferSent);
	}

	[Fact]
	public async Task Create_MissingZalo_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateOfferRequestDto
		{
			SelectedOffer = "Test",
			FullName = "Test",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/offer-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_InvalidPhone_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateOfferRequestDto
		{
			SelectedOffer = "Test",
			FullName = "Test",
			Phone = "123",
			Zalo = "test"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/offer-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}
}