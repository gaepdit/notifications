using Notifications.Database;
using Notifications.Platform;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddZLoggerConsole(options => options.UseJsonFormatter());
builder.Services.BindAppSettings().AddApiKeyAuthentication();
builder.ConfigureDatabase();

var app = builder.Build();
app.MapEndpoints();

await app.BuildDatabaseAsync();
await app.RunAsync();
