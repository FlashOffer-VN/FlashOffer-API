// tests/FlashOffer.API.IntegrationTests/Controllers/PurchaseRequestsControllerTests.cs
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class PurchaseRequestsControllerTests : BaseIntegrationTest
{
	private const string BaseUrl = "/api/leads/purchase-requests";

	public PurchaseRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task UpdateStatus_ValidRequest_ReturnsOk()
	{
		// Arrange
		var entity = await CreateTestPurchaseRequest();
		Console.WriteLine($"✅ Created entity with Id: {entity.Id}");

		// Kiểm tra tồn tại trong DB của test
		var exists = await DbContext.PurchaseRequests.AnyAsync(x => x.Id == entity.Id);
		Console.WriteLine($"✅ Exists in test DbContext: {exists}");

		// Kiểm tra tồn tại qua API (GET)
		var getResponse = await Client.GetAsync($"{BaseUrl}/{entity.Id}");
		Console.WriteLine($"✅ GET /{entity.Id} status: {getResponse.StatusCode}");

		var dto = new UpdatePurchaseRequestStatusDto { Status = PurchaseRequestStatus.Contacted };

		// Act
		var response = await Client.PatchAsJsonAsync($"{BaseUrl}/{entity.Id}/status", dto);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<PurchaseRequestStatusResponseDto>>();
		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.Data!.Status.Should().Be(PurchaseRequestStatus.Contacted);
	}

	[Fact]
	public async Task UpdateStatus_InvalidStatus_ReturnsBadRequest()
	{
		// Arrange
		var entity = await CreateTestPurchaseRequest();
		var dto = new UpdatePurchaseRequestStatusDto { Status = (PurchaseRequestStatus)99 };

		// Act
		var response = await Client.PatchAsJsonAsync($"{BaseUrl}/{entity.Id}/status", dto);
		var content = await response.Content.ReadAsStringAsync();
		Console.WriteLine($"Response: {content}"); // Xem message lỗi chi tiết
		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task UpdateStatus_NotFound_ReturnsNotFound()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new UpdatePurchaseRequestStatusDto { Status = PurchaseRequestStatus.Contacted };

		// Act
		var response = await Client.PatchAsJsonAsync($"{BaseUrl}/{id}/status", dto);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	private async Task<PurchaseRequest> CreateTestPurchaseRequest()
	{
		var entity = new PurchaseRequest
		{
			ProductName = "Test Product",
			Quantity = 3,
			FullName = "Test User",
			Phone = "0987654321",
			Status = PurchaseRequestStatus.Pending
		};

		await DbContext.PurchaseRequests.AddAsync(entity);
		await DbContext.SaveChangesAsync();

		return entity; 
	}
}