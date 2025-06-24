using Notifications.Models;

namespace Notifications.Settings;

internal class AppSettings
{
    public List<Notification> Notifications { get; } = [];
}

internal static class AppSettingsExtensions
{
    public static void BindAppSettings(this IServiceCollection services)
    {
        services.AddOptions<AppSettings>().BindConfiguration(configSectionPath: nameof(AppSettings));
    }
}
