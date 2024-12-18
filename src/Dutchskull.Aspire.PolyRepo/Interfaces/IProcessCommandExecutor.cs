namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IProcessCommandExecutor
{
    int BuildDotNetProject(string resolvedProjectPath);

    void CloneGitRepository(GitConfig gitConfig, string resolvedRepositoryPath, string? branch = null);

    int NpmInstall(string resolvedRepositoryPath);

    void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath);
}