namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IProcessCommandExecutor
{
    int BuildDotNetProject(string resolvedProjectPath);

    void CloneGitRepository(string gitUrl, string resolvedRepositoryPath, string? branch = null);

    int NpmInstall(string resolvedRepositoryPath, string? installerArguments = null,
        PackageManager packageManager = PackageManager.Npm);

    void PullAndResetRepository(string repositoryConfigRepositoryPath);
}