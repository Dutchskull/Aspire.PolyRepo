using System.Diagnostics;
using System.Text;
using Dutchskull.Aspire.PolyRepo.Interfaces;
using LibGit2Sharp;
using System.Linq;

namespace Dutchskull.Aspire.PolyRepo;

public class ProcessCommandExecutor : IProcessCommandExecutor
{
    public int BuildDotNetProject(string resolvedProjectPath, string[]? args = null)
    {
        string arguments = $"build {resolvedProjectPath}";

        if (args != null && args.Length > 0)
        {
            arguments = $"{arguments} {string.Join(" ", args)}";
        }

        return RunProcess("dotnet", arguments);
    }

    public void CloneGitRepository(GitConfig gitConfig, string resolvedRepositoryPath, string? branch = null)
    {
        CloneOptions cloneOptions = new()
        {
            BranchName = branch,
            FetchOptions =
            {
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = gitConfig.Username,
                    Password = gitConfig.Password
                },
                CustomHeaders = gitConfig.CustomHeaders
            }
        };

        Repository.Clone(gitConfig.Url, resolvedRepositoryPath, cloneOptions);

        if (string.IsNullOrWhiteSpace(gitConfig.Tag))
        {
            return;
        }

        using Repository repo = new(resolvedRepositoryPath);

        Remote? remote = repo.Network.Remotes.FirstOrDefault();
        ArgumentNullException.ThrowIfNull(remote);

        string[] tagRefSpec = [$"refs/tags/{gitConfig.Tag}:refs/tags/{gitConfig.Tag}"];
        Commands.Fetch(repo, remote.Name, tagRefSpec, cloneOptions.FetchOptions, null);

        Tag? tag = repo.Tags[gitConfig.Tag] ??
            throw new Exception($"Tag '{gitConfig.Tag}' not found in repository after fetch.");

        Commit? commit = (tag.PeeledTarget as Commit ?? repo.Lookup<Commit>(tag.Target.Sha)) ??
            throw new Exception($"Tag '{gitConfig.Tag}' does not resolve to a commit.");

        repo.Reset(ResetMode.Hard, commit);
    }

    public int NpmInstall(string resolvedRepositoryPath) =>
        RunProcess("cmd.exe", $"/C cd {resolvedRepositoryPath} && npm i");

    public void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath)
    {
        using Repository repository = new(repositoryConfigRepositoryPath);

        if (string.IsNullOrWhiteSpace(gitConfig.Tag))
        {
            FetchCurrentBranch(gitConfig, repository);

            return;
        }

        FetchCurrentTag(gitConfig, repository);
    }

    private static void FetchCurrentTag(GitConfig gitConfig, Repository repository)
    {
        Remote? remote = repository.Network.Remotes.FirstOrDefault();
        ArgumentNullException.ThrowIfNull(remote);

        FetchOptions fetchOptions = new()
        {
            CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
            {
                Username = gitConfig.Username,
                Password = gitConfig.Password
            },
            CustomHeaders = gitConfig.CustomHeaders
        };

        List<string> references = [.. remote.FetchRefSpecs.Select(x => x.Specification)];
        references.Add($"refs/tags/{gitConfig.Tag}:refs/tags/{gitConfig.Tag}");

        Commands.Fetch(repository, remote.Name, references, fetchOptions, null);

        Tag? tag = repository.Tags[gitConfig.Tag] ??
            throw new Exception($"Tag '{gitConfig.Tag}' not found after fetch.");

        Commit? commit = (tag.PeeledTarget as Commit ?? repository.Lookup<Commit>(tag.Target.Sha)) ??
            throw new Exception($"Tag '{gitConfig.Tag}' does not resolve to a commit.");

        repository.Reset(ResetMode.Hard, commit);
    }

    private static void FetchCurrentBranch(GitConfig gitConfig, Repository repository)
    {
        string? branchName = repository.Head.TrackedBranch?.FriendlyName;
        Remote? branchRemote = repository.Network.Remotes.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(branchRemote);
        ArgumentNullException.ThrowIfNull(branchName);

        FetchOptions branchFetchOptions = new()
        {
            CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
            {
                Username = gitConfig.Username,
                Password = gitConfig.Password
            },
            CustomHeaders = gitConfig.CustomHeaders
        };

        IEnumerable<string> branchReferences = branchRemote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repository, branchRemote.Name, branchReferences, branchFetchOptions, null);

        Branch? remoteBranch = repository.Branches[branchName];
        Commit? latestCommit = remoteBranch?.Tip;

        repository.Reset(ResetMode.Hard, latestCommit);
    }

    private static DataReceivedEventHandler LogData(StringBuilder output, string type)
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
}