using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessFieldRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ Thêm cột BusinessFieldId cho Collaborators (THÊM DÒNG NÀY)
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "Collaborators",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "PurchaseRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "Partners",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "PartnerProducts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "OfferRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "GroupBuyingRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_BusinessFieldId",
                table: "Collaborators",
                column: "BusinessFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_BusinessFieldId",
                table: "PurchaseRequests",
                column: "BusinessFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_BusinessFieldId",
                table: "Partners",
                column: "BusinessFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProducts_BusinessFieldId",
                table: "PartnerProducts",
                column: "BusinessFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRequests_BusinessFieldId",
                table: "OfferRequests",
                column: "BusinessFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBuyingRequests_BusinessFieldId",
                table: "GroupBuyingRequests",
                column: "BusinessFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_BusinessFields_BusinessFieldId",
                table: "Collaborators",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupBuyingRequests_BusinessFields_BusinessFieldId",
                table: "GroupBuyingRequests",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferRequests_BusinessFields_BusinessFieldId",
                table: "OfferRequests",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerProducts_BusinessFields_BusinessFieldId",
                table: "PartnerProducts",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_BusinessFields_BusinessFieldId",
                table: "Partners",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_BusinessFields_BusinessFieldId",
                table: "PurchaseRequests",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_BusinessFields_BusinessFieldId",
                table: "Collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupBuyingRequests_BusinessFields_BusinessFieldId",
                table: "GroupBuyingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferRequests_BusinessFields_BusinessFieldId",
                table: "OfferRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerProducts_BusinessFields_BusinessFieldId",
                table: "PartnerProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_BusinessFields_BusinessFieldId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_BusinessFields_BusinessFieldId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_BusinessFieldId",
                table: "Collaborators");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_BusinessFieldId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_Partners_BusinessFieldId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_PartnerProducts_BusinessFieldId",
                table: "PartnerProducts");

            migrationBuilder.DropIndex(
                name: "IX_OfferRequests_BusinessFieldId",
                table: "OfferRequests");

            migrationBuilder.DropIndex(
                name: "IX_GroupBuyingRequests_BusinessFieldId",
                table: "GroupBuyingRequests");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "Collaborators");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "PartnerProducts");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "GroupBuyingRequests");
        }
    }
}