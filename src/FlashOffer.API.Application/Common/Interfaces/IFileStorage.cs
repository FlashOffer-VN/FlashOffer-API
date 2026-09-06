namespace FlashOffer.API.Application.Common.Interfaces;

/// <summary>
/// Abstraction lưu trữ file. Hiện tại dùng local disk (LocalFileStorage);
/// khi chuyển sang bên thứ 3 (S3, Cloudinary, ...) chỉ cần thêm impl mới + đổi DI.
/// </summary>
public interface IFileStorage
{
	/// <summary>
	/// Lưu file từ stream, tự sinh tên file an toàn, trả về path/url tương đối.
	/// </summary>
	Task<StoredFile> SaveAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);

	/// <summary>
	/// Xóa file theo path tương đối (vd: "uploads/2026/09/abc.png"). Trả false nếu không tồn tại.
	/// </summary>
	Task<bool> DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
}

public record StoredFile(string Path, string Url, long SizeBytes);