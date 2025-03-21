using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ApiData>().BindConfiguration(nameof(ApiData));
var app = builder.Build();

// API Endpoints
app.MapGet("/current", (IOptionsSnapshot<ApiData> data) =>
    data.Value.Notifications
        .Where(notification => notification.DisplayStart < DateTime.Now && DateTime.Now < notification.DisplayEnd)
        .OrderBy(notification => notification.DisplayStart).ToArray());

await app.RunAsync();

// API classes
internal record Notification
{
    public required string Message { get; init; }
    public DateTime DisplayStart { get; init; }
    public DateTime DisplayEnd { get; init; }
}

internal record ApiData
{
    public List<Notification> Notifications { get; init; } = [];
}
