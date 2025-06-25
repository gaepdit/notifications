using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Notifications.Database;
using Notifications.Models;

namespace Notifications.API.Tests;

[TestFixture]
public class AddNotificationTests
{
    private AppDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RepositoryTest")
            // .UseAsyncSeeding((context, _, token) => TestData.SeedDataAsync((AppDbContext)context, token))
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
    public async Task HappyPath_ReturnsSuccess()
    {
        // Arrange
        var resource = new CreateNotificationDto
        {
            Message = "Test message",
            DisplayStart = DateTime.Now.AddDays(-1),
            DisplayEnd = DateTime.Now.AddDays(1),
        };

        // Act
        var result = await Repository.AddNotification(resource, _dbContext);

        // Assert
        using var scope = new AssertionScope();

        result.Should().BeOfType<Ok<Notification>>();

        var notification = ((Ok<Notification>)result).Value;
        notification.Should().BeOfType<Notification>();
        notification.Should().NotBeNull();
        notification.Message.Should().Be(resource.Message);

        var current = await Repository.GetCurrentNotificationsAsync(_dbContext);
        current.Should().Contain(notification);
    }

    [Test]
    public async Task EmptyMessage_ReturnsBadRequest()
    {
        // Arrange
        var resource = new CreateNotificationDto
        {
            Message = "",
            DisplayStart = DateTime.Now.AddDays(-1),
            DisplayEnd = DateTime.Now.AddDays(1),
        };

        // Act
        var result = await Repository.AddNotification(resource, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<List<string>>>();
    }

    [Test]
    public async Task InvalidDates_ReturnsBadRequest()
    {
        // Arrange
        var resource = new CreateNotificationDto
        {
            Message = "Test message",
            DisplayStart = DateTime.Now.AddDays(1),
            DisplayEnd = DateTime.Now.AddDays(-1),
        };

        // Act
        var result = await Repository.AddNotification(resource, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<List<string>>>();
    }

    [Test]
    public async Task EndDateInPast_ReturnsBadRequest()
    {
        // Arrange
        var resource = new CreateNotificationDto
        {
            Message = "Test message",
            DisplayStart = DateTime.Now.AddDays(-2),
            DisplayEnd = DateTime.Now.AddDays(-1),
        };

        // Act
        var result = await Repository.AddNotification(resource, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<List<string>>>();
    }
}
