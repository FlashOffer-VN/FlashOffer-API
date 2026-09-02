using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessFieldIdToMissingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Chỉ thêm BusinessFieldId cho Partner (nếu chưa có)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                   WHERE table_name='Partners' AND column_name='BusinessFieldId') 
                    THEN
                        ALTER TABLE ""Partners"" ADD COLUMN ""BusinessFieldId"" uuid NULL;
                    END IF;
                END $$;
            ");

            // Thêm cho các bảng khác nếu chưa có
            // PartnerProduct
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                   WHERE table_name='PartnerProducts' AND column_name='BusinessFieldId') 
                    THEN
                        ALTER TABLE ""PartnerProducts"" ADD COLUMN ""BusinessFieldId"" uuid NULL;
                    END IF;
                END $$;
            ");

            // Tạo index cho các bảng
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM pg_indexes 
                                   WHERE tablename='Partners' AND indexname='IX_Partners_BusinessFieldId') 
                    THEN
                        CREATE INDEX ""IX_Partners_BusinessFieldId"" ON ""Partners"" (""BusinessFieldId"");
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback nếu cần
            migrationBuilder.Sql(@"
                ALTER TABLE ""Partners"" DROP COLUMN IF EXISTS ""BusinessFieldId"";
                ALTER TABLE ""PartnerProducts"" DROP COLUMN IF EXISTS ""BusinessFieldId"";
            ");
        }
    }
}
