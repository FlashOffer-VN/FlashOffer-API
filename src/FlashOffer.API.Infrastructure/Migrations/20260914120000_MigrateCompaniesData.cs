using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    public partial class MigrateCompaniesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Insert companies from Partners if not exists (by TaxCode or Name)
            migrationBuilder.Sql(@"
                INSERT INTO ""Companies"" (""Id"", ""Name"", ""TaxCode"", ""Address"", ""Website"", ""BusinessType"", ""CompanySize"", ""BusinessFieldId"", ""CreatedAt"", ""IsDeleted"", ""CreatedBy"")
                SELECT gen_random_uuid(), p.""CompanyName"", p.""CompanyTax"", p.""CompanyAddress"", p.""CompanyWebsite"", p.""BusinessType"", p.""CompanySize"", p.""BusinessFieldId"", now(), FALSE, 'MigrateFromPartners'
                FROM ""Partners"" p
                WHERE p.""CompanyName"" IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM ""Companies"" c
                      WHERE (c.""TaxCode"" IS NOT NULL AND c.""TaxCode"" = p.""CompanyTax"") OR (c.""Name"" = p.""CompanyName"")
                  );
            ");

            // 2) Insert companies from Collaborators if not exists (by Name)
            migrationBuilder.Sql(@"
                INSERT INTO ""Companies"" (""Id"", ""Name"", ""TaxCode"", ""Address"", ""Website"", ""BusinessFieldId"", ""CreatedAt"", ""IsDeleted"", ""CreatedBy"")
                SELECT gen_random_uuid(), c.""BusinessName"", NULL, c.""Address"", c.""Website"", c.""BusinessFieldId"", now(), FALSE, 'MigrateFromCollaborators'
                FROM ""Collaborators"" c
                WHERE c.""BusinessName"" IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1 FROM ""Companies"" co
                      WHERE co.""Name"" = c.""BusinessName""
                  );
            ");

            // 3) Link Partners -> Companies when matching by TaxCode or Name
            migrationBuilder.Sql(@"
                UPDATE ""Partners"" p
                SET ""CompanyId"" = co.""Id""
                FROM ""Companies"" co
                WHERE p.""CompanyId"" IS NULL
                  AND (
                        (p.""CompanyTax"" IS NOT NULL AND co.""TaxCode"" = p.""CompanyTax"")
                     OR (p.""CompanyName"" IS NOT NULL AND co.""Name"" = p.""CompanyName"")
                  );
            ");

            // 4) Link Collaborators -> Companies when matching by BusinessName
            migrationBuilder.Sql(@"
                UPDATE ""Collaborators"" c
                SET ""CompanyId"" = co.""Id""
                FROM ""Companies"" co
                WHERE c.""CompanyId"" IS NULL
                  AND c.""BusinessName"" IS NOT NULL
                  AND co.""Name"" = c.""BusinessName"";
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Unlink partners/collaborators created by this migration and remove companies created by migration
            migrationBuilder.Sql(@"
                UPDATE ""Partners"" SET ""CompanyId"" = NULL WHERE ""CompanyId"" IN (
                    SELECT ""Id"" FROM ""Companies"" WHERE ""CreatedBy"" = 'MigrateFromPartners' OR ""CreatedBy"" = 'MigrateFromCollaborators'
                );
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Collaborators"" SET ""CompanyId"" = NULL WHERE ""CompanyId"" IN (
                    SELECT ""Id"" FROM ""Companies"" WHERE ""CreatedBy"" = 'MigrateFromPartners' OR ""CreatedBy"" = 'MigrateFromCollaborators'
                );
            ");

            migrationBuilder.Sql(@"
                DELETE FROM ""Companies"" WHERE ""CreatedBy"" = 'MigrateFromPartners' OR ""CreatedBy"" = 'MigrateFromCollaborators';
            ");
        }
    }
}
