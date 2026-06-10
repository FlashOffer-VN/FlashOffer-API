using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace FlashOffer.API.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
	protected readonly HttpClient Client;

	protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
	{
		Console.WriteLine("🧪 Initializing test fixture...");

		var application = factory.WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				// Xóa tất cả DbContext registrations
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

				// Thêm InMemory database
				services.AddDbContext<ApplicationDbContext>(options =>
				{
					options.UseInMemoryDatabase($"FlashOfferTestDb_{Guid.NewGuid()}");
					options.EnableSensitiveDataLogging();
				});

				// Đăng ký lại IApplicationDbContext
				services.AddScoped<IApplicationDbContext>(provider =>
					provider.GetRequiredService<ApplicationDbContext>());
			});
		});

		Client = application.CreateClient();
		Console.WriteLine("✅ Test fixture ready");
	}
}