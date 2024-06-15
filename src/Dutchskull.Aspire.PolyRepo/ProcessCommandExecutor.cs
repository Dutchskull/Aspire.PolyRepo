using System.Diagnostics;
using System.Text;
using Dutchskull.Aspire.PolyRepo.Interfaces;

namespace Dutchskull.Aspire.PolyRepo;

public class ProcessCommandExecutor : IProcessCommandExecutor
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

        StringBuilder output = new();
        StringBuilder error = new();

        process.OutputDataReceived += LogData(output, "OUTPUT");

        process.ErrorDataReceived += LogData(error, "ERROR");

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            Console.WriteLine($"Process {fileName} {arguments} finished successfully.");
            return process.ExitCode;
        }

        string errorMessage = $"Process {fileName} {arguments} failed with exit code {process.ExitCode}: {error}";
        Console.WriteLine(errorMessage);
        throw new Exception(errorMessage);
    }

    private static DataReceivedEventHandler LogData(StringBuilder output,string type)
    {
        return (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            output.AppendLine(e.Data);
            Console.WriteLine($"[{type}]: {e.Data}");
        };
    }
}