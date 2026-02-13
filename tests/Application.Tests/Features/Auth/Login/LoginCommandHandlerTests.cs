using Application.Common.Abstractions;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Common;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Features.Auth.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public LoginCommandHandlerTests()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenPasswordIsWrong()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser { UserId = userId, Username = "wrongpwd", IsActive = true, PasswordHash = "hash" };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<SuUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        var command = new LoginCommand("wrongpwd", "wrong");

        // Act & Assert
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new LoginCommandHandler(context, _passwordHasherMock.Object, _tokenServiceMock.Object);
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials");
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenUserIsInactive()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser { UserId = userId, Username = "inactive", IsActive = false, PasswordHash = "hash" };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        // Even with correct password
        _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<SuUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var command = new LoginCommand("inactive", "password");

        // Act & Assert
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new LoginCommandHandler(context, _passwordHasherMock.Object, _tokenServiceMock.Object);
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("User is inactive"); // Expect specific message now
        }
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenUserIsLocked()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser { 
            UserId = userId, 
            Username = "locked", 
            IsActive = true, 
            IsLocked = true,
            LockoutEndAt = DateTimeOffset.UtcNow.AddMinutes(10),
            PasswordHash = "hash" 
        };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        // Even with correct password
        _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<SuUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var command = new LoginCommand("locked", "password");

        // Act & Assert
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new LoginCommandHandler(context, _passwordHasherMock.Object, _tokenServiceMock.Object);
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("User is locked");
        }
    }
}
