using Application.Common.Abstractions;
using Domain.Entities.DB;
using Domain.Entities.SU;
using Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests;

public class GlobalFilterTests
{
    [Fact]
    public async Task Query_ShouldFilterByOrgId()
    {
        // 1. Setup
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var currentUserService = new FakeCurrentUserService();
        
        // 2. Seed Data
        // We need SuOrganize because DbEmployee requires it (FK)? 
        // InMemory doesn't enforce FK usually unless configured, but better to be safe or minimal.
        // DbEmployee has Org property required? 
        // Let's check DbEmployee.cs again. It has `public SuOrganize Org { get; set; } = null!;`
        // But in InMemory we can just set IDs if we don't traverse navigation or if we seed carefully.
        
        var org1Id = Guid.NewGuid();
        var org2Id = Guid.NewGuid();

        using (var context = new ApplicationDbContext(options, currentUserService))
        {
            context.Database.EnsureCreated();

            context.Set<DbEmployee>().Add(new DbEmployee 
            { 
                EmployeeId = Guid.NewGuid(), 
                OrgId = org1Id, 
                FirstName = "Emp1",
                // Validations?
            });

            context.Set<DbEmployee>().Add(new DbEmployee 
            { 
                EmployeeId = Guid.NewGuid(), 
                OrgId = org2Id, 
                FirstName = "Emp2" 
            });

            await context.SaveChangesAsync();
        }

        // 3. Test Org1
        currentUserService.OrgId = org1Id.ToString();
        using (var context = new ApplicationDbContext(options, currentUserService))
        {
            var employees = await context.Set<DbEmployee>().ToListAsync();
            Assert.Single(employees);
            Assert.Equal("Emp1", employees.First().FirstName);
        }

        // 4. Test Org2
        currentUserService.OrgId = org2Id.ToString();
        using (var context = new ApplicationDbContext(options, currentUserService))
        {
            var employees = await context.Set<DbEmployee>().ToListAsync();
            Assert.Single(employees);
            Assert.Equal("Emp2", employees.First().FirstName);
        }
    }

    public class FakeCurrentUserService : ICurrentUserService
    {
        public string? UserId => "test-user";
        public string? OrgId { get; set; }
    }
}
