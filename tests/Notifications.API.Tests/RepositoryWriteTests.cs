using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notifications.Database;
using Notifications.Models;

namespace Notifications.API.Tests;

[TestFixture]
public class RepositoryWriteTests
{
    private AppDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RepositoryTest")
            .UseAsyncSeeding((context, _, token) => TestData.SeedDataAsync((AppDbContext)context, token))
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
    public async Task AddNotification_ReturnsSuccess()
    {
        // Arrange
        var resource = new CreateNotification
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
    public async Task DeactivateNotification_ReturnsSuccess()
    {
        // Arrange
        var create = new CreateNotification
        {
            Message = "Test message",
            DisplayStart = DateTime.Now.AddDays(-1),
            DisplayEnd = DateTime.Now.AddDays(1),
        };
        var newNotification = (await Repository.AddNotification(create, _dbContext) as Ok<Notification>)!.Value;
        var request = new NotificationRequest { Id = newNotification!.Id };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        using var scope = new AssertionScope();

        result.Should().BeOfType<Ok<Notification>>();

        var notification = ((Ok<Notification>)result).Value;
        notification.Should().BeOfType<Notification>();
        notification.Should().NotBeNull();
        notification.Should().Be(newNotification with { Active = false });
    }

    [Test]
    public async Task DeactivateNotification_MissingId_ReturnsNotFound()
    {
        // Arrange
        var request = new NotificationRequest { Id = Guid.NewGuid() };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<NotFound<string>>();
    }

    [Test]
    public async Task DeactivateNotification_InactiveNotification_ReturnsBadRequest()
    {
        // Arrange
        var request = new NotificationRequest { Id = TestData.InactiveNotificationId };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
    }

    [Test]
    public async Task DeactivateNotification_ExpiredNotification_ReturnsBadRequest()
    {
        // Arrange
        var request = new NotificationRequest { Id = TestData.ExpiredNotificationId };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
    }
}
