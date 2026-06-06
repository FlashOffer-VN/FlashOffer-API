# Controller Template

namespace FlashOffer.API.WebApi.Controllers.v1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs;
using FlashOffer.API.Domain.Entities;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[{EntityName}]")]
[Authorize]
public class {EntityName}Controller : ApiControllerBase
{
    private readonly IRepository<{EntityName}> _repository;
    private readonly IMapper _mapper;

    public {EntityName}Controller(IRepository<{EntityName}> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _repository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<{EntityName}Dto>>(items));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return NotFound("{EntityName} not found");
        return Ok(_mapper.Map<{EntityName}Dto>(item));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create{EntityName}Dto createDto)
    {
        var entity = _mapper.Map<{EntityName}>(createDto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, 
            _mapper.Map<{EntityName}Dto>(entity));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Update{EntityName}Dto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("ID mismatch");

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound("{EntityName} not found");

        _mapper.Map(updateDto, existing);
        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return NoContent("{EntityName} updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound("{EntityName} not found");

        _repository.Delete(existing);
        await _repository.SaveChangesAsync();
        return NoContent("{EntityName} deleted successfully");
    }
}
