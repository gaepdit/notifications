using Mindscape.Raygun4Net.AspNetCore;
using Notifications.AuthHandlers;
using Notifications.Endpoints;
using Notifications.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRaygun(builder.Configuration);
builder.Services.BindAppSettings();
builder.Services.AddApiKeyAuthentication();

var app = builder.Build();
app.UseRaygun();
app.MapEndpoints();

await app.RunAsync();
