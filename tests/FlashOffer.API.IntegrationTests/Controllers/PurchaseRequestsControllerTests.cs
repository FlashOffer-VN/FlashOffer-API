using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class PurchaseRequestsControllerTests : BaseIntegrationTest
{
	private const string BaseUrl = "/api/leads/purchase-requests";

	public PurchaseRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
	{
	}

	#region Create Tests

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
		var response = await Client.PostAsJsonAsync(BaseUrl, request);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(result);
		Assert.True(result!.Success);
		Assert.NotNull(result.Data);
		Assert.Equal(request.ProductName, result.Data!.ProductName);
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
		var response = await Client.PostAsJsonAsync(BaseUrl, request);

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
		var response = await Client.PostAsJsonAsync(BaseUrl, request);

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
		var response = await Client.PostAsJsonAsync(BaseUrl, request);

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
		var response = await Client.PostAsJsonAsync(BaseUrl, request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	#endregion

	#region GetList Tests

	[Fact]
	public async Task GetList_ShouldReturnPagedResult()
	{
		// Arrange
		await SetAdminAuthorization();
		await SeedPurchaseRequestsAsync();

		// Act
		var response = await Client.GetAsync($"{BaseUrl}?page=1&pageSize=10");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(result);
		Assert.True(result!.Success);
		Assert.NotNull(result.Data);
		Assert.Equal(1, result.PageNumber);
	}

	[Fact]
	public async Task GetList_FilterByStatus_ShouldReturnFiltered()
	{
		// Arrange
		await SetAdminAuthorization();
		await SeedPurchaseRequestsAsync();

		// Act
		var response = await Client.GetAsync($"{BaseUrl}?status=Pending");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(result);
		Assert.All(result!.Data, item => Assert.Equal(PurchaseRequestStatus.Pending, item.Status));
	}

	#endregion

	#region UpdateStatus Tests

	[Fact]
	public async Task UpdateStatus_ValidRequest_ReturnsOk()
	{
		// Arrange
		await SetAdminAuthorization();
		var entity = await CreateTestPurchaseRequest();
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
		await SetAdminAuthorization();
		var entity = await CreateTestPurchaseRequest();
		var dto = new UpdatePurchaseRequestStatusDto { Status = (PurchaseRequestStatus)99 };

		// Act
		var response = await Client.PatchAsJsonAsync($"{BaseUrl}/{entity.Id}/status", dto);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task UpdateStatus_NotFound_ReturnsNotFound()
	{
		// Arrange
		await SetAdminAuthorization();
		var id = Guid.NewGuid();
		var dto = new UpdatePurchaseRequestStatusDto { Status = PurchaseRequestStatus.Contacted };

		// Act
		var response = await Client.PatchAsJsonAsync($"{BaseUrl}/{id}/status", dto);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	#endregion

	#region Helper Methods

	private async Task SeedPurchaseRequestsAsync()
	{
		using var scope = Factory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		dbContext.PurchaseRequests.RemoveRange(dbContext.PurchaseRequests);
		await dbContext.SaveChangesAsync();

		var requests = new List<PurchaseRequest>
		{
			new()
			{
				Id = Guid.NewGuid(),
				ProductName = "iPhone 15 Pro Max",
				Quantity = 2,
				ExpectedPrice = 28000000,
				FullName = "Nguyễn Văn A",
				Phone = "0987654321",
				Email = "a@gmail.com",
				Note = "Cần hàng mới 100%",
				Status = PurchaseRequestStatus.Pending,
				CreatedAt = DateTime.UtcNow
			},
			new()
			{
				Id = Guid.NewGuid(),
				ProductName = "Samsung Galaxy S24",
				Quantity = 1,
				ExpectedPrice = 20000000,
				FullName = "Trần Văn B",
				Phone = "0978123456",
				Email = "b@gmail.com",
				Note = "Màu đen",
				Status = PurchaseRequestStatus.Contacted,
				CreatedAt = DateTime.UtcNow.AddHours(-1)
			},
			new()
			{
				Id = Guid.NewGuid(),
				ProductName = "Laptop Dell XPS",
				Quantity = 3,
				ExpectedPrice = 30000000,
				FullName = "Lê Thị C",
				Phone = "0965123456",
				Email = "c@gmail.com",
				Note = "Cần cấu hình cao",
				Status = PurchaseRequestStatus.Completed,
				CreatedAt = DateTime.UtcNow.AddHours(-2)
			}
		};

		await dbContext.PurchaseRequests.AddRangeAsync(requests);
		await dbContext.SaveChangesAsync();
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

	#endregion
}