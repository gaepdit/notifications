using Microsoft.Extensions.Options;
using notifications;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ApiData>().BindConfiguration(nameof(ApiData));
var app = builder.Build();

// Endpoints
app.MapGet("/", (IOptionsSnapshot<ApiData> data) =>
    GetCurrentNotifications(data.Value.Notifications));
app.MapGet("/upcoming", (IOptionsSnapshot<ApiData> data) =>
    GetUpcomingNotifications(data.Value.Notifications));

await app.RunAsync();
return;

// API functions
Notification[] GetCurrentNotifications(List<Notification> list) => list
    .Where(n => n.DisplayStart < DateTime.Now && n.DisplayEnd > DateTime.Now)
    .OrderBy(n => n.DisplayStart).ToArray();

Notification[] GetUpcomingNotifications(List<Notification> list) => list
    .Where(n => n.DisplayEnd > DateTime.Now)
    .OrderBy(n => n.DisplayStart).ToArray();
