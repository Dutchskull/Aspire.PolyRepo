namespace Dutchskull.Aspire.Git.Interfaces;

public interface IProcessCommandExecutor
{
    int BuildDotNetProject(string resolvedProjectPath);

    int CloneGitRepository(string gitUrl, string resolvedRepositoryPath, string? branch = null);

    int NpmInstall(string resolvedRepositoryPath);
}