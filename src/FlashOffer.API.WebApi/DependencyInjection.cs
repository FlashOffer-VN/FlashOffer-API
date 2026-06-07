using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlashOffer.API.Application;
using FlashOffer.API.Infrastructure;
using FlashOffer.API.Infrastructure.Configurations;
using FlashOffer.API.WebApi.Filters;
using FlashOffer.API.WebApi.Configurations;
using FluentValidation.AspNetCore;
using FlashOffer.API.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.WebApi;

public static class DependencyInjection
{
	public static IServiceCollection AddWebApiServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Add JWT Settings
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

		var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
		if (jwtSettings == null
			|| string.IsNullOrWhiteSpace(jwtSettings.Secret)
			|| jwtSettings.Secret.Length < 32
			|| string.IsNullOrWhiteSpace(jwtSettings.Issuer)
			|| string.IsNullOrWhiteSpace(jwtSettings.Audience)
			|| jwtSettings.ExpiryMinutes <= 0)
		{
			throw new InvalidOperationException("JWT configuration is invalid. Please configure JwtSettings in appsettings or environment variables.");
		}

		var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = true;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidIssuer = jwtSettings.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtSettings.Audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
		});

		services.AddAuthorization();

		// Add API Versioning
		services.AddApiVersioningConfig();

		// Add Application layer services
		services.AddApplicationServices();

		// Add Infrastructure layer services
		services.AddInfrastructureServices(configuration);

		// Add API specific services with FluentValidation
		services.AddControllers(options =>
		{
			options.Filters.Add<ValidationFilter>();
		})
		.AddFluentValidation(fv =>
		{
			fv.RegisterValidatorsFromAssemblyContaining<CreatePurchaseRequestValidator>();
			fv.AutomaticValidationEnabled = true;
		});

		// QUAN TRỌNG: Tắt filter mặc định của .NET
		services.Configure<ApiBehaviorOptions>(options =>
		{
			options.SuppressModelStateInvalidFilter = true;
		});

		services.AddLocalization();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}
}