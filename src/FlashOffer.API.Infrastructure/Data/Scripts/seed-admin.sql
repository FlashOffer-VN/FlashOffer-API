-- Sử dụng PasswordHasher.Hash("Admin@123") để lấy hash
-- Chạy lệnh này trên production database
DECLARE @AdminId UNIQUEIDENTIFIER = NEWID();

IF NOT EXISTS (SELECT 1 FROM [Users] WHERE Username = 'admin')
BEGIN
    INSERT INTO [Users] (
        [Id],
        [Username],
        [PasswordHash],
        [FullName],
        [Email],
        [Phone],
        [IsActive],
        [Role],
        [CreatedAt],
        [IsDeleted]
    )
    VALUES (
        @AdminId,
        'admin',
        '$2a$11$KjZ9qN5xHzD6Zj3.9C/w3O8TQgPZ6XZv1YFk4X8YvVhZ7dQwJ5sMq', -- Admin@123
        'Administrator',
        'admin@flashoffer.com',
        '0987654321',
        1,
        3,
        GETUTCDATE(),
        0
    )
END