using AutoMapper;
using FlashOffer.API.Application.Common.Extensions;
using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Infrastructure.Services;

namespace FlashOffer.API.Application.Services;

public class CtvRegistrationService : ICtvRegistrationService
{
	private readonly IRepository<Collaborator> _repository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IQueryService _queryService;

	public CtvRegistrationService(
		IRepository<Collaborator> repository,
		IMapper mapper,
		IUserService userService,
		IQueryService queryService)
	{
		_repository = repository;
		_mapper = mapper;
		_userService = userService;
		_queryService = queryService;
	}

    public async Task<CtvRegistrationResponseDto> CreateAsync(CreateCtvRegistrationDto dto)
    {
        // 1. Lấy hoặc tạo User từ Phone
        var userId = await _userService.GetOrCreateUserAsync(
            dto.FullName,
            dto.Phone,
            dto.Email
        );

        // 2. Map và gán UserId
        var entity = _mapper.Map<Collaborator>(dto);
        entity.UserId = userId;
        entity.CollaboratorCode = CodeGenerator.Generate("CTV");
        entity.IsApproved = false;
        entity.CreatedAt = DateTime.UtcNow.AddHours(7);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CtvRegistrationResponseDto>(entity);
    }

    public async Task<PagedList<CtvRegistrationResponseDto>> GetPagedAsync(CtvRegistrationQueryDto query)
	{
		var q = _queryService.GetQueryableNoTracking<Collaborator>()
			.WhereIf(query.IsApproved.HasValue, x => x.IsApproved == query.IsApproved!.Value);

		var pagedEntities = await q.ToPagedListAsync(
			query.Page, query.PageSize,
			query.SortBy, query.SortOrder,
			defaultSortBy: "CreatedAt"
		);

		return _mapper.MapPagedList<Collaborator, CtvRegistrationResponseDto>(pagedEntities);
	}

	public async Task<CtvRegistrationResponseDto> ApproveAsync(Guid id)
	{
		var entity = await _repository.GetByIdAsync(id);
		if (entity == null)
			throw new KeyNotFoundException($"CtvRegistration with ID {id} not found");

		if (entity.IsApproved)
			throw new InvalidOperationException("CTV already approved");

		entity.IsApproved = true;
		entity.ApprovedAt = DateTime.UtcNow.AddHours(7); // UTC+7

		_repository.Update(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<CtvRegistrationResponseDto>(entity);
	}
}