using FlashOffer.API.Shared.Common.Helpers;

Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine("║       PASSWORD HASHER TOOL            ║");
Console.WriteLine("╚════════════════════════════════════════╝");
Console.WriteLine();

while (true)
{
    Console.Write("Nhập password (hoặc 'exit' để thoát): ");
    var password = Console.ReadLine();

    if (string.IsNullOrEmpty(password))
        continue;

    if (password.ToLower() == "exit")
        break;

    var hash = PasswordHasher.Hash(password);
    var verify = PasswordHasher.Verify(password, hash);

    Console.WriteLine($"Hash: {hash}");
    Console.WriteLine($"Verify: {verify}");
    Console.WriteLine(new string('-', 50));
    Console.WriteLine();
}

Console.WriteLine("Đã thoát!");