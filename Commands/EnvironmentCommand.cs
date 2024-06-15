using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace PipManager.Cli.Commands;

[Command("env", Description = "Manage Pip environments.")]
public class EnvironmentCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        console.Output.WriteLine("Environment command executed.");

        return default;
    }
}