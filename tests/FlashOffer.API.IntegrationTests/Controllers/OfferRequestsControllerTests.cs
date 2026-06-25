using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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

	[Fact]
	public async Task GetList_WithDefaultPaging_ReturnsOk()
	{
		// Arrange - seed data
		var requests = new[]
		{
		new OfferRequest { SelectedOffer = "Offer 1", FullName = "A", Phone = "0912345678", IsOfferSent = false, CreatedAt = DateTime.UtcNow.AddHours(7) },
		new OfferRequest { SelectedOffer = "Offer 2", FullName = "B", Phone = "0912345679", IsOfferSent = true, CreatedAt = DateTime.UtcNow.AddHours(7).AddMinutes(-1) }
	};
		await DbContext.OfferRequests.AddRangeAsync(requests);
		await DbContext.SaveChangesAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/offer-requests");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<OfferRequestResponseDto>>();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.Data.Should().HaveCount(2);
		result.TotalCount.Should().Be(2);
		result.PageNumber.Should().Be(1);
	}

	[Fact]
	public async Task GetList_FilterByIsOfferSent_ReturnsFiltered()
	{
		// Arrange
		await DbContext.OfferRequests.AddRangeAsync(
			new OfferRequest { SelectedOffer = "Offer 1", FullName = "A", Phone = "0912345678", IsOfferSent = true, CreatedAt = DateTime.UtcNow.AddHours(7) },
			new OfferRequest { SelectedOffer = "Offer 2", FullName = "B", Phone = "0912345679", IsOfferSent = false, CreatedAt = DateTime.UtcNow.AddHours(7) }
		);
		await DbContext.SaveChangesAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/offer-requests?isOfferSent=true");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<OfferRequestResponseDto>>();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Data.Should().AllSatisfy(x => x.IsOfferSent.Should().BeTrue());
		result.TotalCount.Should().Be(1);
	}

	[Fact]
	public async Task GetList_WithInvalidPage_ReturnsBadRequest()
	{
		// Act
		var response = await Client.GetAsync("/api/leads/offer-requests?page=0");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}