namespace FlashOffer.API.Application.DTOs.Responses;

/// <summary>
/// Báo cáo tổng quan hệ thống cho Admin
/// </summary>
public class ReportOverviewDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    // ===== Người dùng =====
    public int TotalUsers { get; set; }
    public int NewUsers { get; set; }
    public int ActiveUsers { get; set; }

    // ===== Đối tác =====
    public int TotalPartners { get; set; }
    public int NewPartners { get; set; }
    public int PendingPartners { get; set; }
    public int ApprovedPartners { get; set; }

    // ===== Cộng tác viên =====
    public int TotalCollaborators { get; set; }
    public int NewCollaborators { get; set; }
    public int PendingCollaborators { get; set; }
    public int ApprovedCollaborators { get; set; }

    // ===== Bài viết =====
    public int TotalPosts { get; set; }
    public int NewPosts { get; set; }
    public int PendingPosts { get; set; }
    public int ApprovedPosts { get; set; }

    // ===== Yêu cầu mua hàng =====
    public int TotalPurchaseRequests { get; set; }
    public int PendingPurchaseRequests { get; set; }

    // ===== Yêu cầu mua nhóm =====
    public int TotalGroupBuyingRequests { get; set; }
    public int PendingGroupBuyingRequests { get; set; }

    // ===== Yêu cầu báo giá =====
    public int TotalOfferRequests { get; set; }
    public int PendingOfferRequests { get; set; }
}

/// <summary>
/// Điểm dữ liệu của biểu đồ xu hướng theo ngày
/// </summary>
public class ReportTrendPointDto
{
    public DateTime Date { get; set; }
    public int NewUsers { get; set; }
    public int NewPartners { get; set; }
    public int NewCollaborators { get; set; }
    public int NewPosts { get; set; }
    public int NewRequests { get; set; }
}

/// <summary>
/// Báo cáo xu hướng tăng trưởng theo ngày
/// </summary>
public class ReportTrendDto
{
    public int Days { get; set; }
    public List<ReportTrendPointDto> Points { get; set; } = new();
}

/// <summary>
/// Chi tiết một loại yêu cầu phân theo trạng thái
/// </summary>
public class RequestBreakdownDto
{
    public string Type { get; set; } = string.Empty;   // purchase | groupbuying | offer
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// Báo cáo yêu cầu (mua hàng / mua nhóm / báo giá)
/// </summary>
public class RequestReportDto
{
    public List<RequestBreakdownDto> Items { get; set; } = new();
}

/// <summary>
/// Số lượng bài viết theo loại
/// </summary>
public class PostsByTypeDto
{
    public string Type { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// Bài viết nổi bật
/// </summary>
public class TopPostDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public int SharesCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Báo cáo hoạt động mạng xã hội
/// </summary>
public class SocialReportDto
{
    public int TotalPosts { get; set; }
    public int TotalLikes { get; set; }
    public int TotalComments { get; set; }
    public int TotalShares { get; set; }
    public List<PostsByTypeDto> PostsByType { get; set; } = new();
    public List<TopPostDto> TopPosts { get; set; } = new();
}

/// <summary>
/// Cặp nhãn - số lượng dùng cho báo cáo phân nhóm
/// </summary>
public class NameCountDto
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// Báo cáo đối tác
/// </summary>
public class PartnerReportDto
{
    public int Total { get; set; }
    public List<NameCountDto> ByStatus { get; set; } = new();
    public List<NameCountDto> ByBusinessType { get; set; } = new();
    public List<NameCountDto> ByBusinessField { get; set; } = new();
}

/// <summary>
/// Báo cáo cộng tác viên (CTV)
/// </summary>
public class CollaboratorReportDto
{
    public int Total { get; set; }
    public List<NameCountDto> ByStatus { get; set; } = new();
    public List<NameCountDto> ByLevel { get; set; } = new();
    public List<NameCountDto> BySalesChannel { get; set; } = new();
}

/// <summary>
/// Báo cáo thành viên (đối tác + cộng tác viên)
/// </summary>
public class MembersReportDto
{
    public PartnerReportDto Partners { get; set; } = new();
    public CollaboratorReportDto Collaborators { get; set; } = new();
}