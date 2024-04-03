using System.Diagnostics;

namespace Aspire.Git;

internal class ProcessCommands
{
    internal static void BuildDotNetProject(string resolvedProjectPath) =>
        RunProcess("dotnet", $"build {resolvedProjectPath}");

    internal static void CloneGitRepository(string gitUrl, string resolvedRepositoryPath) =>
        RunProcess("git", $"clone {gitUrl} {resolvedRepositoryPath}");

    internal static void NpmInstall(string resolvedRepositoryPath) =>
        RunProcess("npm", $"i --prefix {resolvedRepositoryPath}");

    internal static void RunProcess(string fileName, string arguments)
    {
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = fileName,
                Arguments = arguments,
            }
        };

        process.Start();
        process.WaitForExit();
    }
}