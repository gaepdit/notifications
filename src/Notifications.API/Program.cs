using Mindscape.Raygun4Net.AspNetCore;
using Notifications.Database;
using Notifications.Platform;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddZLoggerConsole(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UsePlainTextFormatter();
    else
        options.UseJsonFormatter();
});
builder.Services.AddRaygun(builder.Configuration);
builder.Services.BindAppSettings();
builder.Services.AddApiKeyAuthentication();
builder.ConfigureDatabase();

var app = builder.Build();
app.UseRaygun();
app.MapEndpoints();
await app.BuildDatabaseAsync();

await app.RunAsync();
