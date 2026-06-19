using System.Net;
using System.Net.Http.Json;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.Shared.Common.Helpers;
using FlashOffer.API.WebApi.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FlashOffer.API.IntegrationTests.Controllers;

public class AuthControllerTests : BaseIntegrationTest
{
	public AuthControllerTests(WebApplicationFactory<Program> factory) : base(factory)
	{
		// Seed an admin user into the in-memory database used by the test factory
		using var scope = Factory.Server.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		var admin = new Admin
		{
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("password123"),
			IsActive = true
		};
		db.Admins.Add(admin);
		db.SaveChanges();

		// Seed verification done during test runs; no console output here
	}

	[Fact]
	public async Task Login_ValidCredentials_ReturnsOk()
	{
		// Arrange
		var request = new LoginRequest
		{
			Username = "admin",
			Password = "password123"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/auth/login", request);
		var content = await response.Content.ReadAsStringAsync();
		ApiResponse<LoginResponse>? result = null;
		try
		{
			result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}
		catch { /* ignore parse errors for debugging */ }

		// (No debug output)

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.Data.Should().NotBeNull();
		result.Data!.Token.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Login_InvalidCredentials_ReturnsUnauthorized()
	{
		// Arrange
		var request = new LoginRequest
		{
			Username = "admin",
			Password = "wrongpassword"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/auth/login", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task Login_MissingUsername_ReturnsBadRequest()
	{
		// Arrange
		var request = new LoginRequest
		{
			Username = "",
			Password = "pwd"
		};

		// Act
		var response = await Client.PostAsJsonAsync("/api/v1/auth/login", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
