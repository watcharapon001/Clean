using Application.Common.Abstractions;
using Application.Features.SU.SURT04.Commands;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Features.SU.SURT04;

public class DeleteSurt04CommandTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public DeleteSurt04CommandTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser 
        { 
            UserId = userId, 
            Username = "todelete",
            PasswordHash = "hash",
            SecurityStamp = "stamp",
            IsActive = true
        };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var command = new DeleteSurt04Command(userId); // Ensure this constructor exists or adjust property

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new DeleteSurt04CommandHandler(context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedUser = await context.Users.FindAsync(userId);
            deletedUser.Should().BeNull();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var command = new DeleteSurt04Command(Guid.NewGuid());

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new DeleteSurt04CommandHandler(context);

            // Act & Assert
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Exception>(); // Or NotFoundException if you use one
        }
    }
}
