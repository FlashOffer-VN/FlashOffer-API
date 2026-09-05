using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Exceptions;
using AutoMapper;

namespace FlashOffer.API.Application.Services;

public class BusinessFieldService : IBusinessFieldService
{
    private readonly IRepository<BusinessField> _repository;
    private readonly IMapper _mapper;

    public BusinessFieldService(
        IRepository<BusinessField> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> GetOrCreateBusinessFieldAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Tên lĩnh vực không được để trống");

        var normalized = name.Trim().ToUpperInvariant();

        // 1. Tìm theo NormalizedName
        var existing = await _repository.GetFirstAsync(f => f.NormalizedName == normalized && !f.IsDeleted);
        if (existing != null) return existing.Id;

        // 2. Tìm theo Alias (nếu có)
        var allFields = await _repository.FindAsync(f => !f.IsDeleted);
        var matchedByAlias = allFields.FirstOrDefault(f =>
            !string.IsNullOrEmpty(f.Aliases) &&
            f.Aliases.Contains(normalized, StringComparison.OrdinalIgnoreCase));
        if (matchedByAlias != null) return matchedByAlias.Id;

        // 3. Tạo mới
        var field = new BusinessField
        {
            BusinessFieldCode = CodeGenerator.Generate("BSF"),
            Name = name.Trim(),
            NormalizedName = normalized,
            IsActive = true
        };

        await _repository.AddAsync(field);
        await _repository.SaveChangesAsync();

        return field.Id;
    }

    public async Task<List<BusinessFieldDto>> GetActiveFieldsAsync()
    {
        var fields = await _repository.FindAsync(f => f.IsActive && !f.IsDeleted);
        return _mapper.Map<List<BusinessFieldDto>>(fields);
    }
}