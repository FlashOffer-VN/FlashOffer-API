using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;
using System.Diagnostics;

namespace FlashOffer.API.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
	protected readonly HttpClient Client;

	protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
	{
		Console.WriteLine("=== BaseIntegrationTest constructor START ===");

		var currentDir = Directory.GetCurrentDirectory();
		Console.WriteLine($"Current directory: {currentDir}");

		var logDir = Path.Combine(currentDir, "logs");
		Console.WriteLine($"Log directory: {logDir}");

		if (!Directory.Exists(logDir))
		{
			Directory.CreateDirectory(logDir);
			Console.WriteLine($"Created directory: {logDir}");
		}

		var logPath = Path.Combine(logDir, $"integration-test-{DateTime.Now:yyyyMMdd-HHmmss}.log");
		Console.WriteLine($"Log path: {logPath}");

		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File(logPath)
			.CreateLogger();

		Log.Information("BaseIntegrationTest initialized");
		Log.Information($"Log file: {logPath}");

		// Ép buộc ghi log ngay
		Log.CloseAndFlush();

		Console.WriteLine("Log should be written to: " + logPath);


		var application = factory.WithWebHostBuilder(builder =>
		{
			// Thêm Serilog vào logging pipeline
			builder.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddSerilog(Log.Logger);
			});

			builder.ConfigureServices(services =>
			{
				// Xóa TẤT CẢ DbContext và options
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
					options.UseInMemoryDatabase("FlashOfferTestDb");
					options.EnableSensitiveDataLogging();
				});

				// Đăng ký lại IApplicationDbContext
				services.AddScoped<IApplicationDbContext>(provider =>
					provider.GetRequiredService<ApplicationDbContext>());
			});
		});

		Client = application.CreateClient();
	}
}