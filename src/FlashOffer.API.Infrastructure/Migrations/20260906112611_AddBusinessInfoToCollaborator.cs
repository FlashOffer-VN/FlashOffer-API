using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessInfoToCollaborator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Collaborators",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessSize",
                table: "Collaborators",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Collaborators",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "BusinessSize",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Collaborators");
        }
    }
}
