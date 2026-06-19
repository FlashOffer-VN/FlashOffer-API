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

public class GroupBuyingRequestsControllerTests : BaseIntegrationTest
{
	public GroupBuyingRequestsControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

	[Fact]
	public async Task Create_ValidRequest_ReturnsOk()
	{
		// Arrange
		var request = new CreateGroupBuyingRequestDto
		{
			ProductName = "Test Product",
			TargetPeopleCount = 5,
			FullName = "Test User",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/group-buying-requests", request);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<GroupBuyingRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data.CurrentPeopleCount);
		Assert.Equal(5, result.Data.TargetPeopleCount);
	}

	[Fact]
	public async Task Create_MissingProductName_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateGroupBuyingRequestDto
		{
			TargetPeopleCount = 5,
			FullName = "Test User",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/group-buying-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_InvalidPhone_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateGroupBuyingRequestDto
		{
			ProductName = "Test",
			TargetPeopleCount = 3,
			FullName = "Test",
			Phone = "123"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/group-buying-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Create_TargetPeopleCountLessThan2_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateGroupBuyingRequestDto
		{
			ProductName = "Test",
			TargetPeopleCount = 1,
			FullName = "Test",
			Phone = "0978123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/group-buying-requests", request);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task GetList_WithValidParams_ReturnsOk()
	{
		// Arrange
		var url = "/api/leads/group-buying-requests?page=1&pageSize=10";

		// Act
		var response = await Client.GetAsync(url);
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<GroupBuyingRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(result);
		Assert.True(result.Success);
		Assert.NotNull(result.Data);
	}

	[Fact]
	public async Task GetList_WithStatusFilter_ReturnsOk()
	{
		// Arrange
		var url = "/api/leads/group-buying-requests?page=1&pageSize=10&status=Pending";

		// Act
		var response = await Client.GetAsync(url);
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<GroupBuyingRequestResponseDto>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(result.Success);
		// Verify chỉ có item với status Pending (có thể kiểm tra thêm)
	}

	[Fact]
	public async Task GetList_WithInvalidStatus_ReturnsBadRequest()
	{
		// Arrange
		var url = "/api/leads/group-buying-requests?page=1&pageSize=10&status=InvalidStatus";

		// Act
		var response = await Client.GetAsync(url);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}
}