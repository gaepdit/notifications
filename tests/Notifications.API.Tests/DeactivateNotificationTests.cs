using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Notifications.Database;
using Notifications.Models;

namespace Notifications.API.Tests;

[TestFixture]
public class DeactivateNotificationTests
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
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task HappyPath_ReturnsSuccess()
    {
        // Arrange
        var create = new CreateNotificationDto
        {
            Message = "Test message",
            DisplayStart = DateTime.Now.AddDays(-1),
            DisplayEnd = DateTime.Now.AddDays(1),
        };
        var newNotification = (await Repository.AddNotification(create, _dbContext) as Ok<Notification>)!.Value;
        var request = new DeactivateNotificationDto { Id = newNotification!.Id };

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
    public async Task MissingId_ReturnsNotFound()
    {
        // Arrange
        var request = new DeactivateNotificationDto { Id = Guid.NewGuid() };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<NotFound<string>>();
    }

    [Test]
    public async Task InactiveNotification_ReturnsBadRequest()
    {
        // Arrange
        var request = new DeactivateNotificationDto { Id = TestData.InactiveNotificationId };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<List<string>>>();
    }

    [Test]
    public async Task ExpiredNotification_ReturnsBadRequest()
    {
        // Arrange
        var request = new DeactivateNotificationDto { Id = TestData.ExpiredNotificationId };

        // Act
        var result = await Repository.DeactivateNotification(request, _dbContext);

        // Assert
        result.Should().BeOfType<BadRequest<List<string>>>();
    }
}
