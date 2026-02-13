using Application.Common.Abstractions;
using Application.Features.Auth.Commands.SwitchOrg;
using Application.Features.Auth.Common;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace IntegrationTests.Features.Auth;

public class SwitchOrgTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public SwitchOrgTests()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldReturnNewToken_WhenSwitchingToValidOrg()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        var user = new SuUser 
        { 
            UserId = userId, 
            Username = "testuser", 
            IsActive = true 
        };
        var org = new SuOrganize 
        { 
            OrgId = orgId, 
            OrgCode = "ORG1", 
            OrgName = "Test Org", 
            IsActive = true 
        };
        var userOrg = new SuUserOrg 
        { 
            UserId = userId, 
            OrgId = orgId, 
            IsActive = true,
            Org = org 
        };
        user.UserOrgs.Add(userOrg);

        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            context.Users.Add(user);
            context.Organizes.Add(org);
            await context.SaveChangesAsync();
        }

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());
        _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("new-jwt-token");

        var command = new SwitchOrgCommand(orgId.ToString());

        // Act
        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            var handler = new SwitchOrgCommandHandler(context, _currentUserServiceMock.Object, _tokenServiceMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("new-jwt-token");
            result.DefaultOrgId.Should().Be(orgId.ToString());
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorized_WhenSwitchingToInvalidOrg()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser 
        { 
            UserId = userId, 
            Username = "testuser", 
            IsActive = true 
        };

        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());
        
        var command = new SwitchOrgCommand(Guid.NewGuid().ToString()); // Random Org ID

        // Act & Assert
        using (var context = new ApplicationDbContext(_options, _currentUserServiceMock.Object))
        {
            var handler = new SwitchOrgCommandHandler(context, _currentUserServiceMock.Object, _tokenServiceMock.Object);
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
