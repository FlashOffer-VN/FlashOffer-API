using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Application.Features.PurchaseRequests.Handlers;
using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FlashOffer.API.UnitTests.Handlers;

public class CreatePurchaseRequestHandlerTests
{
	private readonly Mock<IRepository<PurchaseRequest>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly CreatePurchaseRequestHandler _handler;

	public CreatePurchaseRequestHandlerTests()
	{
		_repositoryMock = new Mock<IRepository<PurchaseRequest>>();
		_mapperMock = new Mock<IMapper>();
		_handler = new CreatePurchaseRequestHandler(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task Handle_Should_Create_PurchaseRequest_Successfully()
	{
		// Arrange
		var command = new CreatePurchaseRequestCommand
		{
			ProductName = "Test Product",
			Quantity = 2,
			ExpectedPrice = 100000,
			FullName = "Test User",
			Phone = "0978123456"
		};

		var entity = new PurchaseRequest();
		_mapperMock.Setup(m => m.Map<PurchaseRequest>(command)).Returns(entity);
		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<PurchaseRequest>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		await _handler.Handle(command, CancellationToken.None);

		// Assert
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<PurchaseRequest>(), It.IsAny<CancellationToken>()), Times.Once);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenSaveFails_ShouldThrowException()
	{
		// Arrange
		var command = new CreatePurchaseRequestCommand
		{
			ProductName = "Test Product",
			Quantity = 2,
			ExpectedPrice = 100000,
			FullName = "Test User",
			Phone = "0978123456"
		};

		var entity = new PurchaseRequest();
		_mapperMock.Setup(m => m.Map<PurchaseRequest>(command)).Returns(entity);
		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<PurchaseRequest>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(new DbUpdateException("Database error"));

		// Act & Assert
		await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(command, CancellationToken.None));
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<PurchaseRequest>(), It.IsAny<CancellationToken>()), Times.Once);
	}
}