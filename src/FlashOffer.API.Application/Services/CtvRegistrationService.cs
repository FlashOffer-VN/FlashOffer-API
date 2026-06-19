using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;

namespace FlashOffer.API.Application.Services;

public class CtvRegistrationService : ICtvRegistrationService
{
	private readonly IRepository<CtvRegistration> _repository;
	private readonly IMapper _mapper;

	public CtvRegistrationService(IRepository<CtvRegistration> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<CtvRegistrationResponseDto> CreateAsync(CreateCtvRegistrationDto dto)
	{
		var entity = _mapper.Map<CtvRegistration>(dto);
		entity.IsApproved = false;
		entity.CreatedAt = DateTime.UtcNow.AddHours(7); // UTC+7

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<CtvRegistrationResponseDto>(entity);
	}
}