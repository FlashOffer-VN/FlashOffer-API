using Serilog;
using FlashOffer.API.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using FlashOffer.API.WebApi.Configurations;
using DotNetEnv;
using Microsoft.AspNetCore.Localization;

// Load .env file
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
envPath = Path.GetFullPath(envPath);

if (File.Exists(envPath))
{
	Env.Load(envPath);
	Console.WriteLine("✓ .env loaded");
}
else
{
	Console.WriteLine("⚠ .env not found");
}

var builder = WebApplication.CreateBuilder(args);

// Configuration - Env ưu tiên cao nhất
var envConfig = new Dictionary<string, string?>
{
	["ConnectionStrings:DefaultConnection"] = Env.GetString("DB_CONNECTION_STRING"),
	["JwtSettings:Secret"] = Env.GetString("JWT_SECRET"),
	["JwtSettings:Issuer"] = Env.GetString("JWT_ISSUER"),
	["JwtSettings:Audience"] = Env.GetString("JWT_AUDIENCE"),
	["JwtSettings:ExpiryMinutes"] = Env.GetString("JWT_EXPIRY_MINUTES"),
	["Logging:LogLevel:Default"] = Env.GetString("LOG_LEVEL"),
	["CorsSettings:Policy"] = Env.GetString("CORS_POLICY"),
	["CorsSettings:AllowedOrigins"] = Env.GetString("ALLOWED_ORIGINS")
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

// Add CORS
builder.Services.AddCors(options =>
{
	var allowedOrigins = Env.GetString("ALLOWED_ORIGINS")?.Split(',') ?? new[] { "http://localhost:4200" };
	options.AddPolicy("AllowSpecific", policy =>
	{
		policy.WithOrigins(allowedOrigins)
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});

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

// Configure localization
var supportedCultures = new[] { "en", "vi" };
var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture("vi")
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure pipeline
if (app.Environment.IsDevelopment())
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
app.UseMiddleware<FlashOffer.API.WebApi.Middlewares.GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseCors("SwaggerPolicy");
}
else
{
	var corsPolicy = Env.GetString("CORS_POLICY") ?? "AllowSpecific";
	app.UseCors(corsPolicy);
}

if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }