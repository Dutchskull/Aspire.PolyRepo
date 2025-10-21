namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IGitService
{
    void CloneGitRepository(GitConfig gitConfig, string resolvedRepositoryPath, string? branch = null);

    void PullAndResetRepository(GitConfig gitConfig, string repositoryConfigRepositoryPath);
}