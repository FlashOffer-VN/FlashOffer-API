using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBusinessFields : Migration
    {
        /// <summary>
        /// Danh sách lĩnh vực hoạt động chuẩn. Id cố định để mọi môi trường dùng chung
        /// một tập giá trị. NormalizedName viết HOA — đúng convention của
        /// BusinessFieldService.GetOrCreateBusinessFieldAsync (ToUpperInvariant).
        /// Aliases chứa slug tiếng Anh cũ (form đăng ký từng gửi 'technology', 'trade'...)
        /// để dữ liệu cũ vẫn khớp qua nhánh tìm theo alias.
        /// </summary>
        private static readonly (string Id, string Code, string Name, string Aliases)[] Fields =
        {
            ("b5f00001-0000-4000-8000-000000000001", "BSF-TECH",  "Công nghệ thông tin",      "[\"TECHNOLOGY\",\"CNTT\"]"),
            ("b5f00001-0000-4000-8000-000000000002", "BSF-MANU",  "Sản xuất - Chế tạo",       "[\"MANUFACTURING\",\"SAN XUAT\"]"),
            ("b5f00001-0000-4000-8000-000000000003", "BSF-TRADE", "Thương mại - Dịch vụ",     "[\"TRADE\",\"THUONG MAI\"]"),
            ("b5f00001-0000-4000-8000-000000000004", "BSF-AGRI",  "Nông nghiệp - Thực phẩm",  "[\"AGRICULTURE\",\"NONG NGHIEP\"]"),
            ("b5f00001-0000-4000-8000-000000000005", "BSF-CONS",  "Xây dựng - Bất động sản",  "[\"CONSTRUCTION\",\"XAY DUNG\"]"),
            ("b5f00001-0000-4000-8000-000000000006", "BSF-EDU",   "Giáo dục - Đào tạo",       "[\"EDUCATION\",\"GIAO DUC\"]"),
            ("b5f00001-0000-4000-8000-000000000007", "BSF-HEAL",  "Y tế - Chăm sóc sức khỏe", "[\"HEALTHCARE\",\"Y TE\"]"),
            ("b5f00001-0000-4000-8000-000000000008", "BSF-FIN",   "Tài chính - Ngân hàng",    "[\"FINANCE\",\"TAI CHINH\"]"),
            ("b5f00001-0000-4000-8000-000000000009", "BSF-LOGI",  "Vận tải - Logistics",      "[\"LOGISTICS\",\"VAN TAI\"]"),
            ("b5f00001-0000-4000-8000-000000000010", "BSF-CONSU", "Tư vấn - Chiến lược",      "[\"CONSULTING\",\"TU VAN\"]"),
            ("b5f00001-0000-4000-8000-000000000011", "BSF-OTHER", "Lĩnh vực khác",            "[\"OTHER\",\"KHAC\"]")
        };

        /// <summary>Các bảng có cột BusinessFieldId — dùng để chặn xoá khi rollback.</summary>
        private static readonly string[] ReferencingTables =
        {
            "Collaborators", "Partners", "PartnerProducts",
            "PurchaseRequests", "GroupBuyingRequests", "OfferRequests"
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Idempotent: ON CONFLICT DO NOTHING phủ cả unique index trên NormalizedName
            // và BusinessFieldCode, nên chạy lại trên DB đã có sẵn lĩnh vực (do
            // find-or-create tạo trước đó) không vỡ.
            foreach (var (id, code, name, aliases) in Fields)
            {
                var escapedName = name.Replace("'", "''");

                migrationBuilder.Sql($@"
                    INSERT INTO ""BusinessFields""
                        (""Id"", ""BusinessFieldCode"", ""Name"", ""NormalizedName"",
                         ""Aliases"", ""IsActive"", ""IsDeleted"", ""CreatedAt"", ""CreatedBy"")
                    VALUES
                        ('{id}', '{code}', '{escapedName}', UPPER('{escapedName}'),
                         '{aliases}', TRUE, FALSE, now(), 'SeedBusinessFields')
                    ON CONFLICT DO NOTHING;");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Chỉ xoá lĩnh vực do seed tạo và chưa được bảng nào tham chiếu,
            // tránh vỡ FK khi đã có CTV/Partner/request chọn lĩnh vực đó.
            var ids = string.Join(", ", Fields.Select(f => $"'{f.Id}'"));
            var notReferenced = string.Join("\n                  ", ReferencingTables.Select(t =>
                $@"AND NOT EXISTS (SELECT 1 FROM ""{t}"" t WHERE t.""BusinessFieldId"" = bf.""Id"")"));

            migrationBuilder.Sql($@"
                DELETE FROM ""BusinessFields"" bf
                WHERE bf.""Id"" IN ({ids})
                  AND bf.""CreatedBy"" = 'SeedBusinessFields'
                  {notReferenced};");
        }
    }
}
