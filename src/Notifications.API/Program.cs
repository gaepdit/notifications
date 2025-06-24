using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;
using Notifications.AuthHandlers;
using Notifications.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.BindAppSettings();
builder.Services.AddApiKeyAuthentication();

var app = builder.Build();

app.UseRaygun();

// API Endpoints
app.MapGet("/health", () => Results.Ok("OK"));

app.MapGet("/current", (IOptionsSnapshot<AppSettings> data) => data.Value.Notifications
        .Where(n => n.Active && n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd)
        .OrderBy(n => n.DisplayStart).ToArray())
    .RequireAuthorization(SchemeType.ApiKey);

await app.RunAsync();
