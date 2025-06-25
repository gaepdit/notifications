using Microsoft.EntityFrameworkCore;
using Notifications.Database;

namespace Notifications.API.Tests;

[TestFixture]
public class GetNotificationsTests
{
    private AppDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RepositoryTest")
            .UseAsyncSeeding((context, _, _) => TestData.SeedDataAsync((AppDbContext)context))
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetCurrentNotifications()
    {
        // Arrange
        var expected = TestData.NotificationSeedItems.Where(e =>
            e.Active && e.DisplayStart < DateTime.Now && DateTime.Now < e.DisplayEnd);

        // Act
        var results = await Repository.GetCurrentNotificationsAsync(_dbContext);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetFutureNotifications()
    {
        // Arrange
        var expected = TestData.NotificationSeedItems.Where(e =>
            e.Active && DateTime.Now < e.DisplayStart);

        // Act
        var results = await Repository.GetFutureNotificationsAsync(_dbContext);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetAllNotifications()
    {
        // Act
        var results = await Repository.GetAllNotifications(_dbContext);

        // Assert
        results.Should().BeEquivalentTo(TestData.NotificationSeedItems);
    }
}
