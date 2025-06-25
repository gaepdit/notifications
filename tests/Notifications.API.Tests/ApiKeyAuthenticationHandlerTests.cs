using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notifications.Platform;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Notifications.API.Tests;

[TestFixture]
public class ApiKeyAuthenticationHandlerTests
{
    private ApiKeyAuthenticationHandler _sut;
    private ILoggerFactory _loggerFactory;
    private HttpContext _httpContext;
    private const string ValidApiKey = "valid-api-key";
    private const string TestScheme = "TestApiKeyScheme";

    [SetUp]
    public void Setup()
    {
        // Set up mocks
        var options = Substitute.For<IOptionsMonitor<AuthenticationSchemeOptions>>();
        options.Get(TestScheme).Returns(new AuthenticationSchemeOptions());

        var apiKeys = Substitute.For<IOptionsSnapshot<List<string>>>();
        apiKeys.Get(AppSettings.ApiKeys).Returns([ValidApiKey]);

        _loggerFactory = Substitute.For<ILoggerFactory>();
        var urlEncoder = Substitute.For<UrlEncoder>();

        // Create HTTP context
        _httpContext = new DefaultHttpContext();

        // Create handler
        _sut = new ApiKeyAuthenticationHandler(options, apiKeys, _loggerFactory, urlEncoder);

        // Initialize handler with HTTP context
        var scheme = new AuthenticationScheme(TestScheme, TestScheme, typeof(ApiKeyAuthenticationHandler));
        _sut.InitializeAsync(scheme, _httpContext).GetAwaiter().GetResult();
    }

    [TearDown]
    public void Cleanup()
    {
        _loggerFactory.Dispose();
    }

    [Test]
    public async Task HandleAuthenticateAsync_WithMissingApiKey_ReturnsFailure()
    {
        // Act
        var result = await _sut.AuthenticateAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Be("API Key header is missing");
    }

    [Test]
    public async Task HandleAuthenticateAsync_WithEmptyApiKey_ReturnsFailure()
    {
        // Arrange
        _httpContext.Request.Headers[ApiKeyAuthenticationHandler.ApiKeyHeaderName] = string.Empty;

        // Act
        var result = await _sut.AuthenticateAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Be("API Key is empty");
    }

    [Test]
    public async Task HandleAuthenticateAsync_WithInvalidApiKey_ReturnsFailure()
    {
        // Arrange
        _httpContext.Request.Headers[ApiKeyAuthenticationHandler.ApiKeyHeaderName] = "invalid-key";

        // Act
        var result = await _sut.AuthenticateAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Succeeded.Should().BeFalse();
        result.Failure!.Message.Should().Be("Invalid API Key");
    }

    [Test]
    public async Task HandleAuthenticateAsync_WithValidApiKey_ReturnsSuccess()
    {
        // Arrange
        _httpContext.Request.Headers[ApiKeyAuthenticationHandler.ApiKeyHeaderName] = ValidApiKey;

        // Act
        var result = await _sut.AuthenticateAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Succeeded.Should().BeTrue();
        var identity = result.Principal?.Identity as ClaimsIdentity;
        identity.Should().NotBeNull();
        Debug.Assert(identity != null);
        identity.AuthenticationType.Should().Be(TestScheme);
    }
}
