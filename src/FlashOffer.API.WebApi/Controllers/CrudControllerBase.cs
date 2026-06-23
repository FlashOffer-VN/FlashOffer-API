using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.WebApi.Controllers;

[Authorize]
public abstract class CrudControllerBase<TEntity, TDto> : ApiControllerBase
	where TEntity : class
{
	protected readonly IRepository<TEntity> _repository;
	protected readonly IMapper _mapper;

	protected CrudControllerBase(IRepository<TEntity> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet("paged")]
	[AllowAnonymous]
	public virtual async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var pagedEntities = await _repository.GetPagedAsync(pageNumber, pageSize);
		return Ok(_mapper.MapPagedList<TEntity, TDto>(pagedEntities));
	}

	[HttpGet("{id:guid}")]
	[AllowAnonymous]
	public virtual async Task<IActionResult> GetById(Guid id)
	{
		var entity = await _repository.GetByIdAsync(id);
		if (entity == null) throw new NotFoundException(typeof(TEntity).Name, id);
		return Ok(_mapper.Map<TDto>(entity));
	}

	[HttpPost]
	public virtual async Task<IActionResult> Create([FromBody] TDto createDto)
	{
		var entity = _mapper.Map<TEntity>(createDto);
		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		var dto = _mapper.Map<TDto>(entity);
		return CreatedAtAction(nameof(GetById), new { id = (entity as dynamic).Id }, dto);
	}

	[HttpPut("{id:guid}")]
	public virtual async Task<IActionResult> Update(Guid id, [FromBody] TDto updateDto)
	{
		var existing = await _repository.GetByIdAsync(id);
		if (existing == null) throw new NotFoundException(typeof(TEntity).Name, id);

		_mapper.Map(updateDto, existing);
		_repository.Update(existing);
		await _repository.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public virtual async Task<IActionResult> Delete(Guid id)
	{
		var entity = await _repository.GetByIdAsync(id);
		if (entity == null) throw new NotFoundException(typeof(TEntity).Name, id);

		_repository.Delete(entity);
		await _repository.SaveChangesAsync();
		return NoContent();
	}
}