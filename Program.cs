using System.Diagnostics.CodeAnalysis;
using CliFx;
using PipManager.Cli.Commands;
using PipManager.Cli.Commands.Environment;
using PipManager.Core.Configuration;

namespace PipManager.Cli;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(EnvironmentCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ExecutePipCommand))]
    public static async Task<int> Main(string[] args)
    {
        if (!File.Exists(Configuration.ConfigPath))
        {
            Console.WriteLine($"It seems to be using PipManager.Cli for the first time, the settings file has been created ({Configuration.ConfigPath})");
        }
        
        Configuration.Initialize();
        
        return await new CliApplicationBuilder()
            .AddCommand<EnvironmentCommand>()
            .AddCommand<EnvironmentAddCommand>()
            .AddCommand<ExecutePipCommand>()
            .SetExecutableName("PipManager.Cli")
            .Build()
            .RunAsync(args);
    }
}