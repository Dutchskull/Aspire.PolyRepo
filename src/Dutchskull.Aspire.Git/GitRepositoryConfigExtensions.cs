namespace Dutchskull.Aspire.Git;

internal static class GitRepositoryConfigExtensions
{
    internal static GitRepositoryConfig InitializeGitRepository(this Action<GitRepositoryConfigBuilder> configureGitRepository)
    {
        GitRepositoryConfigBuilder _gitRepositoryConfigBuilder = new();

        configureGitRepository.Invoke(_gitRepositoryConfigBuilder);

        return _gitRepositoryConfigBuilder
            .BuildConfig()
            .SetupGitRepository();
    }

    internal static void CloneGitRepository(this GitRepositoryConfig gitRepositoryConfig)
    {
        if (gitRepositoryConfig.FileSystem.DirectoryExists(gitRepositoryConfig.CloneTargetPath))
        {
            return;
        }

        gitRepositoryConfig.ProcessCommandsExecutor
            .CloneGitRepository(
                gitRepositoryConfig.GitUrl,
                gitRepositoryConfig.CloneTargetPath,
                gitRepositoryConfig.Branch);
    }

    internal static GitRepositoryConfig SetupGitRepository(this GitRepositoryConfig gitRepositoryConfig)
    {
        CloneGitRepository(gitRepositoryConfig);

        if (gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath))
        {
            return gitRepositoryConfig;
        }

        string message = string.Format("Project folder {0} not found", gitRepositoryConfig.ProjectPath);
        throw new Exception(message);
    }
}