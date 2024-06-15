using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using PipManager.Core.Configuration;

namespace PipManager.Cli.Commands;

[Command]
public class ExecutePipCommand : ICommand
{
    [CommandParameter(0)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required IReadOnlyList<string> Arguments { get; init; }
    
    public ValueTask ExecuteAsync(IConsole console)
    {
        if(Configuration.AppConfig!.Environments.Count == 0)
        {
            console.Error.WriteLine("Python environment has not been added yet, add it with the 'env add' command.");
            return default;
        }
        foreach (var argument in Arguments)
        { 
            console.Output.WriteLine($"Executing: {argument}");
        }

        return default;
    }
}