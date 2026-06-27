using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FlashOffer.API.UnitTests.Services;

public class AuthServiceTests
{
	private readonly Mock<ILogger<AuthService>> _loggerMock;

	public AuthServiceTests()
	{
		_loggerMock = new Mock<ILogger<AuthService>>();
	}

	[Fact]
	public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("password123"),
			FullName = "Administrator",
			Email = "admin@test.com",
			IsActive = true,
			Role = UserRole.Admin
		};

		var repoMock = new Mock<IRepository<User>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<User> { user });
		repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);
		jwtMock.Setup(j => j.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
			.Returns("token123");

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options, _loggerMock.Object);

		var request = new LoginRequest { Username = "admin", Password = "password123" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("token123", result.Token);
		Assert.Equal("admin", result.Username);
		Assert.Equal("Administrator", result.FullName);
		Assert.Equal("Admin", result.Role);
	}

	[Fact]
	public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Username = "admin",
			PasswordHash = PasswordHasher.Hash("password123"),
			FullName = "Administrator",
			Email = "admin@test.com",
			IsActive = true,
			Role = UserRole.Admin
		};

		var repoMock = new Mock<IRepository<User>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<User> { user });

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options, _loggerMock.Object);

		var request = new LoginRequest { Username = "admin", Password = "wrongpassword" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task LoginAsync_WithInactiveUser_ReturnsNull()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Username = "inactive",
			PasswordHash = PasswordHasher.Hash("password123"),
			FullName = "Inactive User",
			Email = "inactive@test.com",
			IsActive = false,
			Role = UserRole.Customer
		};

		var repoMock = new Mock<IRepository<User>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<User> { user });

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options, _loggerMock.Object);

		var request = new LoginRequest { Username = "inactive", Password = "password123" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task LoginAsync_WithNonExistentUser_ReturnsNull()
	{
		// Arrange
		var repoMock = new Mock<IRepository<User>>();
		repoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<System.Threading.CancellationToken>()))
			.ReturnsAsync(new List<User>());

		var jwtMock = new Mock<Shared.Common.Interfaces.IJwtService>(MockBehavior.Strict);

		var options = Options.Create(new JwtSettings { ExpiryMinutes = 60 });

		var service = new AuthService(repoMock.Object, jwtMock.Object, options, _loggerMock.Object);

		var request = new LoginRequest { Username = "nonexistent", Password = "password123" };

		// Act
		var result = await service.LoginAsync(request);

		// Assert
		Assert.Null(result);
	}
}