using Application.Common.Abstractions;
using Application.Features.SU.SURT04.Commands;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Features.SU.SURT04;

public class UpdateSurt04CommandTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public UpdateSurt04CommandTests()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser 
        { 
            UserId = userId, 
            Username = "oldname", 
            Email = "old@example.com",
            PasswordHash = "hash",
            SecurityStamp = "stamp",
            IsActive = true
        };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var command = new UpdateSurt04Command
        {
            UserId = userId,
            Username = "newname",
            Email = "new@example.com",
            IsActive = true,
            ProfileIds = new List<Guid>()
        };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new UpdateSurt04CommandHandler(context, _passwordHasherMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedUser = await context.Users.FindAsync(userId);
            updatedUser.Should().NotBeNull();
            updatedUser!.Username.Should().Be("newname");
            updatedUser.Email.Should().Be("new@example.com");
        }
    }

    [Fact]
    public async Task Handle_ShouldUpdatePassword_WhenPasswordProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new SuUser { UserId = userId, Username = "user", PasswordHash = "oldhash" };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var command = new UpdateSurt04Command
        {
            UserId = userId,
            Username = "user",
            Email = "user@example.com",
            Password = "NewPassword123!",
            ProfileIds = new List<Guid>()
        };

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<SuUser>(), It.IsAny<string>()))
            .Returns("newhash");

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new UpdateSurt04CommandHandler(context, _passwordHasherMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedUser = await context.Users.FindAsync(userId);
            updatedUser!.PasswordHash.Should().Be("newhash");
        }
    }
    [Fact]
    public async Task Handle_ShouldUpdateUserOrgs_WhenValidRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var orgId1 = Guid.NewGuid();
        var orgId2 = Guid.NewGuid();

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Organizes.AddRange(
                new SuOrganize { OrgId = orgId1, OrgCode = "ORG01", OrgName = "Org 1" },
                new SuOrganize { OrgId = orgId2, OrgCode = "ORG02", OrgName = "Org 2" }
            );

            var user = new SuUser 
            { 
                UserId = userId, 
                Username = "user", 
                PasswordHash = "hash",
                SecurityStamp = "stamp",
                IsActive = true
            };
            
            // Initially assigned to Org1 (Default)
            user.UserOrgs.Add(new SuUserOrg { UserId = userId, OrgId = orgId1, IsDefault = true });
            
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var command = new UpdateSurt04Command
        {
            UserId = userId,
            Username = "user",
            Email = "user@example.com",
            IsActive = true,
            UserOrgs = new List<UserOrgInput>
            {
                // Remove Org1, Add Org2 (Default)
                new UserOrgInput { OrgId = orgId2, IsDefault = true }
            }
        };

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new UpdateSurt04CommandHandler(context, _passwordHasherMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedUser = await context.Users
                .Include(u => u.UserOrgs)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            updatedUser!.UserOrgs.Should().HaveCount(1);
            updatedUser.UserOrgs.First().OrgId.Should().Be(orgId2);
            updatedUser.UserOrgs.First().IsDefault.Should().BeTrue();
        }
    }
}
