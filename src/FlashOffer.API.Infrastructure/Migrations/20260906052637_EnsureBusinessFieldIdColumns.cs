using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnsureBusinessFieldIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Đồng bộ schema: model khai báo BusinessFieldId trên các bảng request/collaborator
            // nhưng một số DB (tạo từ trước khi migration này ra đời) chưa có cột.
            // Bổ sung idempotent — chỉ thêm khi cột/index chưa tồn tại.
            foreach (var table in new[] { "PurchaseRequests", "GroupBuyingRequests", "OfferRequests", "Collaborators" })
            {
                migrationBuilder.Sql($@"
                    DO $$
                    BEGIN
                        IF NOT EXISTS (
                            SELECT 1 FROM information_schema.columns
                            WHERE table_name = '{table}' AND column_name = 'BusinessFieldId'
                        ) THEN
                            ALTER TABLE ""{table}"" ADD COLUMN ""BusinessFieldId"" uuid NULL;
                        END IF;
                    END $$;");

                migrationBuilder.Sql($@"
                    DO $$
                    BEGIN
                        IF NOT EXISTS (
                            SELECT 1 FROM pg_indexes
                            WHERE tablename = '{table}' AND indexname = 'IX_{table}_BusinessFieldId'
                        ) THEN
                            CREATE INDEX ""IX_{table}_BusinessFieldId"" ON ""{table}"" (""BusinessFieldId"");
                        END IF;
                    END $$;");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var table in new[] { "PurchaseRequests", "GroupBuyingRequests", "OfferRequests", "Collaborators" })
            {
                migrationBuilder.Sql($@"
                    DROP INDEX IF EXISTS ""IX_{table}_BusinessFieldId"";
                    ALTER TABLE ""{table}"" DROP COLUMN IF EXISTS ""BusinessFieldId"";
                ");
            }
        }
    }
}