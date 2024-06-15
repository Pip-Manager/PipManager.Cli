using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using PipManager.Core.Configuration;

namespace PipManager.Cli.Commands.Environment;

[Command("env", Description = "Manage Python environments.")]
public class EnvironmentCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        var selectedEnvironment = Configuration.AppConfig!.SelectedEnvironment;
        var environments = Configuration.AppConfig.Environments;
        switch (environments.Count)
        {
            case 0:
                console.Error.WriteLine("Python environment has not been added yet, add it with the 'env add' command.");
                break;
            default:
                switch (selectedEnvironment)
                {
                    case null:
                        console.Output.WriteLine("No environment selected, select it with the 'env select' command.");
                        break;
                    default:
                        console.Output.WriteLine($"Selected environment: Pip {selectedEnvironment.PipVersion} (Python {selectedEnvironment.PythonVersion})");
                        console.Output.WriteLine($"Located at: {selectedEnvironment.PythonPath}");
                        break;
                }
                break;
        }

        return default;
    }
}