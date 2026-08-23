using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

public class CtvFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public CollaboratorStatus? Status { get; set; }
}