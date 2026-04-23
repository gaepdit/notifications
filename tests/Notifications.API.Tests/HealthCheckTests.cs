using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Notifications.Database;

namespace Notifications.API.Tests;

[TestFixture]
public class HealthCheckTests
{
    private AppDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RepositoryTest")
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }

    [Test]
    public async Task DbAvailable_IfAvailable()
    {
        // Act
        var results = await Repository.DbAvailable(_dbContext);

        // Assert
        results.Should().BeOfType<Ok<string>>();
    }
}
