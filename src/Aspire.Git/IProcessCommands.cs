namespace Aspire.Git;

public interface IProcessCommands
{
    int BuildDotNetProject(string resolvedProjectPath);

    int CloneGitRepository(string gitUrl, string resolvedRepositoryPath, string? branch = null);

    int NpmInstall(string resolvedRepositoryPath);
}