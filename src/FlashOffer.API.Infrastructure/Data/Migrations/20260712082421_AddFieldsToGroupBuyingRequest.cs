using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToGroupBuyingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "GroupBuyingRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductLink",
                table: "GroupBuyingRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zalo",
                table: "GroupBuyingRequests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "GroupBuyingRequests");

            migrationBuilder.DropColumn(
                name: "ProductLink",
                table: "GroupBuyingRequests");

            migrationBuilder.DropColumn(
                name: "Zalo",
                table: "GroupBuyingRequests");
        }
    }
}
