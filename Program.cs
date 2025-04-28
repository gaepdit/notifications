using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;
using Notifications;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.AddOptions<ApiData>().BindConfiguration(nameof(ApiData));

var app = builder.Build();
app.UseRaygun();

// API Endpoints
app.MapGet("/health", () => Results.Ok());
app.MapGet("/current", (IOptionsSnapshot<ApiData> data) =>
    data.Value.Notifications
        .Where(notification => notification.DisplayStart < DateTime.Now && DateTime.Now < notification.DisplayEnd)
        .OrderBy(notification => notification.DisplayStart).ToArray());

await app.RunAsync();
