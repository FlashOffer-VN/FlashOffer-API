using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Moq;

namespace FlashOffer.API.UnitTests.Services;

public class CreateGroupBuyingRequestValidatorTests
{
	private readonly Mock<IRepository<GroupBuyingRequest>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly GroupBuyingRequestService _service;

	public CreateGroupBuyingRequestValidatorTests()
	{
		_repositoryMock = new Mock<IRepository<GroupBuyingRequest>>();
		_mapperMock = new Mock<IMapper>();
		_service = new GroupBuyingRequestService(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task CreateAsync_Should_Set_CurrentPeopleCount_To_1()
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