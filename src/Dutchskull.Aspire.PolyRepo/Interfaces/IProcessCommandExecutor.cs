namespace Dutchskull.Aspire.PolyRepo.Interfaces;

public interface IProcessCommandExecutor
{
    int BuildDotNetProject(string resolvedProjectPath, string[]? args);

    int NpmInstall(string resolvedRepositoryPath);

}
