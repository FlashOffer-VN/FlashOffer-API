using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesChannelToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bỏ qua nếu đã tồn tại
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.columns 
                               WHERE table_name='CtvRegistrations' AND column_name='SalesChannel') THEN
                        ALTER TABLE ""CtvRegistrations"" 
                        ALTER COLUMN ""SalesChannel"" TYPE integer 
                        USING (CASE 
                            WHEN ""SalesChannel"" = 'retail' THEN 1
                            WHEN ""SalesChannel"" = 'wholesale' THEN 2
                            WHEN ""SalesChannel"" = 'online' THEN 3
                            WHEN ""SalesChannel"" = 'offline' THEN 4
                            ELSE 5
                        END);
                
                        ALTER TABLE ""CtvRegistrations"" 
                        ALTER COLUMN ""SalesChannel"" SET DEFAULT 5;
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SalesChannel",
                table: "CtvRegistrations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValue: 5);
        }
    }
}
