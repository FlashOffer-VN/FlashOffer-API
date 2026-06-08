using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.Services;

public class GroupBuyingRequestService : IGroupBuyingRequestService
{
	private readonly IRepository<GroupBuyingRequest> _repository;
	private readonly IMapper _mapper;

	public GroupBuyingRequestService(IRepository<GroupBuyingRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<GroupBuyingRequestResponseDto> CreateAsync(CreateGroupBuyingRequestDto request)
	{
		var entity = _mapper.Map<GroupBuyingRequest>(request);
		entity.CurrentPeopleCount = 1;
		entity.Status = GroupBuyingStatus.Pending;

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<GroupBuyingRequestResponseDto>(entity);
	}
}