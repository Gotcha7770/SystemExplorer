using System;
using Splat;
using CommandLine;
using Avalonia;
using Avalonia.ReactiveUI;
using SystemExplorer.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SystemExplorer;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var parserResult = Parser.Default
            .ParseArguments<CommandLineOptions>(args);

        var shouldExit = parserResult.Tag == ParserResultType.NotParsed;
        if (shouldExit) return;

        // var parsed = (Parsed<CommandLineOptions>)parserResult;
        // if (parsed.Value.IsIncognitoModeEnabled) -> do not use storing

        SubscribeToDomainUnhandledEvents();
        RegisterDependencies();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void SubscribeToDomainUnhandledEvents() =>
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var logger = Locator.Current.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
            var ex = (Exception)args.ExceptionObject;

            logger.LogCritical($"Unhandled application error: {ex}");
        };
    
    private static void RegisterDependencies() =>
       Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);
    
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
