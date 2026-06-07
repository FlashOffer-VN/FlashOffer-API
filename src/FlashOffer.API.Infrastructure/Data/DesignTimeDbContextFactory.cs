// src/FlashOffer.API.Infrastructure/Data/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FlashOffer.API.Application.Common.Interfaces;

namespace FlashOffer.API.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FlashOfferDb;Trusted_Connection=True;");

		return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeCurrentUserService());
	}
}

public class DesignTimeCurrentUserService : ICurrentUserService
{
	public string? UserId => "DevLocal_Migration";
	public string? UserName => "DevLocal_Migration";
	public bool IsAuthenticated => false;
}