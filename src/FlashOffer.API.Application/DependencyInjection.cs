using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Shared.Common.Interfaces;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlashOffer.API.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(
		this IServiceCollection services)
	{
		// Add AutoMapper
		services.AddAutoMapper(typeof(DependencyInjection).Assembly);

		// Add FluentValidation
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

		// JWT Service
		services.AddScoped<IJwtService, JwtService>();

		// Register services
		services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
		services.AddScoped<IGroupBuyingRequestService, GroupBuyingRequestService>();
		services.AddScoped<ICtvRegistrationService, CtvRegistrationService>();
		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}
