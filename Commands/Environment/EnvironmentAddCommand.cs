using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ConsoleTables;
using PipManager.Cli.Extensions;
using PipManager.Core.Enums;
using PipManager.Core.PyEnvironment;
using static PipManager.Core.PyEnvironment.Search;
using PipManager.Core.Configuration;

namespace PipManager.Cli.Commands.Environment;

[Command("env add", Description = "Add a Python environment.")]
public class EnvironmentAddCommand : ICommand
{
    [CommandOption("auto", 'a', Description = "Automatically detect the Python environment.")]
    public bool AutomaticallyDetect { get; init; }
    
    public ValueTask ExecuteAsync(IConsole console)
    {
        if (AutomaticallyDetect)
        {
            var environments = Detector.ByEnvironmentVariable();

            if (environments.Type == ResponseMessageType.Success)
            {
                console.Output.WriteLine("Detected environments:");
                var table = new ConsoleTable(new ConsoleTableOptions
                {
                    OutputTo = console.Output,
                });
                table.AddColumn(["Id", "Pip Version", "Python Version", "Python Path"]);
                for (var i = 0; i < environments.Message.Count; i++)
                {
                    var environment = environments.Message[i];
                    table.AddRow(i, environment.PipVersion, environment.PythonVersion, environment.PythonPath);
                }
                table.Write(Format.Minimal);
                
                console.Output.Write("Enter the id you want to add: ");
                var id = console.Input.ReadLine();
                
                if (int.TryParse(id, out var index) && index < environments.Message.Count)
                {
                    var environment = environments.Message[index];
                    if (FindEnvironmentByPythonPath(environment.PythonPath).Type == ResponseMessageType.Success)
                    {
                        console.Error.WriteLine("Environment already exists");
                        return default;
                    }
                    
                    console.Output.Write("Set an identifier for this environment: ");
                    var identifier = console.Input.ReadLine();
                    if(string.IsNullOrWhiteSpace(identifier))
                    {
                        console.Error.WriteLine("Identifier cannot be empty");
                        return default;
                    }
                    if (FindEnvironmentByIdentifier(identifier).Type == ResponseMessageType.Success)
                    {
                        console.Error.WriteLine("Identifier already exists");
                        return default;
                    }
                    environment.Identifier = identifier;
                    console.Output.Write("Set this environment as the selected environment? (y/n [default]): ");
                    var selected = console.Input.IsYes();
                    
                    if (selected)
                    {
                        Configuration.AppConfig!.SelectedEnvironment = environment;
                    }
                    
                    Configuration.AppConfig!.Environments.Add(environment);
                    Configuration.Save();
                    
                    console.Output.WriteLine("Environment added successfully");
                }
                else
                {
                    console.Error.WriteLine("Invalid id");
                }
            }
        }

        return default;
    }
}