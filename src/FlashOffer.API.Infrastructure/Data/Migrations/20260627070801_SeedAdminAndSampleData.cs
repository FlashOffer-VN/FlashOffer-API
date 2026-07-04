using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashOffer.API.Infrastructure.Data.Migrations
{
	/// <inheritdoc />
	public partial class SeedAdminAndSampleData : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// 1. Seed Admin User
			migrationBuilder.Sql(@"
                INSERT INTO [Users] ([Id], [Username], [PasswordHash], [FullName], [Email], [Phone], [IsActive], [Role], [CreatedAt], [IsDeleted])
                VALUES 
                (NEWID(), 'admin', '$2a$11$KjZ9qN5xHzD6Zj3.9C/w3O8TQgPZ6XZv1YFk4X8YvVhZ7dQwJ5sMq', 'Administrator', 'admin@flashoffer.com', '0987654321', 1, 3, GETUTCDATE(), 0)
            ");

			// 2. Seed Users (khách hàng)
			migrationBuilder.Sql(@"
                DECLARE @User1 UNIQUEIDENTIFIER = NEWID();
                DECLARE @User2 UNIQUEIDENTIFIER = NEWID();
                DECLARE @User3 UNIQUEIDENTIFIER = NEWID();
                DECLARE @User4 UNIQUEIDENTIFIER = NEWID();

                INSERT INTO [Users] ([Id], [Username], [PasswordHash], [FullName], [Email], [Phone], [IsActive], [Role], [CreatedAt], [IsDeleted])
                VALUES 
                (@User1, 'nguyenvana', NULL, 'Nguyễn Văn A', 'nguyenvana@email.com', '0912345678', 1, 1, GETUTCDATE(), 0),
                (@User2, 'tranthib', NULL, 'Trần Thị B', 'tranthib@email.com', '0923456789', 1, 1, GETUTCDATE(), 0),
                (@User3, 'lethic', NULL, 'Lê Thị C', 'lethic@email.com', '0934567890', 1, 1, GETUTCDATE(), 0),
                (@User4, 'phamvand', NULL, 'Phạm Văn D', 'phamvand@email.com', '0945678901', 1, 1, GETUTCDATE(), 0)
            ");

			// 3. Seed Purchase Requests
			migrationBuilder.Sql(@"
                DECLARE @User1 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'nguyenvana');
                DECLARE @User2 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'tranthib');
                DECLARE @User3 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'lethic');
                DECLARE @User4 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'phamvand');
                DECLARE @AdminId UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'admin');

                INSERT INTO [PurchaseRequests] ([Id], [UserId], [ProductName], [Quantity], [ExpectedPrice], [FullName], [Phone], [Email], [Status], [CreatedAt], [IsDeleted])
                VALUES 
                (NEWID(), @User1, 'iPhone 15 Pro Max', 2, 25000000, 'Nguyễn Văn A', '0912345678', 'nguyenvana@email.com', 1, DATEADD(day, -5, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Laptop Dell XPS 13', 1, 18000000, 'Trần Thị B', '0923456789', 'tranthib@email.com', 1, DATEADD(day, -3, GETUTCDATE()), 0),
                (NEWID(), @User3, 'Samsung Galaxy S24', 3, 20000000, 'Lê Thị C', '0934567890', 'lethic@email.com', 2, DATEADD(day, -2, GETUTCDATE()), 0),
                (NEWID(), @User4, 'iPad Pro M2', 1, 15000000, 'Phạm Văn D', '0945678901', 'phamvand@email.com', 1, DATEADD(day, -1, GETUTCDATE()), 0),
                (NEWID(), @User1, 'MacBook Air M3', 1, 22000000, 'Nguyễn Văn A', '0912345678', 'nguyenvana@email.com', 3, DATEADD(day, -7, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Tai nghe Sony WH-1000XM5', 5, 8000000, 'Trần Thị B', '0923456789', 'tranthib@email.com', 1, GETUTCDATE(), 0),
                (NEWID(), @AdminId, 'Điện thoại Oppo Find X6', 2, 12000000, 'Administrator', '0987654321', 'admin@flashoffer.com', 1, DATEADD(day, -4, GETUTCDATE()), 0),
                (NEWID(), @User3, 'Đồng hồ Apple Watch Series 9', 2, 10000000, 'Lê Thị C', '0934567890', 'lethic@email.com', 2, DATEADD(day, -6, GETUTCDATE()), 0),
                (NEWID(), @User4, 'Máy ảnh Canon EOS R50', 1, 14000000, 'Phạm Văn D', '0945678901', 'phamvand@email.com', 1, DATEADD(day, -8, GETUTCDATE()), 0),
                (NEWID(), @User1, 'Loa Bluetooth JBL Charge 5', 3, 3000000, 'Nguyễn Văn A', '0912345678', 'nguyenvana@email.com', 1, DATEADD(day, -9, GETUTCDATE()), 0)
            ");

			// 4. Seed Group Buying Requests
			migrationBuilder.Sql(@"
                DECLARE @User1 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'nguyenvana');
                DECLARE @User2 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'tranthib');
                DECLARE @User3 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'lethic');
                DECLARE @User4 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'phamvand');

                INSERT INTO [GroupBuyingRequests] ([Id], [UserId], [ProductName], [TargetPeopleCount], [CurrentPeopleCount], [TargetPrice], [FullName], [Phone], [Status], [CreatedAt], [IsDeleted])
                VALUES 
                (NEWID(), @User1, 'Laptop Dell XPS 13', 10, 5, 18000000, 'Nguyễn Văn A', '0912345678', 1, DATEADD(day, -5, GETUTCDATE()), 0),
                (NEWID(), @User2, 'iPhone 15 Pro Max', 8, 3, 22000000, 'Trần Thị B', '0923456789', 2, DATEADD(day, -3, GETUTCDATE()), 0),
                (NEWID(), @User3, 'Samsung Galaxy S24', 6, 6, 18000000, 'Lê Thị C', '0934567890', 1, DATEADD(day, -2, GETUTCDATE()), 0),
                (NEWID(), @User4, 'MacBook Air M3', 5, 2, 20000000, 'Phạm Văn D', '0945678901', 1, DATEADD(day, -1, GETUTCDATE()), 0),
                (NEWID(), @User1, 'iPad Pro M2', 7, 4, 14000000, 'Nguyễn Văn A', '0912345678', 3, DATEADD(day, -7, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Tai nghe Sony WH-1000XM5', 12, 8, 7000000, 'Trần Thị B', '0923456789', 1, GETUTCDATE(), 0),
                (NEWID(), @User3, 'Đồng hồ Apple Watch Series 9', 9, 3, 9000000, 'Lê Thị C', '0934567890', 1, DATEADD(day, -4, GETUTCDATE()), 0),
                (NEWID(), @User4, 'Máy ảnh Canon EOS R50', 4, 1, 13000000, 'Phạm Văn D', '0945678901', 2, DATEADD(day, -6, GETUTCDATE()), 0)
            ");

			// 5. Seed Offer Requests
			migrationBuilder.Sql(@"
                DECLARE @User1 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'nguyenvana');
                DECLARE @User2 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'tranthib');
                DECLARE @User3 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'lethic');
                DECLARE @User4 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'phamvand');

                INSERT INTO [OfferRequests] ([Id], [UserId], [SelectedOffer], [FullName], [Phone], [Zalo], [Email], [Status], [IsOfferSent], [CreatedAt], [IsDeleted])
                VALUES 
                (NEWID(), @User1, 'Giảm 30% cho đơn hàng đầu tiên', 'Nguyễn Văn A', '0912345678', 'nguyenvana', 'nguyenvana@email.com', 1, 0, DATEADD(day, -5, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Tặng voucher 500k cho đơn hàng từ 5tr', 'Trần Thị B', '0923456789', 'tranthib', 'tranthib@email.com', 2, 1, DATEADD(day, -3, GETUTCDATE()), 0),
                (NEWID(), @User3, 'Miễn phí vận chuyển toàn quốc', 'Lê Thị C', '0934567890', 'lethic', 'lethic@email.com', 1, 0, DATEADD(day, -2, GETUTCDATE()), 0),
                (NEWID(), @User4, 'Giảm 20% cho khách hàng thân thiết', 'Phạm Văn D', '0945678901', 'phamvand', 'phamvand@email.com', 1, 0, DATEADD(day, -1, GETUTCDATE()), 0),
                (NEWID(), @User1, 'Combo 2 sản phẩm giá ưu đãi', 'Nguyễn Văn A', '0912345678', 'nguyenvana', 'nguyenvana@email.com', 3, 1, DATEADD(day, -7, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Giảm 15% cho đơn hàng thứ 2', 'Trần Thị B', '0923456789', 'tranthib', 'tranthib@email.com', 1, 0, GETUTCDATE(), 0),
                (NEWID(), @User3, 'Tặng quà tặng trị giá 200k', 'Lê Thị C', '0934567890', 'lethic', 'lethic@email.com', 2, 1, DATEADD(day, -4, GETUTCDATE()), 0),
                (NEWID(), @User4, 'Giảm 25% cho sản phẩm mới', 'Phạm Văn D', '0945678901', 'phamvand', 'phamvand@email.com', 1, 0, DATEADD(day, -6, GETUTCDATE()), 0)
            ");

			// 6. Seed CTV Registrations
			migrationBuilder.Sql(@"
                DECLARE @User1 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'nguyenvana');
                DECLARE @User2 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'tranthib');
                DECLARE @User3 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'lethic');
                DECLARE @User4 UNIQUEIDENTIFIER = (SELECT Id FROM [Users] WHERE Username = 'phamvand');

                INSERT INTO [CtvRegistrations] ([Id], [UserId], [FullName], [Phone], [Zalo], [Email], [SalesChannel], [Experience], [Status], [IsApproved], [CreatedAt], [IsDeleted])
                VALUES 
                (NEWID(), @User1, 'Nguyễn Văn A', '0912345678', 'nguyenvana', 'nguyenvana@email.com', 'Facebook, Zalo', 'Đã bán hàng online 3 năm', 1, 0, DATEADD(day, -5, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Trần Thị B', '0923456789', 'tranthib', 'tranthib@email.com', 'TikTok, Shopee', 'Kinh nghiệm bán hàng 2 năm', 2, 1, DATEADD(day, -3, GETUTCDATE()), 0),
                (NEWID(), @User3, 'Lê Thị C', '0934567890', 'lethic', 'lethic@email.com', 'Facebook, Instagram', 'Đã bán hàng online 4 năm', 1, 0, DATEADD(day, -2, GETUTCDATE()), 0),
                (NEWID(), @User4, 'Phạm Văn D', '0945678901', 'phamvand', 'phamvand@email.com', 'Zalo, Lazada', 'Kinh nghiệm bán hàng 1 năm', 3, 0, DATEADD(day, -1, GETUTCDATE()), 0),
                (NEWID(), @User1, 'Nguyễn Văn A', '0912345678', 'nguyenvana', 'nguyenvana@email.com', 'Facebook, Tiki', 'Đã bán hàng online 5 năm', 2, 1, DATEADD(day, -7, GETUTCDATE()), 0),
                (NEWID(), @User2, 'Trần Thị B', '0923456789', 'tranthib', 'tranthib@email.com', 'Shopee, Zalo', 'Kinh nghiệm bán hàng 2 năm', 1, 0, GETUTCDATE(), 0)
            ");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DELETE FROM [Users] WHERE Username = 'admin'");
			migrationBuilder.Sql("DELETE FROM [Users] WHERE Username IN ('nguyenvana', 'tranthib', 'lethic', 'phamvand')");
		}
	}
}