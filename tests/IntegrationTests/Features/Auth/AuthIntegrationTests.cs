using Application.Common.Abstractions;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.SwitchOrg;
using Application.Features.Auth.Common;
using Domain.Entities.SU;
using FluentAssertions;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace IntegrationTests.Features.Auth;

public class AuthIntegrationTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public AuthIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private ITokenService CreateTokenService()
    {
        var jwtSettings = new JwtSettings 
        { 
            Key = "SuperSecretKey12345678901234567890", 
            Issuer = "TestIssuer", 
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        
        var optionsMock = new Mock<IOptions<JwtSettings>>();
        optionsMock.Setup(o => o.Value).Returns(jwtSettings);
        
        return new JwtTokenService(optionsMock.Object);
    }
    
    private IPasswordHasher CreatePasswordHasher()
    {
        return new PasswordHasher();
    }

    [Fact]
    public async Task ShouldReturnUserAndDefaultOrg_OnLogin()
    {
        // 1. Arrange
        var userId = Guid.NewGuid();
        var defaultOrgId = Guid.NewGuid();
        var secondOrgId = Guid.NewGuid();
        
        var user = new SuUser 
        { 
            UserId = userId, 
            Username = "integration_user", 
            IsActive = true 
        };
        var hasher = CreatePasswordHasher();
        user.PasswordHash = hasher.HashPassword(user, "password123");

        var org1 = new SuOrganize { OrgId = defaultOrgId, OrgName = "Default Org", IsActive = true };
        var org2 = new SuOrganize { OrgId = secondOrgId, OrgName = "Second Org", IsActive = true };

        user.UserOrgs.Add(new SuUserOrg { UserId = userId, OrgId = defaultOrgId, Org = org1, IsActive = true, IsDefault = true });
        user.UserOrgs.Add(new SuUserOrg { UserId = userId, OrgId = secondOrgId, Org = org2, IsActive = true, IsDefault = false });

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            context.Organizes.AddRange(org1, org2);
            await context.SaveChangesAsync();
        }

        // 2. Act (Login)
        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            var handler = new LoginCommandHandler(context, hasher, CreateTokenService());
            var result = await handler.Handle(new LoginCommand("integration_user", "password123"), CancellationToken.None);

            // 3. Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.DefaultOrgId.Should().Be(defaultOrgId.ToString()); // Default org
            result.Orgs.Should().HaveCount(2);
        }
    }

    [Fact]
    public async Task ShouldUpdateCurrentOrg_OnSwitchOrg()
    {
        // 1. Arrange
        var userId = Guid.NewGuid();
        var defaultOrgId = Guid.NewGuid();
        var targetOrgId = Guid.NewGuid();
        
        var user = new SuUser { UserId = userId, Username = "switcher", IsActive = true };
        var org1 = new SuOrganize { OrgId = defaultOrgId, OrgName = "Org 1", IsActive = true };
        var org2 = new SuOrganize { OrgId = targetOrgId, OrgName = "Org 2", IsActive = true };

        user.UserOrgs.Add(new SuUserOrg { UserId = userId, OrgId = defaultOrgId, Org = org1, IsActive = true, IsDefault = true });
        user.UserOrgs.Add(new SuUserOrg { UserId = userId, OrgId = targetOrgId, Org = org2, IsActive = true });

        using (var context = new ApplicationDbContext(_options, new Mock<ICurrentUserService>().Object))
        {
            context.Users.Add(user);
            context.Organizes.AddRange(org1, org2);
            await context.SaveChangesAsync();
        }

        // 2. Act (Switch Org)
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        currentUserServiceMock.Setup(x => x.UserId).Returns(userId.ToString());
        
        using (var context = new ApplicationDbContext(_options, currentUserServiceMock.Object))
        {
            var handler = new SwitchOrgCommandHandler(context, currentUserServiceMock.Object, CreateTokenService());
            var result = await handler.Handle(new SwitchOrgCommand(targetOrgId.ToString()), CancellationToken.None);

            // 3. Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.DefaultOrgId.Should().Be(targetOrgId.ToString()); // The returned "DefaultOrgId" in response is actually the switched generic org id field
            // In a real full integration test, we would parse the token to verify ClaimTypes.OrgId matches targetOrgId
        }
    }
}
