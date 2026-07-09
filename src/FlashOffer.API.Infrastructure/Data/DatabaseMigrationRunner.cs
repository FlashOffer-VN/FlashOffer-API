using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FlashOffer.API.Infrastructure.Data;

public static class DatabaseMigrationRunner
{
	private const string InitialCreateMigrationId = "20260704140637_InitialCreate";

	public static async Task ApplyMigrationsAsync(
		ApplicationDbContext db,
		ILogger logger,
		CancellationToken cancellationToken = default)
	{
		var pendingMigrations = (await db.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
		if (pendingMigrations.Count == 0)
		{
			return;
		}

		logger.LogInformation(
			"Applying {Count} pending migration(s): {Migrations}",
			pendingMigrations.Count,
			string.Join(", ", pendingMigrations));

		try
		{
			await db.Database.MigrateAsync(cancellationToken);
		}
		catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.DuplicateTable)
		{
			logger.LogWarning(
				ex,
				"Database tables already exist without migration history. Baselining {MigrationId}.",
				InitialCreateMigrationId);

			await BaselineMigrationAsync(db, InitialCreateMigrationId, cancellationToken);

			var remainingMigrations = (await db.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
			if (remainingMigrations.Count > 0)
			{
				logger.LogInformation(
					"Retrying {Count} remaining migration(s): {Migrations}",
					remainingMigrations.Count,
					string.Join(", ", remainingMigrations));

				await db.Database.MigrateAsync(cancellationToken);
			}
		}
	}

	private static async Task BaselineMigrationAsync(
		ApplicationDbContext db,
		string migrationId,
		CancellationToken cancellationToken)
	{
		const string productVersion = "10.0.9";

		await db.Database.ExecuteSqlInterpolatedAsync(
			$"""
			INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
			VALUES ({migrationId}, {productVersion})
			ON CONFLICT ("MigrationId") DO NOTHING
			""",
			cancellationToken);
	}
}
