using Microsoft.EntityFrameworkCore;

namespace Notifications.Database;

internal static class DatabaseExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    public static async Task BuildDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (app.Environment.IsDevelopment())
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await TestData.SeedDataAsync(context);
        }
        else
        {
            await context.Database.MigrateAsync();
        }
    }
}
