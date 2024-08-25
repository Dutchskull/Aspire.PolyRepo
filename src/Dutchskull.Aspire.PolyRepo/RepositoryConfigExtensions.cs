namespace Dutchskull.Aspire.PolyRepo;

internal static class RepositoryConfigExtensions
{
    internal static RepositoryConfig InitializeRepository(
        this Action<RepositoryConfigBuilder> configureGitRepository,
        string repositoryUrl)
    {
        RepositoryConfigBuilder gitRepositoryConfigBuilder = new();

        gitRepositoryConfigBuilder.WithGitUrl(repositoryUrl);

        configureGitRepository.Invoke(gitRepositoryConfigBuilder);

        return gitRepositoryConfigBuilder
            .Build()
            .SetupRepository();
    }

    internal static RepositoryConfig CloneRepository(this RepositoryConfig repositoryConfig)
    {
        bool repositoryExists = repositoryConfig.FileSystem.DirectoryExists(repositoryConfig.RepositoryPath);

        if (!repositoryExists)
        {
            repositoryConfig.ProcessCommandsExecutor
                .CloneGitRepository(
                    repositoryConfig.GitConfig,
                    repositoryConfig.RepositoryPath,
                    repositoryConfig.Branch);

            return repositoryConfig;
        }

        if (repositoryConfig.KeepUpToDate)
        {
            repositoryConfig.ProcessCommandsExecutor
                .PullAndResetRepository(repositoryConfig.RepositoryPath);
        }

        return repositoryConfig;
    }

    private static RepositoryConfig SetupRepository(this RepositoryConfig gitRepositoryConfig) =>
        CloneRepository(gitRepositoryConfig);
}