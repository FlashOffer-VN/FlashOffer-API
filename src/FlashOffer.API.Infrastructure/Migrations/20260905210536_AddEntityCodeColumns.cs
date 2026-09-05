using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityCodeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===== Collaborator: đổi tên thay vì drop/add (giữ dữ liệu cũ) =====
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Collaborators_ReferralCode\";");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'ReferralCode'
                    ) AND NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'CollaboratorCode'
                    ) THEN
                        ALTER TABLE ""Collaborators"" RENAME COLUMN ""ReferralCode"" TO ""CollaboratorCode"";
                        ALTER TABLE ""Collaborators"" ALTER COLUMN ""CollaboratorCode"" TYPE character varying(30);
                    END IF;
                END $$;");

            // ===== Thêm cột code mới =====
            migrationBuilder.AddColumn<string>(
                name: "UserCode",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagCode",
                table: "Tags",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialPostCode",
                table: "SocialPosts",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialCommentCode",
                table: "SocialComment",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseRequestCode",
                table: "PurchaseRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerProductCode",
                table: "PartnerProducts",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerCommissionCode",
                table: "PartnerCommissions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferRequestCode",
                table: "OfferRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupBuyingRequestCode",
                table: "GroupBuyingRequests",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessFieldCode",
                table: "BusinessFields",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            // ===== Unique index =====
            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCode",
                table: "Users",
                column: "UserCode",
                unique: true,
                filter: "\"UserCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagCode",
                table: "Tags",
                column: "TagCode",
                unique: true,
                filter: "\"TagCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialPosts_SocialPostCode",
                table: "SocialPosts",
                column: "SocialPostCode",
                unique: true,
                filter: "\"SocialPostCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialComment_SocialCommentCode",
                table: "SocialComment",
                column: "SocialCommentCode",
                unique: true,
                filter: "\"SocialCommentCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PurchaseRequestCode",
                table: "PurchaseRequests",
                column: "PurchaseRequestCode",
                unique: true,
                filter: "\"PurchaseRequestCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProducts_PartnerProductCode",
                table: "PartnerProducts",
                column: "PartnerProductCode",
                unique: true,
                filter: "\"PartnerProductCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerCommissions_PartnerCommissionCode",
                table: "PartnerCommissions",
                column: "PartnerCommissionCode",
                unique: true,
                filter: "\"PartnerCommissionCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRequests_OfferRequestCode",
                table: "OfferRequests",
                column: "OfferRequestCode",
                unique: true,
                filter: "\"OfferRequestCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBuyingRequests_GroupBuyingRequestCode",
                table: "GroupBuyingRequests",
                column: "GroupBuyingRequestCode",
                unique: true,
                filter: "\"GroupBuyingRequestCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_CollaboratorCode",
                table: "Collaborators",
                column: "CollaboratorCode",
                unique: true,
                filter: "\"CollaboratorCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessFields_BusinessFieldCode",
                table: "BusinessFields",
                column: "BusinessFieldCode",
                unique: true,
                filter: "\"BusinessFieldCode\" IS NOT NULL");

            // ===== Backfill dữ liệu cũ (mã duy nhất theo Id, an toàn với unique index) =====
            migrationBuilder.Sql("UPDATE \"Users\" SET \"UserCode\" = 'USR-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"UserCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"Tags\" SET \"TagCode\" = 'TAG-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"TagCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"SocialPosts\" SET \"SocialPostCode\" = 'SOC-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"SocialPostCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"SocialComment\" SET \"SocialCommentCode\" = 'SCM-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"SocialCommentCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"PurchaseRequests\" SET \"PurchaseRequestCode\" = 'PRQ-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"PurchaseRequestCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"PartnerProducts\" SET \"PartnerProductCode\" = 'PRDP-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"PartnerProductCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"PartnerCommissions\" SET \"PartnerCommissionCode\" = 'PCM-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"PartnerCommissionCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"OfferRequests\" SET \"OfferRequestCode\" = 'OFR-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"OfferRequestCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"GroupBuyingRequests\" SET \"GroupBuyingRequestCode\" = 'GBR-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"GroupBuyingRequestCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"Collaborators\" SET \"CollaboratorCode\" = 'CTV-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"CollaboratorCode\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"BusinessFields\" SET \"BusinessFieldCode\" = 'BSF-' || upper(substr(md5(\"Id\"::text), 1, 6)) WHERE \"BusinessFieldCode\" IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserCode",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TagCode",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_SocialPosts_SocialPostCode",
                table: "SocialPosts");

            migrationBuilder.DropIndex(
                name: "IX_SocialComment_SocialCommentCode",
                table: "SocialComment");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_PurchaseRequestCode",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PartnerProducts_PartnerProductCode",
                table: "PartnerProducts");

            migrationBuilder.DropIndex(
                name: "IX_PartnerCommissions_PartnerCommissionCode",
                table: "PartnerCommissions");

            migrationBuilder.DropIndex(
                name: "IX_OfferRequests_OfferRequestCode",
                table: "OfferRequests");

            migrationBuilder.DropIndex(
                name: "IX_GroupBuyingRequests_GroupBuyingRequestCode",
                table: "GroupBuyingRequests");

            migrationBuilder.DropIndex(
                name: "IX_Collaborators_CollaboratorCode",
                table: "Collaborators");

            migrationBuilder.DropIndex(
                name: "IX_BusinessFields_BusinessFieldCode",
                table: "BusinessFields");

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TagCode",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "SocialPostCode",
                table: "SocialPosts");

            migrationBuilder.DropColumn(
                name: "SocialCommentCode",
                table: "SocialComment");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestCode",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "PartnerProductCode",
                table: "PartnerProducts");

            migrationBuilder.DropColumn(
                name: "PartnerCommissionCode",
                table: "PartnerCommissions");

            migrationBuilder.DropColumn(
                name: "OfferRequestCode",
                table: "OfferRequests");

            migrationBuilder.DropColumn(
                name: "GroupBuyingRequestCode",
                table: "GroupBuyingRequests");

            migrationBuilder.DropColumn(
                name: "BusinessFieldCode",
                table: "BusinessFields");

            // ===== Khôi phục Collaborator: rename ngược + tạo lại index cũ =====
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'ReferralCode'
                    ) AND EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Collaborators' AND column_name = 'CollaboratorCode'
                    ) THEN
                        ALTER TABLE ""Collaborators"" RENAME COLUMN ""CollaboratorCode"" TO ""ReferralCode"";
                        ALTER TABLE ""Collaborators"" ALTER COLUMN ""ReferralCode"" TYPE character varying(50);
                    END IF;
                END $$;");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_ReferralCode",
                table: "Collaborators",
                column: "ReferralCode",
                unique: true,
                filter: "\"ReferralCode\" IS NOT NULL");
        }
    }
}