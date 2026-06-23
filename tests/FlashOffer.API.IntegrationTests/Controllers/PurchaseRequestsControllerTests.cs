using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;
\using System.Linq.Expressions

namespace FlashOffer.API.IntegrationTests.Controllers;

public class PurchaseRequestsControllerTests : BaseIntegrationTest
{
	public PurchaseRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

	private async Task SeedPurchaseRequestsAsync()
	{
		using var scope = Factory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		// Xóa dữ liệu cũ
		dbContext.PurchaseRequests.RemoveRange(dbContext.PurchaseRequests);
		await dbContext.SaveChangesAsync();

		// Seed 3 records
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

	[Fact]
	public async Task GetList_ShouldReturnPagedResult()
	{
		// Arrange - seed data
		await SeedPurchaseRequestsAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/purchase-requests?page=1&pageSize=10");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(result.Success);
		Assert.NotNull(result.Data);
		Assert.Equal(1, result.PageNumber);
	}

	[Fact]
	public async Task GetList_FilterByStatus_ShouldReturnFiltered()
	{
		// Arrange
		await SeedPurchaseRequestsAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/purchase-requests?status=Pending");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<PurchaseRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.All(result.Data, item => Assert.Equal(PurchaseRequestStatus.Pending, item.Status));
	}
}