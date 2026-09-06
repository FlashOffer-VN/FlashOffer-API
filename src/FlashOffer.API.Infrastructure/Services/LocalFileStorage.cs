using FlashOffer.API.Application.Common.Interfaces;

namespace FlashOffer.API.Infrastructure.Services;

/// <summary>
/// Lưu file trên local disk (wwwroot/uploads/yyyy/MM/dd/...).
/// Khi deploy chuyển sang blob storage bên thứ 3, chỉ cần thay impl này.
/// </summary>
public class LocalFileStorage : IFileStorage
{
	private readonly string _rootPath;

	public LocalFileStorage(string rootPath)
	{
		_rootPath = rootPath;
	}

	public async Task<StoredFile> SaveAsync(
		Stream stream,
		string fileName,
		string contentType,
		CancellationToken cancellationToken = default)
	{
		var subDir = DateTime.UtcNow.ToString("yyyy/MM/dd");
		var relativeDir = Path.Combine("uploads", subDir);
		var absoluteDir = Path.Combine(_rootPath, relativeDir);
		Directory.CreateDirectory(absoluteDir);

		var ext = Path.GetExtension(fileName);
		if (string.IsNullOrEmpty(ext))
		{
			ext = contentType switch
			{
				"image/png" => ".png",
				"image/jpeg" => ".jpg",
				"image/gif" => ".gif",
				"image/webp" => ".webp",
				"image/svg+xml" => ".svg",
				_ => ".bin"
			};
		}

		var name = $"{Guid.NewGuid():N}{ext}";
		var relativePath = Path.Combine(relativeDir, name).Replace('\\', '/');
		var fullPath = Path.Combine(absoluteDir, name);

		await using (var fs = File.Create(fullPath))
		{
			await stream.CopyToAsync(fs, cancellationToken);
		}

		return new StoredFile(relativePath, "/" + relativePath, new FileInfo(fullPath).Length);
	}

	public Task<bool> DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
	{
		var safePath = relativePath.TrimStart('/').Replace('\\', '/');
		var rootFull = Path.GetFullPath(_rootPath);
		var fullPath = Path.GetFullPath(Path.Combine(_rootPath, safePath));

		// Chống path traversal: file phải nằm trong thư mục gốc.
		if (!fullPath.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
		{
			return Task.FromResult(false);
		}

		if (!File.Exists(fullPath))
		{
			return Task.FromResult(false);
		}

		File.Delete(fullPath);
		return Task.FromResult(true);
	}
}