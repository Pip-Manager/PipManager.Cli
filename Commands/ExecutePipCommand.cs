using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace PipManager.Cli.Commands;

[Command]
public class ExecutePipCommand : ICommand
{
    [CommandParameter(0)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required IReadOnlyList<string> Arguments { get; init; }
    
    public ValueTask ExecuteAsync(IConsole console)
    {
        foreach (var argument in Arguments)
        { 
            console.Output.WriteLine($"Executing: {argument}");
        }

        return default;
    }
}