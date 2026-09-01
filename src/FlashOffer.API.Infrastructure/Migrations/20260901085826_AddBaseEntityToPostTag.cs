using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityToPostTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
        name: "CreatedAt",
        table: "PostTags",
        type: "timestamp with time zone",
        nullable: false,
        defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PostTags",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PostTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostTags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PostTags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PostTags",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
        name: "CreatedAt",
        table: "PostTags");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PostTags");
        }
    }
}
