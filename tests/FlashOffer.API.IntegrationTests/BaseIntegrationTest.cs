using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.WebApi.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace FlashOffer.API.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
	protected readonly HttpClient Client;
	protected readonly WebApplicationFactory<Program> Factory;
	protected readonly ApplicationDbContext DbContext;
	private readonly IServiceScope _scope;
	private bool _disposed;
	private string? _adminToken;

	protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
	{
		Console.WriteLine("🧪 Initializing test fixture...");

		var dbName = $"FlashOfferTestDb_{Guid.NewGuid()}";

		var application = factory.WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				var descriptorsToRemove = services.Where(d =>
					d.ServiceType == typeof(ApplicationDbContext) ||
					d.ServiceType == typeof(IApplicationDbContext) ||
					d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
					d.ImplementationType == typeof(ApplicationDbContext) ||
					d.ServiceType.Name.Contains("DbContext")).ToList();

				foreach (var descriptor in descriptorsToRemove)
				{
					services.Remove(descriptor);
				}

				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseInMemoryDatabase(dbName));
				services.AddScoped<IApplicationDbContext>(provider =>
					provider.GetRequiredService<ApplicationDbContext>());
			});
		});

		Client = application.CreateClient();
		Factory = application;

		_scope = application.Services.CreateScope();
		DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		// Seed admin user
		SeedDatabaseAsync().GetAwaiter().GetResult();

		Console.WriteLine("✅ Test fixture ready");
	}

	private async Task SeedDatabaseAsync()
	{
		if (await DbContext.Users.AnyAsync(u => u.Username == "admin"))
			return;

		var adminUser = new User
		{
			Id = Guid.NewGuid(),
			Username = "admin",
			Email = "admin@flashoffer.com",
			FullName = "Admin User",
			PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
			Role = UserRole.Admin,
			CreatedAt = DateTime.UtcNow
		};

		DbContext.Users.Add(adminUser);
		await DbContext.SaveChangesAsync();
	}

	protected async Task<string> GetAdminTokenAsync()
	{
		if (!string.IsNullOrEmpty(_adminToken))
			return _adminToken;

		var loginRequest = new LoginRequest
		{
			Username = "admin",
			Password = "password123"
		};

		var response = await Client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
		_adminToken = result!.Data!.Token;
		return _adminToken;
	}

	protected async Task SetAdminAuthorization()
	{
		var token = await GetAdminTokenAsync();
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed) return;

		if (disposing)
		{
			_scope?.Dispose();
			Client?.Dispose();
			Factory?.Dispose();
		}

		_disposed = true;
	}
}