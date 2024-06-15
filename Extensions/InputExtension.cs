using CliFx.Infrastructure;

namespace PipManager.Cli.Extensions;

public static class InputExtension
{
    public static bool IsYes(this ConsoleReader consoleReader)
        => consoleReader.ReadLine()?.ToLowerInvariant() == "y";
}