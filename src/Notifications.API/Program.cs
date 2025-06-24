using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;
using Notifications;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.AddOptions<ApiData>().BindConfiguration(nameof(ApiData));

var app = builder.Build();
app.UseRaygun();

// API Endpoints
app.MapGet("/health", () => Results.Ok("OK"));
app.MapGet("/current", (IOptionsSnapshot<ApiData> data) =>
    data.Value.Notifications
        .Where(n => n.DisplayStart < DateTime.Now && DateTime.Now < n.DisplayEnd && n.Active)
        .OrderBy(n => n.DisplayStart).ToArray());

await app.RunAsync();
