using System.Diagnostics;

namespace Aspire.Git;

public interface IFileSystem
{
    bool DirectoryExists(string path);

    bool FileExists(string path);
}

public interface IProcessCommands
{
    void BuildDotNetProject(string resolvedProjectPath);

    void CloneGitRepository(string gitUrl, string resolvedRepositoryPath);

    void NpmInstall(string resolvedRepositoryPath);
}

public class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public bool FileExists(string path) => File.Exists(path);
}

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