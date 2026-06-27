-- Seed admin user for PostgreSQL
-- Password: Admin@123 (BCrypt hash below)

INSERT INTO "Users" (
    "Id",
    "Username",
    "PasswordHash",
    "FullName",
    "Email",
    "Phone",
    "IsActive",
    "Role",
    "CreatedAt",
    "IsDeleted"
)
SELECT
    gen_random_uuid(),
    'admin',
    '$2a$11$KjZ9qN5xHzD6Zj3.9C/w3O8TQgPZ6XZv1YFk4X8YvVhZ7dQwJ5sMq',
    'Administrator',
    'admin@flashoffer.com',
    '0987654321',
    TRUE,
    3,
    NOW() AT TIME ZONE 'UTC',
    FALSE
WHERE NOT EXISTS (
    SELECT 1 FROM "Users" WHERE "Username" = 'admin'
);
