using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFieldsToOfferRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfferRequests_Phone",
                table: "OfferRequests");

            migrationBuilder.DropIndex(
                name: "IX_OfferRequests_Status",
                table: "OfferRequests");

            migrationBuilder.DropIndex(
                name: "IX_OfferRequests_Zalo",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "SelectedOffer",
                table: "OfferRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Zalo",
                table: "OfferRequests",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OfferRequests",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "OfferRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "OfferRequests",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedPrice",
                table: "OfferRequests",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OfferRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductLink",
                table: "OfferRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OfferRequests",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OfferRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "OfferRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "ExpectedPrice",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "ProductLink",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "OfferRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Zalo",
                table: "OfferRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OfferRequests",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "OfferRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "SelectedOffer",
                table: "OfferRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRequests_Phone",
                table: "OfferRequests",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRequests_Status",
                table: "OfferRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRequests_Zalo",
                table: "OfferRequests",
                column: "Zalo");
        }
    }
}
