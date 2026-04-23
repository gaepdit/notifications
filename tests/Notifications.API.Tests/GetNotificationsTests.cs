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
            .UseAsyncSeeding((context, _, _) => TestData.SeedDataAsync(context))
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
