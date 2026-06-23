using System.Linq.Expressions;
using AutoMapper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using Moq;
using Xunit;

namespace FlashOffer.API.UnitTests.Services;

public class PurchaseRequestServiceTests
{
	private readonly Mock<IRepository<PurchaseRequest>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly PurchaseRequestService _service;

	public PurchaseRequestServiceTests()
	{
		_repositoryMock = new Mock<IRepository<PurchaseRequest>>();
		_mapperMock = new Mock<IMapper>();
		_service = new PurchaseRequestService(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task GetPagedAsync_ShouldReturnPagedResult()
	{
		// Arrange
		var query = new PurchaseRequestQueryDto { Page = 1, PageSize = 10 };
		var entities = new List<PurchaseRequest>
	{
		new() { Id = Guid.NewGuid(), ProductName = "Test 1", Status = PurchaseRequestStatus.Pending },
		new() { Id = Guid.NewGuid(), ProductName = "Test 2", Status = PurchaseRequestStatus.Contacted }
	};
		var pagedEntities = new PagedList<PurchaseRequest>(entities, entities.Count, 1, 10);
		var dtos = new List<PurchaseRequestResponseDto>
	{
		new() { ProductName = "Test 1", Status = PurchaseRequestStatus.Pending },
		new() { ProductName = "Test 2", Status = PurchaseRequestStatus.Contacted }
	};

		_repositoryMock.Setup(r => r.GetPagedWithOrderAsync(
			It.IsAny<int>(),
			It.IsAny<int>(),
			It.IsAny<Expression<Func<PurchaseRequest, bool>>>(),
			It.IsAny<Expression<Func<PurchaseRequest, object>>>(),
			It.IsAny<bool>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedEntities);

		// ✅ Sửa: Setup với entities cụ thể
		_mapperMock.Setup(m => m.Map<List<PurchaseRequestResponseDto>>(entities))
			.Returns(dtos);

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, result.TotalCount);
		Assert.Equal(2, result.Items.Count);
		Assert.NotNull(result.Items); // 👈 Thêm check này
	}

	[Fact]
	public async Task GetPagedAsync_FilterByStatus_ShouldReturnFiltered()
	{
		// Arrange
		var query = new PurchaseRequestQueryDto
		{
			Page = 1,
			PageSize = 10,
			Status = PurchaseRequestStatus.Pending
		};

		var entities = new List<PurchaseRequest>
	{
		new() { Id = Guid.NewGuid(), ProductName = "Test 1", Status = PurchaseRequestStatus.Pending }
	};
		var pagedEntities = new PagedList<PurchaseRequest>(entities, entities.Count, 1, 10);
		var dtos = new List<PurchaseRequestResponseDto>
	{
		new() { ProductName = "Test 1", Status = PurchaseRequestStatus.Pending }
	};

		_repositoryMock.Setup(r => r.GetPagedWithOrderAsync(
			It.IsAny<int>(),
			It.IsAny<int>(),
			It.IsAny<Expression<Func<PurchaseRequest, bool>>>(),
			It.IsAny<Expression<Func<PurchaseRequest, object>>>(),
			It.IsAny<bool>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedEntities);

		// ✅ Thêm setup mapper cho test này
		_mapperMock.Setup(m => m.Map<List<PurchaseRequestResponseDto>>(entities))
			.Returns(dtos);

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Single(result.Items);
		Assert.NotNull(result.Items); // 👈 Thêm check này
	}
}