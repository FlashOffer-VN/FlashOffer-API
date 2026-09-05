// CtvRegistrationQueryDto.cs
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.requests;

public class CtvRegistrationQueryDto
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public bool? IsApproved { get; set; }
	public string? SortBy { get; set; }    // VD: "CreatedAt"
	public string? SortOrder { get; set; } // "asc" | "desc"
}