using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Helpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FlashOffer.API.UnitTests.Services;

public class AuthServiceTests
{
	[Fact]
	public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
	{
		// Arrange
		var admin = new Admin
		{
			Id = Guid.NewGuid(),
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("password123")
		};

		var repoMock = new Mock<IRepository<Admin>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Admin, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<Admin> { admin });
		repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);
		jwtMock.Setup(j => j.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
			.Returns("token123");

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options);

		var request = new LoginRequest { Username = "admin", Password = "password123" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("token123", result.Token);
		Assert.Equal("admin", result.Username);
	}

	[Fact]
	public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
	{
		// Arrange
		var admin = new Admin
		{
			Id = Guid.NewGuid(),
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("password123")
		};

		var repoMock = new Mock<IRepository<Admin>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Admin, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<Admin> { admin });

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options);

		var request = new LoginRequest { Username = "admin", Password = "wrongpassword" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.Null(result);
	}
}
