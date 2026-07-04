using Microsoft.EntityFrameworkCore.Migrations;
using FlashOffer.API.Shared.Common.Helpers;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
	public partial class SeedAdminUser : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Hash password bằng PasswordHasher
			var hashedPassword = PasswordHasher.Hash("Admin@123");

			migrationBuilder.Sql($@"
                IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Username = 'admin')
                BEGIN
                    INSERT INTO [Users] (
                        [Id],
                        [Username],
                        [PasswordHash],
                        [FullName],
                        [Email],
                        [Phone],
                        [IsActive],
                        [Role],
                        [CreatedAt],
                        [IsDeleted]
                    )
                    VALUES (
                        NEWID(),
                        'admin',
                        '{hashedPassword}',
                        'Administrator',
                        'admin@flashoffer.com',
                        '0987654321',
                        1,
                        3,
                        GETUTCDATE(),
                        0
                    )
                END
            ");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DELETE FROM [Users] WHERE Username = 'admin'");
		}
	}
}