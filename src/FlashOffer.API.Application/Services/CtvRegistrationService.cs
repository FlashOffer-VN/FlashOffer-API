using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Infrastructure.Services;
using System.Linq.Expressions;

namespace FlashOffer.API.Application.Services;

public class CtvRegistrationService : ICtvRegistrationService
{
	private readonly IRepository<CtvRegistration> _repository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

	public CtvRegistrationService(IRepository<CtvRegistration> repository, IMapper mapper, IUserService userService)
	{
		_repository = repository;
		_mapper = mapper;
		_userService = userService;
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
        var entity = _mapper.Map<CtvRegistration>(dto);
        entity.UserId = userId;
        entity.IsApproved = false;
        entity.CreatedAt = DateTime.UtcNow.AddHours(7);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CtvRegistrationResponseDto>(entity);
    }

    public async Task<PagedList<CtvRegistrationResponseDto>> GetPagedAsync(CtvRegistrationQueryDto query)
	{
		var predicate = BuildPredicate(query.IsApproved);

		var pagedEntities = await _repository.GetPagedWithOrderAsync(
			query.Page,
			query.PageSize,
			predicate,
			x => x.CreatedAt,
			true
		);

		var items = _mapper.Map<List<CtvRegistrationResponseDto>>(pagedEntities.Items);
		return new PagedList<CtvRegistrationResponseDto>(items, pagedEntities.TotalCount, query.Page, query.PageSize);
	}

	private static Expression<Func<CtvRegistration, bool>>? BuildPredicate(bool? isApproved)
	{
		if (!isApproved.HasValue) return null;
		return x => x.IsApproved == isApproved.Value;
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