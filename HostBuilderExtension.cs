using System;
using Avalonia;

namespace SystemExplorer.Core.Extensions;

public static class HostBuilderExtension
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    public static IHostBuilder ConfigureAvaloniaAppBuilder<TApplication>(
        this IHostBuilder hostBuilder, 
        Func<AppBuilder> appBuilderResolver, 
        Action<AppBuilder> configureAppBuilder, 
        IHostedLifetime? lifetime = null) 
        where TApplication : Avalonia.Application
    {
        ArgumentNullException.ThrowIfNull(configureAppBuilder);

        hostBuilder.ConfigureServices((ctx, s) => {
            AppBuilder appBuilder = appBuilderResolver();
            configureAppBuilder(appBuilder);

            s.AddSingleton(appBuilder);

            if (appBuilder.Instance is null)
            {
                appBuilder.SetupWithoutStarting();
            }

            s.AddSingleton<Avalonia.Application>((_) => appBuilder.Instance!);
            s.AddSingleton<TApplication>((_) => (TApplication)appBuilder.Instance!);
            s.AddSingleton<IHostedLifetime>(p => lifetime ?? HostedLifetime.Select(p.GetRequiredService<ILoggerFactory>(), p.GetRequiredService<Avalonia.Application>().ApplicationLifetime));
        });

        return hostBuilder;
    }
}
