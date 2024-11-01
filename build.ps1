param(
    [string] $Architecture = "x64",
    [string] $Version = "1.1.0.0"
)

$ErrorActionPreference = "Stop";

Write-Output "Start building singleWithRuntime...";

dotnet publish src/PipManager.Cli.csproj -c Release -r "win-$Architecture" -o "build/$Version/singleWithRuntime" -p:Platform=$Architecture -p:PublishReadyToRun=true -p:EnableCompressionInSingleFile=true -p:PublishSingleFile=true -p:SelfContained=true -p:AssemblyVersion=$Version -p:Configuration=Release;

Write-Output "Build Finished";

[Console]::ReadKey()