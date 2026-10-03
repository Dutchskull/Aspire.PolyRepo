using System.Diagnostics;
using System.Text;
using Dutchskull.Aspire.PolyRepo.Interfaces;
using LibGit2Sharp;

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
    }

    public void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath, string? branch = null)
    {
        using Repository repository = new(repositoryConfigRepositoryPath);

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

        IEnumerable<string> references = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repository, remote.Name, references, fetchOptions, null);

        if (!string.IsNullOrWhiteSpace(branch) && repository.Head.FriendlyName != branch)
        {
            TryCheckoutBranch(repository, remote, branch);
        }

        string? branchName = repository.Head.TrackedBranch?.FriendlyName;

        ArgumentNullException.ThrowIfNull(branchName);

        Branch? remoteBranch = repository.Branches[branchName];
        Commit? latestCommit = remoteBranch.Tip;

        repository.Reset(ResetMode.Hard, latestCommit);
    }

    private static void TryCheckoutBranch(Repository repository, Remote remote, string branch)
    {
        string currentBranch = repository.Head.FriendlyName;
        string repositoryPath = repository.Info.WorkingDirectory;

        if (repository.RetrieveStatus(new StatusOptions { IncludeUntracked = false }).IsDirty)
        {
            Console.WriteLine($"Could not switch repository {repositoryPath} from {currentBranch} to {branch}: uncommitted changes found. Staying on {currentBranch}.");

            return;
        }

        Branch? remoteBranch = repository.Branches[$"{remote.Name}/{branch}"];

        if (remoteBranch == null)
        {
            Console.WriteLine($"Could not switch repository {repositoryPath} from {currentBranch} to {branch}: branch not found on remote {remote.Name}. Staying on {currentBranch}.");

            return;
        }

        Branch localBranch = repository.Branches[branch] ?? repository.CreateBranch(branch, remoteBranch.Tip);
        repository.Branches.Update(localBranch, x => x.TrackedBranch = remoteBranch.CanonicalName);

        try
        {
            Commands.Checkout(repository, localBranch);
        }
        catch (CheckoutConflictException exception)
        {
            Console.WriteLine($"Could not switch repository {repositoryPath} from {currentBranch} to {branch}: {exception.Message} Staying on {currentBranch}.");

            return;
        }

        Console.WriteLine($"Switched repository {repositoryPath} from {currentBranch} to {branch} successfully.");
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

    [Obsolete("Already present in Aspire.Hosting.Javascript .WithNpm() extension method, please use it instead.")]
    public int NpmInstall(string resolvedRepositoryPath) => RunProcess("cmd.exe", $"/C cd {resolvedRepositoryPath} && npm i");
}