using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Features.OfferRequests.Commands;
using FlashOffer.API.Application.Features.OfferRequests.Handlers;
using FlashOffer.API.Domain.Entities;
using Moq;

namespace FlashOffer.API.UnitTests.Handlers;

public class CreateOfferRequestHandlerTests
{
	private readonly Mock<IRepository<OfferRequest>> _repositoryMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly CreateOfferRequestHandler _handler;

	public CreateOfferRequestHandlerTests()
	{
		_repositoryMock = new Mock<IRepository<OfferRequest>>();
		_mapperMock = new Mock<IMapper>();
		_handler = new CreateOfferRequestHandler(_repositoryMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async Task Handle_Should_Create_Request_With_IsOfferSent_False()
	{
		// Arrange
		var command = new CreateOfferRequestCommand
		{
			SelectedOffer = "Test Offer",
			FullName = "Test User",
			Phone = "0978123456",
			Zalo = "testzalo"
		};

		var entity = new OfferRequest();
		_mapperMock.Setup(m => m.Map<OfferRequest>(command)).Returns(entity);
		_repositoryMock.Setup(r => r.AddAsync(It.IsAny<OfferRequest>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);
		_repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		await _handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.False(entity.IsOfferSent);
		_repositoryMock.Verify(r => r.AddAsync(It.IsAny<OfferRequest>(), It.IsAny<CancellationToken>()), Times.Once);
		_repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}