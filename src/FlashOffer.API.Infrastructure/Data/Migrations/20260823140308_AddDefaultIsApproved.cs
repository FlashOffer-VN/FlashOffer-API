using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultIsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Collaborators",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback: xóa default value
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Collaborators",
                nullable: false,
                defaultValue: null);
        }
    }
}