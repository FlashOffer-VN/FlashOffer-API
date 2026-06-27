using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class CtvRegistrationsControllerTests : BaseIntegrationTest
{
	public CtvRegistrationsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task Post_CtvRegistrations_WithValidData_ReturnsSuccess()
	{
		// Arrange
		var request = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",
			Phone = "0933123456",
			Zalo = "nguyenvana",
			Email = "a@gmail.com"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/ctv-registrations", request);
		var result = await response.Content.ReadFromJsonAsync<ApiResponse<CtvRegistrationResponseDto>>();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.Data.Should().NotBeNull();
		result.Data!.FullName.Should().Be(request.FullName);
		result.Data.Phone.Should().Be(request.Phone);
		result.Data.IsApproved.Should().BeFalse();
	}

	[Fact]
	public async Task Post_CtvRegistrations_WithMissingFullName_ReturnsBadRequest()
	{
		// Arrange
		var request = new CreateCtvRegistrationDto
		{
			FullName = "",
			Phone = "0933123456"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/leads/ctv-registrations", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Get_CtvRegistrations_WithDefaultPaging_ReturnsPagedList()
	{
		// Arrange
		await SetAdminAuthorization();

		var registrations = new[]
		{
			new CtvRegistration { FullName = "A", Phone = "0912345678", IsApproved = false, CreatedAt = DateTime.UtcNow.AddHours(7) },
			new CtvRegistration { FullName = "B", Phone = "0912345679", IsApproved = true, CreatedAt = DateTime.UtcNow.AddHours(7).AddMinutes(-1) },
			new CtvRegistration { FullName = "C", Phone = "0912345680", IsApproved = false, CreatedAt = DateTime.UtcNow.AddHours(7).AddMinutes(-2) }
		};
		await DbContext.CtvRegistrations.AddRangeAsync(registrations);
		await DbContext.SaveChangesAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/ctv-registrations");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<CtvRegistrationResponseDto>>();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.Data.Should().HaveCount(3);
		result.TotalCount.Should().Be(3);
		result.PageNumber.Should().Be(1);
		result.HasNextPage.Should().BeFalse();
	}

	[Fact]
	public async Task Get_CtvRegistrations_FilterByIsApproved_ReturnsFiltered()
	{
		// Arrange
		await SetAdminAuthorization();

		await DbContext.CtvRegistrations.AddRangeAsync(
			new CtvRegistration { FullName = "A", Phone = "0912345678", IsApproved = true, CreatedAt = DateTime.UtcNow.AddHours(7) },
			new CtvRegistration { FullName = "B", Phone = "0912345679", IsApproved = false, CreatedAt = DateTime.UtcNow.AddHours(7) }
		);
		await DbContext.SaveChangesAsync();

		// Act
		var response = await Client.GetAsync("/api/leads/ctv-registrations?isApproved=true");
		var result = await response.Content.ReadFromJsonAsync<PagedResponse<CtvRegistrationResponseDto>>();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Data.Should().AllSatisfy(x => x.IsApproved.Should().BeTrue());
		result.TotalCount.Should().Be(1);
	}

	[Fact]
	public async Task Get_CtvRegistrations_WithInvalidPage_ReturnsBadRequest()
	{
		// Arrange
		await SetAdminAuthorization();

		// Act
		var response = await Client.GetAsync("/api/leads/ctv-registrations?page=0");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}