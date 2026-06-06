using Serilog;
using FlashOffer-API.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using FlashOffer-API.WebApi.Configurations;
using DotNetEnv;

// Load .env file (ưu tiên cao nhất)
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Nominals config load order: appsettings, environment, then .env overrides
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["ConnectionStrings:DefaultConnection"] = Env.GetString("DB_CONNECTION_STRING"),
        ["JwtSettings:Secret"] = Env.GetString("JWT_SECRET"),
        ["JwtSettings:Issuer"] = Env.GetString("JWT_ISSUER"),
        ["JwtSettings:Audience"] = Env.GetString("JWT_AUDIENCE"),
        ["JwtSettings:ExpiryMinutes"] = Env.GetString("JWT_EXPIRY_MINUTES"),
        ["Logging:LogLevel:Default"] = Env.GetString("LOG_LEVEL")
    });

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

// Add Swagger configuration
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

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
app.UseMiddleware<FlashOffer-API.WebApi.Middlewares.GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
