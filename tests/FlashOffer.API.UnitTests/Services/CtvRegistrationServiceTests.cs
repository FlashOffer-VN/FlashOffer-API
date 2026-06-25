using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace FlashOffer.API.UnitTests.Services;

public class CtvRegistrationServiceTests
{
	private readonly Mock<IRepository<CtvRegistration>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly CtvRegistrationService _service;

	public CtvRegistrationServiceTests()
	{
		_repositoryMock = new Mock<IRepository<CtvRegistration>>();
		_mapperMock = new Mock<IMapper>();
		_service = new CtvRegistrationService(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task CreateAsync_ShouldCreateCtvRegistration()
	{
		// Arrange
		var dto = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",
			Phone = "0933123456"
		};

		var entity = new CtvRegistration { FullName = dto.FullName, Phone = dto.Phone };
		var response = new CtvRegistrationResponseDto { Id = entity.Id, FullName = dto.FullName, Phone = dto.Phone };

		_mapperMock.Setup(m => m.Map<CtvRegistration>(dto)).Returns(entity);
		_mapperMock.Setup(m => m.Map<CtvRegistrationResponseDto>(entity)).Returns(response);

		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		var result = await _service.CreateAsync(dto);

		// Assert
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()), Times.Once);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		Assert.Equal(response.FullName, result.FullName);
		Assert.False(entity.IsApproved);
	}

	[Fact]
	public async Task CreateAsync_WhenSaveFails_ShouldThrowException()
	{
		// Arrange
		var dto = new CreateCtvRegistrationDto
		{
			FullName = "Nguyen Van A",
			Phone = "0933123456"
		};

		var entity = new CtvRegistration { FullName = dto.FullName, Phone = dto.Phone };

		_mapperMock.Setup(m => m.Map<CtvRegistration>(dto)).Returns(entity);
		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new DbUpdateException("Database error"));

		// Act & Assert
		await Assert.ThrowsAsync<DbUpdateException>(() => _service.CreateAsync(dto));
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<CtvRegistration>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task GetPagedAsync_ShouldReturnPagedList_WithFilter()
	{
		// Arrange
		var query = new CtvRegistrationQueryDto
		{
			Page = 1,
			PageSize = 10,
			IsApproved = true
		};

		var entities = new List<CtvRegistration>
	{
		new() { Id = Guid.NewGuid(), FullName = "A", IsApproved = true, CreatedAt = DateTime.UtcNow },
		new() { Id = Guid.NewGuid(), FullName = "B", IsApproved = true, CreatedAt = DateTime.UtcNow }
	};

		var pagedEntities = new PagedList<CtvRegistration>(entities, 2, 1, 10);
		var responseDtos = entities.Select(e => new CtvRegistrationResponseDto
		{
			Id = e.Id,
			FullName = e.FullName,
			IsApproved = e.IsApproved
		}).ToList();

		_repositoryMock
			.Setup(r => r.GetPagedWithOrderAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<Expression<Func<CtvRegistration, bool>>>(),
				It.IsAny<Expression<Func<CtvRegistration, object>>>(),
				It.IsAny<bool>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedEntities);

		_mapperMock
			.Setup(m => m.Map<List<CtvRegistrationResponseDto>>(pagedEntities.Items))
			.Returns(responseDtos);

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, result.TotalCount);
		Assert.Equal(1, result.PageNumber);
		Assert.Equal(10, result.PageSize);
		Assert.All(result.Items, item => Assert.True(item.IsApproved));

		_repositoryMock.Verify(r => r.GetPagedWithOrderAsync(
			query.Page,
			query.PageSize,
			It.IsAny<Expression<Func<CtvRegistration, bool>>>(),
			It.IsAny<Expression<Func<CtvRegistration, object>>>(),
			true,
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task GetPagedAsync_WithNoFilter_ShouldReturnAll()
	{
		// Arrange
		var query = new CtvRegistrationQueryDto { Page = 1, PageSize = 5, IsApproved = null };
		var entities = new List<CtvRegistration>
	{
		new() { Id = Guid.NewGuid(), FullName = "A", IsApproved = false },
		new() { Id = Guid.NewGuid(), FullName = "B", IsApproved = true }
	};
		var pagedEntities = new PagedList<CtvRegistration>(entities, 2, 1, 5);

		_repositoryMock
			.Setup(r => r.GetPagedWithOrderAsync(1, 5, null, It.IsAny<Expression<Func<CtvRegistration, object>>>(), true, It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedEntities);

		_mapperMock.Setup(m => m.Map<List<CtvRegistrationResponseDto>>(pagedEntities.Items))
			.Returns(new List<CtvRegistrationResponseDto>());

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, result.TotalCount);
		_repositoryMock.Verify(r => r.GetPagedWithOrderAsync(1, 5, null, It.IsAny<Expression<Func<CtvRegistration, object>>>(), true, It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task ApproveAsync_WithValidId_ShouldApproveSuccessfully()
	{
		// Arrange
		var id = Guid.NewGuid();
		var entity = new CtvRegistration
		{
			Id = id,
			FullName = "Nguyen Van A",
			Phone = "0912345678",
			IsApproved = false,
			ApprovedAt = null
		};
		var response = new CtvRegistrationResponseDto
		{
			Id = id,
			FullName = entity.FullName,
			IsApproved = true,
			ApprovedAt = DateTime.UtcNow.AddHours(7)
		};

		_repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(entity);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);
		_mapperMock.Setup(m => m.Map<CtvRegistrationResponseDto>(entity))
			.Returns(response);

		// Act
		var result = await _service.ApproveAsync(id);

		// Assert
		Assert.True(entity.IsApproved);
		Assert.NotNull(entity.ApprovedAt);
		_repositoryMock.Verify(r => r.Update(entity), Times.Once);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		Assert.Equal(response.FullName, result.FullName);
	}

	[Fact]
	public async Task ApproveAsync_WithNotFoundId_ShouldThrowKeyNotFoundException()
	{
		// Arrange
		var id = Guid.NewGuid();
		_repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((CtvRegistration?)null);

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ApproveAsync(id));
		_repositoryMock.Verify(r => r.Update(It.IsAny<CtvRegistration>()), Times.Never);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task ApproveAsync_WithAlreadyApproved_ShouldThrowInvalidOperationException()
	{
		// Arrange
		var id = Guid.NewGuid();
		var entity = new CtvRegistration
		{
			Id = id,
			FullName = "Nguyen Van A",
			IsApproved = true,
			ApprovedAt = DateTime.UtcNow.AddHours(-1)
		};

		_repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(entity);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ApproveAsync(id));
		_repositoryMock.Verify(r => r.Update(It.IsAny<CtvRegistration>()), Times.Never);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}
}