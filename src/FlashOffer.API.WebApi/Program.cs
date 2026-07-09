using DotNetEnv;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

// Load optional .env file for local development
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
envPath = Path.GetFullPath(envPath);

if (File.Exists(envPath))
{
	Env.Load(envPath);
	Console.WriteLine("Environment file loaded");
}
else
{
	Console.WriteLine("Environment file not found; using OS environment variables");
}

static string? GetEnvironmentValue(string key)
{
	var value = Environment.GetEnvironmentVariable(key);
	if (!string.IsNullOrWhiteSpace(value))
	{
		return value;
	}

	try
	{
		return Env.GetString(key);
	}
	catch
	{
		return null;
	}
}

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddLogging();

// Configuration: OS environment variables take precedence over .env file
var envConfig = new Dictionary<string, string?>
{
	["ConnectionStrings:DefaultConnection"] = GetEnvironmentValue("DB_CONNECTION_STRING") ?? GetEnvironmentValue("DATABASE_URL"),
	["JwtSettings:Secret"] = GetEnvironmentValue("JWT_SECRET"),
	["JwtSettings:Issuer"] = GetEnvironmentValue("JWT_ISSUER"),
	["JwtSettings:Audience"] = GetEnvironmentValue("JWT_AUDIENCE"),
	["JwtSettings:ExpiryMinutes"] = GetEnvironmentValue("JWT_EXPIRY_MINUTES"),
	["Logging:LogLevel:Default"] = GetEnvironmentValue("LOG_LEVEL"),
	["CorsSettings:Policy"] = GetEnvironmentValue("CORS_POLICY"),
	["CorsSettings:AllowedOrigins"] = GetEnvironmentValue("ALLOWED_ORIGINS")
};

builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
	.AddInMemoryCollection(envConfig.Where(x => x.Value != null)
		.ToDictionary(x => x.Key, x => x.Value))
	.AddEnvironmentVariables();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddHealthChecks();

var allowedOriginsRaw = GetEnvironmentValue("ALLOWED_ORIGINS");
var useAllowAllOrigins = string.IsNullOrWhiteSpace(allowedOriginsRaw)
	|| allowedOriginsRaw.Trim() == "*";

// Add CORS
builder.Services.AddCors(options =>
{
	if (useAllowAllOrigins)
	{
		options.AddPolicy("AllowAllOrigins", policy =>
		{
			policy.AllowAnyOrigin()
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
	}
	else
	{
		var allowedOrigins = allowedOriginsRaw!
			.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		options.AddPolicy("AllowSpecific", policy =>
		{
			policy.WithOrigins(allowedOrigins)
				  .AllowAnyMethod()
				  .AllowAnyHeader()
				  .AllowCredentials();
		});
	}

	options.AddPolicy("SwaggerPolicy", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

// Add Swagger configuration
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	var migrationLogger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");
	await DatabaseMigrationRunner.ApplyMigrationsAsync(db, migrationLogger);
	await DatabaseSeeder.SeedAsync(db);
}

// Configure localization
var supportedCultures = new[] { "en", "vi" };
var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture("vi")
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

var enableSwagger = string.Equals(
	GetEnvironmentValue("ENABLE_SWAGGER"),
	"true",
	StringComparison.OrdinalIgnoreCase);

// Configure pipeline
if (app.Environment.IsDevelopment() || enableSwagger)
{
	var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		foreach (var description in provider.ApiVersionDescriptions)
		{
			options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
				description.GroupName.ToUpperInvariant());
		}
	});
}

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
	app.UseCors("SwaggerPolicy");
}
else
{
	var corsPolicy = useAllowAllOrigins
		? "AllowAllOrigins"
		: GetEnvironmentValue("CORS_POLICY") ?? "AllowSpecific";
	app.UseCors(corsPolicy);
}

app.UseMiddleware<FlashOffer.API.WebApi.Middlewares.GlobalExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}
