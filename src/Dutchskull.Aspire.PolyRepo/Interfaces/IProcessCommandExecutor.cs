namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IProcessCommandExecutor
{
    int BuildDotNetProject(string resolvedProjectPath, string[]? args);

    void CloneGitRepository(GitConfig gitConfig, string resolvedRepositoryPath, string? branch = null);

    [Obsolete("Already present in Aspire.Hosting.Javascript .WithNpm() extension method, please use it instead.")]
    int NpmInstall(string resolvedRepositoryPath);

    void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath);
}