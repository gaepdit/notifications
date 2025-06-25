using Microsoft.EntityFrameworkCore;
using Notifications.Models;

namespace Notifications.Database;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Notification>().HasKey(e => e.Id);
}
