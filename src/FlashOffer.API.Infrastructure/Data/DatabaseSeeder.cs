using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Shared.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FlashOffer.API.Infrastructure.Data;

public static class DatabaseSeeder
{
	public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
	{
		if (await context.Users.AnyAsync(u => u.Username == "admin", cancellationToken))
		{
			return;
		}

		var admin = new User
		{
			UserCode = CodeGenerator.Generate("USR"),
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("Admin@123"),
			FullName = "Administrator",
			Email = "admin@flashoffer.com",
			Phone = "0987654321",
			IsActive = true,
			Role = UserRole.Admin
		};

		context.Users.Add(admin);
		await context.SaveChangesAsync(cancellationToken);
	}
}
