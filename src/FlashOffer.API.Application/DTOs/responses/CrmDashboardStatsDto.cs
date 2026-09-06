namespace FlashOffer.API.Application.DTOs.Responses;

/// <summary>
/// Thống kê CRM cho Admin.
/// Contract cho GET /api/v1/admin/crm (khớp với frontend CrmDashboardStats).
/// </summary>
public class CrmDashboardStatsDto
{
    public CrmSummaryDto Summary { get; set; } = new();
    public List<CrmTrendPointDto> Trends { get; set; } = new();
    public List<CrmDistributionSliceDto> Distribution { get; set; } = new();
    public List<CrmCategoryCountDto> TopCategories { get; set; } = new();
}

public class CrmSummaryDto
{
    public int TotalOffers { get; set; }
    public int PendingOffers { get; set; }
    public int TotalPurchaseRequests { get; set; }
    public int PendingPurchaseRequests { get; set; }
    public int TotalGroupBuyingRequests { get; set; }
    public int PendingGroupBuyingRequests { get; set; }
    public int TotalCtvRegistrations { get; set; }
    public int PendingCtvRegistrations { get; set; }
    public int TotalPartners { get; set; }
    public int PendingPartners { get; set; }
    public int TotalSocialPosts { get; set; }
}

public class CrmTrendPointDto
{
    /// <summary>Tháng dạng 'yyyy-MM', ví dụ '2026-01'.</summary>
    public string Month { get; set; } = string.Empty;
    public int Offers { get; set; }
    public int PurchaseRequests { get; set; }
    public int CtvRegistrations { get; set; }
    public int Partners { get; set; }
    /// <summary>Luôn 0 cho tới khi module doanh số ra mắt.</summary>
    public decimal Revenue { get; set; }
}

public class CrmDistributionSliceDto
{
    /// <summary>'offer' | 'purchase' | 'ctv' | 'partner'.</summary>
    public string Entity { get; set; } = string.Empty;
    /// <summary>Giá trị enum numeric tương ứng model hiện có.</summary>
    public int Status { get; set; }
    /// <summary>i18n key (vd 'COMMON.STATUS.PENDING') để frontend dịch.</summary>
    public string StatusLabel { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class CrmCategoryCountDto
{
    /// <summary>productCategory label.</summary>
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
