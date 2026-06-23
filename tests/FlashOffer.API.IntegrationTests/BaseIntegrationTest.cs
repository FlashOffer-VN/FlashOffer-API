// tests/FlashOffer.API.IntegrationTests/BaseIntegrationTest.cs
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace FlashOffer.API.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
	protected readonly HttpClient Client;
	protected readonly WebApplicationFactory<Program> Factory;
	protected readonly ApplicationDbContext DbContext;
	private readonly IServiceScope _scope;
	private bool _disposed;

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

		Console.WriteLine("✅ Test fixture ready");
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