// BusinessType.cs
namespace FlashOffer.API.Domain.Enums;

public enum BusinessType
{
    SME = 1,          // Doanh nghiệp vừa và nhỏ
    SoleProprietor = 2, // Hộ kinh doanh cá thể
    Partnership = 3,   // Công ty hợp danh
    Corporation = 4,   // Công ty cổ phần
    Limited = 5,       // Công ty TNHH
    Other = 6          // Khác
}