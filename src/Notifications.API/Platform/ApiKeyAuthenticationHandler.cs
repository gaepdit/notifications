using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Notifications.Platform;

internal static class AuthenticationHandlerExtensions
{
    public static void AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(AppSettings.ApiKeys)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AppSettings.ApiKeys, null);
        services.AddAuthorizationBuilder().AddPolicy(AppSettings.ApiKeys, policy =>
            policy.RequireAuthenticatedUser().AuthenticationSchemes.Add(AppSettings.ApiKeys));
    }
}

internal class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    IOptionsSnapshot<List<string>> appSettings,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string ApiKeyHeaderName = "X-API-Key";
    private List<string> ApiKeys { get; } = appSettings.Get(AppSettings.ApiKeys);

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
