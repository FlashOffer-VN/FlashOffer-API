using System.Net;
using System.Net.Http.Json;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

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
}