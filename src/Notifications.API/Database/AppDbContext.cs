using Microsoft.EntityFrameworkCore;
using Notifications.Models;

namespace Notifications.Database;

internal static class AppDbContextExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

            if (builder.Environment.IsDevelopment())
            {
                options.UseAsyncSeeding((context, _, token) => TestData.SeedDataAsync((AppDbContext)context, token));
            }
        });
    }

    public static async Task BuildDatabaseInDevAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Notification>().HasKey(e => e.Id);
}
