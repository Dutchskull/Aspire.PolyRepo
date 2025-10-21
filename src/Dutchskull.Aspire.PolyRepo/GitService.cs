using Dutchskull.Aspire.PolyRepo.Interfaces;
using LibGit2Sharp;

namespace Dutchskull.Aspire.PolyRepo;

public class GitService : IGitService
{
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

    public void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath)
    {
        using Repository repository = new(repositoryConfigRepositoryPath);

        string? branchName = repository.Head.TrackedBranch?.FriendlyName;
        Remote? remote = repository.Network.Remotes.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(remote);
        ArgumentNullException.ThrowIfNull(branchName);

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

        Branch? remoteBranch = repository.Branches[branchName];
        Commit? latestCommit = (remoteBranch?.Tip) ?? throw new InvalidOperationException($"Remote branch '{branchName}' not found or has no commits.");
        repository.Reset(ResetMode.Hard, latestCommit);
    }

}