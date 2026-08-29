using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCtvRegistrationToCollaborator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CtvRegistrations");

            migrationBuilder.AddColumn<Guid>(
                name: "CollaboratorId",
                table: "Partners",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Zalo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Skills = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Interests = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Goals = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SalesChannel = table.Column<int>(type: "integer", nullable: true),
                    Experience = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AgreeTerms = table.Column<bool>(type: "boolean", nullable: false),
                    ParentCollaboratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    ReferralCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaborators_Collaborators_ParentCollaboratorId",
                        column: x => x.ParentCollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Collaborators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CollaboratorId",
                table: "Partners",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_Email",
                table: "Collaborators",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_ParentCollaboratorId",
                table: "Collaborators",
                column: "ParentCollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_Phone",
                table: "Collaborators",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_ReferralCode",
                table: "Collaborators",
                column: "ReferralCode",
                unique: true,
                filter: "\"ReferralCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_UserId",
                table: "Collaborators",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Collaborators_CollaboratorId",
                table: "Partners",
                column: "CollaboratorId",
                principalTable: "Collaborators",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Collaborators_CollaboratorId",
                table: "Partners");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CollaboratorId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "CollaboratorId",
                table: "Partners");

            migrationBuilder.CreateTable(
                name: "CtvRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Experience = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Phone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    SalesChannel = table.Column<int>(type: "integer", nullable: true, defaultValue: 5),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Zalo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CtvRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CtvRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CtvRegistrations_Email",
                table: "CtvRegistrations",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_CtvRegistrations_Phone",
                table: "CtvRegistrations",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_CtvRegistrations_Status",
                table: "CtvRegistrations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CtvRegistrations_UserId",
                table: "CtvRegistrations",
                column: "UserId");
        }
    }
}
