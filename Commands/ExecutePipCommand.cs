using System.Diagnostics;
using PipManager.Core.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PipManager.Cli.Commands;


public class ExecutePipCommand
{
    public static void Start(string[] args)
    {
        if(Configuration.AppConfig!.Environments.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Python environment has not been added yet, add it with the 'env add' command.[/]");
        }
        if(Configuration.AppConfig.SelectedEnvironment == null)
        {
            AnsiConsole.MarkupLine("[red]No environment selected, select it with the 'env select' command.[/]");
        }
        
        AnsiConsole.MarkupLine($"[green]Running under Pip {Configuration.AppConfig.SelectedEnvironment!.PipVersion} (Python {Configuration.AppConfig.SelectedEnvironment.PythonVersion})[/]");

        var process = new Process();
        process.StartInfo.FileName = Configuration.AppConfig.SelectedEnvironment.PythonPath;
        process.StartInfo.Arguments = string.Join(" ", ["-m", "pip", ..args]);
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                AnsiConsole.WriteLine(e.Data);
            }
        };
        
        process.ErrorDataReceived += (_, e) => 
        {
            if (e.Data != null)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{e.Data}[/]");
            }
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Exception: {ex.Message}");
        }
    }
}