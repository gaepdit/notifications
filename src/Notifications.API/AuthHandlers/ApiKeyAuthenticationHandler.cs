using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Notifications.Settings;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Notifications.AuthHandlers;

internal static class SchemeType
{
    public const string ApiKey = "ApiKey";
}

internal static class AuthenticationHandlerExtensions
{
    public static void AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(SchemeType.ApiKey)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(SchemeType.ApiKey, null);
        services.AddAuthorizationBuilder().AddPolicy(SchemeType.ApiKey, policy =>
            policy.RequireAuthenticatedUser().AuthenticationSchemes.Add(SchemeType.ApiKey));
    }
}

internal class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    IOptionsSnapshot<AppSettings> appSettings,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string ApiKeyHeaderName = "X-API-Key";
    private List<string> ApiKeys { get; } = appSettings.Value.ApiKeys;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            return Task.FromResult(AuthenticateResult.Fail("API Key header is missing"));

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(providedApiKey))
            return Task.FromResult(AuthenticateResult.Fail("API Key is empty"));

        if (!ApiKeys.Contains(providedApiKey))
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(Scheme.Name)), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
