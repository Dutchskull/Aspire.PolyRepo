using System.Diagnostics;

namespace Aspire.Git;

public class ProcessCommands : IProcessCommands
{
    public void BuildDotNetProject(string resolvedProjectPath) =>
        RunProcess("dotnet", $"build {resolvedProjectPath}");

    public void CloneGitRepository(string gitUrl, string resolvedRepositoryPath) =>
        RunProcess("git", $"clone {gitUrl} {resolvedRepositoryPath}");

    public void NpmInstall(string resolvedRepositoryPath) =>
        RunProcess("npm", $"i --prefix {resolvedRepositoryPath}");

    private static void RunProcess(string fileName, string arguments)
    {
        Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            return;
        }

        throw new Exception($"Process {fileName} {arguments} failed with exit code {process.ExitCode}: {process.StandardError.ReadToEnd()}");
    }
}