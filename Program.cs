using PipManager.Cli.Commands;
using PipManager.Cli.Commands.Environment;
using PipManager.Core.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PipManager.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (!File.Exists(Configuration.ConfigPath))
        {
            Console.WriteLine($"It seems to be using PipManager.Cli for the first time, the settings file has been created ({Configuration.ConfigPath})");
        }
        
        Configuration.Initialize();
        
        if(args[0] != "env")
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
            });
        });
        
        return await app.RunAsync(args);
    }
}