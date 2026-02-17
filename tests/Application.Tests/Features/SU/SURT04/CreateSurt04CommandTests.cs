using Application.Common.Abstractions;
using Application.Features.SU.SURT04.Commands;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Features.SU.SURT04;

public class CreateSurt04CommandTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public CreateSurt04CommandTests()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenValidRequest()
    {
        // Arrange
        var command = new CreateSurt04Command
        {
            Username = "newuser",
            Password = "Password123!",
            Email = "newuser@example.com",
            IsActive = true,
            ProfileIds = new List<Guid>()
        };

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<SuUser>(), It.IsAny<string>()))
            .Returns("hashed_password");

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new CreateSurt04CommandHandler(context, _passwordHasherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            user.Should().NotBeNull();
            user!.Email.Should().Be("newuser@example.com");
            user.PasswordHash.Should().Be("hashed_password");
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUsernameExists()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            // Arrange
            context.Users.Add(new SuUser 
            { 
                UserId = Guid.NewGuid(),
                Username = "existinguser", 
                Email = "old@example.com", 
                PasswordHash = "hash",
                SecurityStamp = "stamp",
                IsActive = true
            });
            await context.SaveChangesAsync();

            var command = new CreateSurt04Command
            {
                Username = "existinguser",
                Password = "Password123!",
                Email = "new@example.com"
            };
            
            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<SuUser>(), It.IsAny<string>()))
                .Returns("hashed_password");

            // Act & Assert
            var handler = new CreateSurt04CommandHandler(context, _passwordHasherMock.Object);

            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<Exception>() // Or specific validation exception if you have one
                .WithMessage("*already exists*");
        }
    }
    [Fact]
    public async Task Handle_ShouldCreateUserWithOrg_WhenOrgProvided()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Organizes.Add(new SuOrganize 
            { 
                OrgId = orgId, 
                OrgCode = "ORG01", 
                OrgName = "Test Org" 
            });
            await context.SaveChangesAsync();
        }

        var command = new CreateSurt04Command
        {
            Username = "userwithorg",
            Password = "Password123!",
            Email = "org@example.com",
            IsActive = true,
            UserOrgs = new List<UserOrgInput>
            {
                new UserOrgInput { OrgId = orgId, IsDefault = true }
            }
        };

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<SuUser>(), It.IsAny<string>()))
            .Returns("hashed_password");

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new CreateSurt04CommandHandler(context, _passwordHasherMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var user = await context.Users
                .Include(u => u.UserOrgs)
                .FirstOrDefaultAsync(u => u.UserId == result);

            user.Should().NotBeNull();
            user!.UserOrgs.Should().HaveCount(1);
            user.UserOrgs.First().OrgId.Should().Be(orgId);
            user.UserOrgs.First().IsDefault.Should().BeTrue();
        }
    }
}
