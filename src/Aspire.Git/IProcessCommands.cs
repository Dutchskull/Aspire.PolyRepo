namespace Aspire.Git;

public interface IProcessCommands
{
    void BuildDotNetProject(string resolvedProjectPath);

    void CloneGitRepository(string gitUrl, string resolvedRepositoryPath);

    void NpmInstall(string resolvedRepositoryPath);
}