using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlashOffer.API.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var connectionString = ResolveConnectionString()
			?? throw new InvalidOperationException(
				"Set DB_CONNECTION_STRING in .env (repo root) or as an environment variable before running EF tools.");

		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseNpgsql(connectionString);

		return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeCurrentUserService());
	}

	private static string? ResolveConnectionString()
	{
		var fromEnvironment = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
			?? Environment.GetEnvironmentVariable("DATABASE_URL");
		if (!string.IsNullOrWhiteSpace(fromEnvironment))
			return fromEnvironment;

		var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
		while (directory != null)
		{
			var envFile = Path.Combine(directory.FullName, ".env");
			if (File.Exists(envFile))
			{
				foreach (var line in File.ReadAllLines(envFile))
				{
					if (line.StartsWith("DB_CONNECTION_STRING=", StringComparison.Ordinal))
						return line["DB_CONNECTION_STRING=".Length..].Trim();
				}

				break;
			}

			directory = directory.Parent;
		}

		return null;
	}
}

public class DesignTimeCurrentUserService : ICurrentUserService
{
	public string? UserId => "DevLocal_Migration";
	public string? UserName => "DevLocal_Migration";
	public bool IsAuthenticated => false;
}
