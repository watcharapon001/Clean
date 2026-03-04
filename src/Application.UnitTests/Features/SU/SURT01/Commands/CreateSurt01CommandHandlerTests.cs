using Application.Common.Abstractions;
using Application.Features.SU.SURT01.Commands;
using Domain.Entities.SU;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Features.SU.SURT01.Commands;

public class CreateSurt01CommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateProfile_AndReturnGuid()
    {
        // Arrange
        var command = new CreateSurt01Command
        {
            ProfileCode = "TEST-01",
            ProfileName = "Test Profile",
            Description = "A test profile",
            IsActive = true
        };

        var mockContext = new Mock<IApplicationDbContext>();
        
        // Mocking the Profiles DbSet
        var profilesData = new List<SuProfile>().AsQueryable();
        var mockDbSet = new Mock<DbSet<SuProfile>>();
        
        mockDbSet.As<IQueryable<SuProfile>>().Setup(m => m.Provider).Returns(profilesData.Provider);
        mockDbSet.As<IQueryable<SuProfile>>().Setup(m => m.Expression).Returns(profilesData.Expression);
        mockDbSet.As<IQueryable<SuProfile>>().Setup(m => m.ElementType).Returns(profilesData.ElementType);
        mockDbSet.As<IQueryable<SuProfile>>().Setup(m => m.GetEnumerator()).Returns(profilesData.GetEnumerator());

        mockContext.Setup(c => c.Profiles).Returns(mockDbSet.Object);

        var handler = new CreateSurt01CommandHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        
        mockDbSet.Verify(m => m.Add(It.Is<SuProfile>(p => 
            p.ProfileCode == "TEST-01" && 
            p.ProfileName == "Test Profile")), Times.Once());
            
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
