// src/FlashOffer.API.Infrastructure/Data/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;

namespace FlashOffer.API.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FlashOfferDb;Username=postgres;Password=postgres");

		return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeCurrentUserService());
	}
}

public class DesignTimeCurrentUserService : ICurrentUserService
{
	public string? UserId => "DevLocal_Migration";
	public string? UserName => "DevLocal_Migration";
	public bool IsAuthenticated => false;
}