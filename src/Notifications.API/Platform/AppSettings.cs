using System.Reflection;

namespace Notifications.Platform;

internal static class AppSettings
{
    public static string? Version { get; private set; }
    public const string ApiKeys = nameof(ApiKeys);

    public static void BindAppSettings(this IServiceCollection services)
    {
        Version = GetVersion();
        services.AddOptions<List<string>>(ApiKeys).BindConfiguration(configSectionPath: ApiKeys);
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
