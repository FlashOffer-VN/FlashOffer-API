using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Infrastructure.Data;
using FlashOffer.API.Infrastructure.Repositories;
using FlashOffer.API.Infrastructure.Services;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlashOffer.API.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Database context
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

		services.AddScoped<IApplicationDbContext>(provider =>
			provider.GetRequiredService<ApplicationDbContext>());

		// Generic repository
		services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

		// Current User Service
		services.AddScoped<ICurrentUserService, CurrentUserService>();

		// HttpContextAccessor
		services.AddHttpContextAccessor();

		return services;
	}
}
