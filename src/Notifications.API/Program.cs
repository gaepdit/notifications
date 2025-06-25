using Mindscape.Raygun4Net.AspNetCore;
using Notifications.Database;
using Notifications.Platform;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.BindAppSettings();
builder.Services.AddApiKeyAuthentication();
builder.ConfigureDatabase();

var app = builder.Build();
app.UseRaygun();
app.MapEndpoints();

if (app.Environment.IsDevelopment()) await app.BuildDatabaseInDevAsync();

await app.RunAsync();
