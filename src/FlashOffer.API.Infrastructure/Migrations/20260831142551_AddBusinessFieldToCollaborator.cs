using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessFieldToCollaborator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột BusinessFieldId vào bảng Collaborators (nếu chưa có)
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessFieldId",
                table: "Collaborators",
                type: "uuid",
                nullable: true);

            // Tạo index cho BusinessFieldId
            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_BusinessFieldId",
                table: "Collaborators",
                column: "BusinessFieldId");

            // Tạo foreign key tới bảng BusinessFields
            migrationBuilder.AddForeignKey(
                name: "FK_Collaborators_BusinessFields_BusinessFieldId",
                table: "Collaborators",
                column: "BusinessFieldId",
                principalTable: "BusinessFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborators_BusinessFields_BusinessFieldId",
                table: "Collaborators");

            // Xóa index
            migrationBuilder.DropIndex(
                name: "IX_Collaborators_BusinessFieldId",
                table: "Collaborators");

            // Xóa cột
            migrationBuilder.DropColumn(
                name: "BusinessFieldId",
                table: "Collaborators");
        }
    }
}