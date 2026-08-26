using AutoMapper;
using FlashOffer.API.Application.Common.Behaviors;
using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Infrastructure.Services;
using FlashOffer.API.Shared.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace FlashOffer.API.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(
		this IServiceCollection services)
	{
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.AssignableTo<IBusinessFieldService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Add AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

		// Add FluentValidation
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

		// JWT Service
		services.AddScoped<IJwtService, JwtService>();

		// Register services
		services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
		services.AddScoped<IGroupBuyingRequestService, GroupBuyingRequestService>();
		services.AddScoped<ICtvRegistrationService, CtvRegistrationService>();
		services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<ICtvService, CtvService>();
        services.AddScoped<ISocialService, SocialService>();
        services.AddScoped<ICollaboratorService, CollaboratorService>();
        services.AddScoped<ISocialInteractionService, SocialInteractionService>();
        services.AddScoped<IBusinessFieldService, BusinessFieldService>();

        return services;
	}
}
