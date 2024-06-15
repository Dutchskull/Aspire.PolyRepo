namespace Dutchskull.Aspire.PolyRepo;

internal static class RepositoryConfigExtensions
{
    internal static RepositoryConfig InitializeRepository(this Action<RepositoryConfigBuilder> configureGitRepository, string repositoryUrl)
    {
        RepositoryConfigBuilder _gitRepositoryConfigBuilder = new();

        _gitRepositoryConfigBuilder.WithGitUrl(repositoryUrl);

        configureGitRepository.Invoke(_gitRepositoryConfigBuilder);

        return _gitRepositoryConfigBuilder
            .Build()
            .SetupRepository();
    }

    internal static RepositoryConfig CloneRepository(this RepositoryConfig repositoryConfig)
    {
        if (repositoryConfig.FileSystem.DirectoryExists(repositoryConfig.RepositoryPath))
        {
            return repositoryConfig;
        }

        repositoryConfig.ProcessCommandsExecutor
            .CloneGitRepository(
                repositoryConfig.GitUrl,
                repositoryConfig.RepositoryPath,
                repositoryConfig.Branch);

        return repositoryConfig;
    }

    internal static RepositoryConfig SetupRepository(this RepositoryConfig gitRepositoryConfig) => 
        CloneRepository(gitRepositoryConfig);
}