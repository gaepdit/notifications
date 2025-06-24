using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;
using Notifications.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.BindAppSettings();

var app = builder.Build();
app.UseRaygun();

// API Endpoints
app.MapGet("/health", () => Results.Ok("OK"));
app.MapGet("/current", (IOptionsSnapshot<AppSettings> data) =>
    data.Value.Notifications
        .Where(n => n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd && n.Active)
        .OrderBy(n => n.DisplayStart).ToArray());

await app.RunAsync();
