using System.Diagnostics;

namespace Aspire.Git;

internal class ProcessCommands
{
    internal static void BuildDotNetProject(string resolvedProjectPath) =>
        RunProcess("dotnet", $"build {resolvedProjectPath}");

    internal static void CloneGitRepository(string gitUrl, string resolvedRepositoryPath) =>
        RunProcess("git", $"clone {gitUrl} {resolvedRepositoryPath}");

    internal static void NpmInstall(string resolvedRepositoryPath) =>
        RunProcess("CMD.exe", $"/C cd {resolvedRepositoryPath} && npm i");

    internal static void RunProcess(string fileName, string arguments)
    {
        ProcessStartInfo processStartInfo = new()
        {
            FileName = fileName,
            Arguments = arguments,
        };

        Process process = new()
        {
            StartInfo = processStartInfo
        };

        process.Start();
        process.WaitForExit();
    }
}