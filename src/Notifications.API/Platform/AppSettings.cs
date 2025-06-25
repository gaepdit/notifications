namespace Notifications.Platform;

internal static class AppSettings
{
    public const string ApiKeys = nameof(ApiKeys);

    public static void BindAppSettings(this IServiceCollection services) =>
        services.AddOptions<List<string>>(ApiKeys).BindConfiguration(configSectionPath: ApiKeys);
}
