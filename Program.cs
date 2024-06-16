using System.Diagnostics.CodeAnalysis;
using PipManager.Cli.Commands;
using PipManager.Cli.Commands.Environment;
using PipManager.Core.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PipManager.Cli;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicNestedTypes, typeof(EnvironmentCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicNestedTypes, typeof(EnvironmentAddCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicNestedTypes, typeof(EnvironmentListCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicNestedTypes, typeof(EnvironmentRemoveCommand))]
    public static async Task<int> Main(string[] args)
    {
        if (!File.Exists(Configuration.ConfigPath))
        {
            Console.WriteLine($"It seems to be using PipManager.Cli for the first time, the settings file has been created ({Configuration.ConfigPath})");
        }
        
        Configuration.Initialize();
        
        if(args.Length > 0 && args[0] != "env")
        {
            ExecutePipCommand.Start(args);
            return 0;
        }
        
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddBranch<EnvSettings>("env", env =>
            {
                env.AddCommand<EnvironmentAddCommand>("add");
                env.AddCommand<EnvironmentListCommand>("list");
                env.AddCommand<EnvironmentRemoveCommand>("remove");
                env.AddCommand<EnvironmentInfoCommand>("info");
                env.AddCommand<EnvironmentSwitchCommand>("switch");
            });

            config.SetExceptionHandler((ex, _) =>
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace | ExceptionFormats.ShortenTypes);
                return 1;
            });
        });
        return await app.RunAsync(args);
    }
}