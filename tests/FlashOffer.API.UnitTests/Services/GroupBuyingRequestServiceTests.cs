using AutoMapper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace FlashOffer.API.UnitTests.Services;

public class GroupBuyingRequestServiceTests
{
	private readonly Mock<IRepository<GroupBuyingRequest>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly GroupBuyingRequestService _service;

	public GroupBuyingRequestServiceTests()
	{
		_repositoryMock = new Mock<IRepository<GroupBuyingRequest>>();
		_mapperMock = new Mock<IMapper>();
		_service = new GroupBuyingRequestService(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task GetPagedAsync_WithStatusFilter_ShouldReturnFilteredResults()
	{
		// Arrange
		var query = new GetGroupBuyingRequestsQueryDto
		{
			Page = 1,
			PageSize = 10,
			Status = "Pending"
		};

		var entities = new List<GroupBuyingRequest>
		{
			new GroupBuyingRequest
			{
				Id = Guid.NewGuid(),
				ProductName = "Test Product",
				Status = GroupBuyingStatus.Pending,
				CreatedAt = DateTime.UtcNow
			}
		};

		var pagedList = new PagedList<GroupBuyingRequest>(entities, 1, 1, 10);

		var responseDtos = new List<GroupBuyingRequestResponseDto>
		{
			new GroupBuyingRequestResponseDto
			{
				Id = entities[0].Id,
				ProductName = "Test Product",
				Status = GroupBuyingStatus.Pending
			}
		};

		_repositoryMock
			.Setup(r => r.GetPagedAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<Expression<Func<GroupBuyingRequest, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedList);

		_mapperMock
			.Setup(m => m.Map<List<GroupBuyingRequestResponseDto>>(It.IsAny<List<GroupBuyingRequest>>()))
			.Returns(responseDtos);

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(1, result.TotalCount);
		Assert.Equal(1, result.PageNumber);
		Assert.Single(result.Items);
		Assert.Equal("Test Product", result.Items[0].ProductName);
	}

	[Fact]
	public async Task GetPagedAsync_WithoutStatus_ShouldReturnAllResults()
	{
		// Arrange
		var query = new GetGroupBuyingRequestsQueryDto
		{
			Page = 1,
			PageSize = 20
		};

		var entities = new List<GroupBuyingRequest>();
		var pagedList = new PagedList<GroupBuyingRequest>(entities, 0, 1, 20);

		_repositoryMock
			.Setup(r => r.GetPagedAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<Expression<Func<GroupBuyingRequest, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedList);

		_mapperMock
			.Setup(m => m.Map<List<GroupBuyingRequestResponseDto>>(It.IsAny<List<GroupBuyingRequest>>()))
			.Returns(new List<GroupBuyingRequestResponseDto>());

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(0, result.TotalCount);
		Assert.Equal(1, result.PageNumber);
	}

	[Fact]
	public async Task GetPagedAsync_WithInvalidStatus_ShouldIgnoreFilter()
	{
		// Arrange
		var query = new GetGroupBuyingRequestsQueryDto
		{
			Page = 1,
			PageSize = 10,
			Status = "InvalidStatus"
		};

		var entities = new List<GroupBuyingRequest>();
		var pagedList = new PagedList<GroupBuyingRequest>(entities, 0, 1, 10);

		_repositoryMock
			.Setup(r => r.GetPagedAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<Expression<Func<GroupBuyingRequest, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(pagedList);

		_mapperMock
			.Setup(m => m.Map<List<GroupBuyingRequestResponseDto>>(It.IsAny<List<GroupBuyingRequest>>()))
			.Returns(new List<GroupBuyingRequestResponseDto>());

		// Act
		var result = await _service.GetPagedAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(0, result.TotalCount);
		Assert.Equal(1, result.PageNumber);
	}

	[Fact]
	public async Task GetPagedAsync_WhenRepositoryThrowsException_ShouldThrow()
	{
		// Arrange
		var query = new GetGroupBuyingRequestsQueryDto { Page = 1, PageSize = 10 };

		_repositoryMock
			.Setup(r => r.GetPagedAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<Expression<Func<GroupBuyingRequest, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ThrowsAsync(new DbUpdateException("Database error"));

		// Act & Assert
		await Assert.ThrowsAsync<DbUpdateException>(() => _service.GetPagedAsync(query));
		_repositoryMock.Verify(r => r.GetPagedAsync(
			It.IsAny<int>(),
			It.IsAny<int>(),
			It.IsAny<Expression<Func<GroupBuyingRequest, bool>>>(),
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task CreateAsync_Should_Set_CurrentPeopleCount_To_1_And_Status_Pending()
	{
		// Arrange
		var request = new CreateGroupBuyingRequestDto
		{
			ProductName = "Test Product",
			TargetPeopleCount = 5,
			FullName = "Test User",
			Phone = "0978123456"
		};

		var entity = new GroupBuyingRequest();
		var response = new GroupBuyingRequestResponseDto();

		_mapperMock.Setup(m => m.Map<GroupBuyingRequest>(request)).Returns(entity);
		_mapperMock.Setup(m => m.Map<GroupBuyingRequestResponseDto>(entity)).Returns(response);
		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<GroupBuyingRequest>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		var result = await _service.CreateAsync(request);

		// Assert
		Assert.Equal(1, entity.CurrentPeopleCount);
		Assert.Equal(GroupBuyingStatus.Pending, entity.Status);
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<GroupBuyingRequest>(), It.IsAny<CancellationToken>()), Times.Once);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}