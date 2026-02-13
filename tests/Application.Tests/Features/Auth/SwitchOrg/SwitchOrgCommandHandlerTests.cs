using Application.Common.Abstractions;
using Application.Features.Auth.Commands.SwitchOrg;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Features.Auth.SwitchOrg;

public class SwitchOrgCommandHandlerTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public SwitchOrgCommandHandlerTests()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenUserIsNotInOrg()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser { UserId = userId, Username = "testuser", IsActive = true };
        
        // Org exists but user is not in it
        var org = new SuOrganize { OrgId = Guid.NewGuid(), OrgName = "Target Org", IsActive = true };

        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            context.Users.Add(user);
            context.Organizes.Add(org);
            await context.SaveChangesAsync();
        }

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());
        
        var command = new SwitchOrgCommand(org.OrgId.ToString());

        // Act & Assert
        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            var handler = new SwitchOrgCommandHandler(context, _currentUserServiceMock.Object, _tokenServiceMock.Object);
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("User does not have access to this organization");
        }
    }
}
