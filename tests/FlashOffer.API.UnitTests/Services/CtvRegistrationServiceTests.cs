using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

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
}