using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncCollaboratorBusinessFieldColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Đồng bộ schema: model kỳ vọng cột "BusinessFieldName" nhưng một số DB
            // (đã migrate trước đây) đang có cột "BusinessField". Chỉ rename khi
            // cột cũ tồn tại VÀ cột mới chưa tồn tại — an toàn với live DB.
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'BusinessField'
                    ) AND NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'BusinessFieldName'
                    ) THEN
                        ALTER TABLE ""Collaborators"" RENAME COLUMN ""BusinessField"" TO ""BusinessFieldName"";
                    END IF;
                END $$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'BusinessField'
                    ) AND EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'BusinessFieldName'
                    ) THEN
                        ALTER TABLE ""Collaborators"" RENAME COLUMN ""BusinessFieldName"" TO ""BusinessField"";
                    END IF;
                END $$;");
        }
    }
}
