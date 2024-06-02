using System.Diagnostics;

namespace Aspire.Git;

public class ProcessCommands : IProcessCommands
{
    public int BuildDotNetProject(string resolvedProjectPath) =>
        RunProcess("dotnet", $"build {resolvedProjectPath}");

    public int CloneGitRepository(string gitUrl, string resolvedRepositoryPath, string? branch = null) =>
        string.IsNullOrEmpty(branch)
            ? RunProcess("git", $"clone {gitUrl} {resolvedRepositoryPath}")
            : RunProcess("git", $"clone --branch {branch} {gitUrl} {resolvedRepositoryPath}");

    public int NpmInstall(string resolvedRepositoryPath) =>
        RunProcess("cmd.exe", $"/C cd {resolvedRepositoryPath} && npm i");

    private static int RunProcess(string fileName, string arguments)
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
            return process.ExitCode;
        }

        throw new Exception($"Process {fileName} {arguments} failed with exit code {process.ExitCode}: {process.StandardError.ReadToEnd()}");
    }
}