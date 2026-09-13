using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyEntityAndCompanyFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Partners",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Collaborators",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TaxCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    BusinessType = table.Column<int>(type: "integer", nullable: true),
                    CompanySize = table.Column<int>(type: "integer", nullable: true),
                    BusinessFieldId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_BusinessFields_BusinessFieldId",
                        column: x => x.BusinessFieldId,
                        principalTable: "BusinessFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CompanyId",
                table: "Partners",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_CompanyId",
                table: "Collaborators",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BusinessFieldId",
                table: "Companies",
                column: "BusinessFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_Companies_CompanyId",
                table: "Collaborators",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Companies_CompanyId",
                table: "Partners",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_Companies_CompanyId",
                table: "Collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Companies_CompanyId",
                table: "Partners");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CompanyId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_CompanyId",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Collaborators");
        }
    }
}
