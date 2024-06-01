using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    private static GitRepositoryConfigBuilder _gitRepositoryConfigBuilder = new();

    public static IResourceBuilder<GitRepositoryResource> AddGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository,
        IProcessCommands? processCommands = null,
        IFileSystem? fileSystem = null)
    {
        GitRepositoryConfig gitRepositoryConfig = BuildGitRepositoryConfig(configureGitRepository);

        GitRepositoryResource gitRepositoryResource = CreateGitRepositoryResource(gitRepositoryConfig);

        SetupGitRepository(gitRepositoryConfig, gitRepositoryResource, processCommands, fileSystem);

        return builder.CreateResourceBuilder(gitRepositoryResource);
    }

    private static GitRepositoryConfig BuildGitRepositoryConfig(Action<GitRepositoryConfigBuilder> configureGitRepository)
    {
        _gitRepositoryConfigBuilder = new();

        configureGitRepository.Invoke(_gitRepositoryConfigBuilder);

        return _gitRepositoryConfigBuilder.Build();
    }

    private static GitRepositoryResource CreateGitRepositoryResource(GitRepositoryConfig gitRepositoryConfig)
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitRepositoryConfig.GitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(gitRepositoryConfig.RepositoryPath), gitProjectName);

        string projectName = gitRepositoryConfig.Name ?? gitProjectName;

        string resolvedProjectPath = Path.GetFullPath(Path.Join(resolvedRepositoryPath, gitRepositoryConfig.RelativeProjectPath));

        return new GitRepositoryResource(projectName, resolvedRepositoryPath, resolvedProjectPath);
    }

    private static string GetProjectNameFromGitUrl(string gitUrl)
    {
        if (gitUrl.EndsWith(".git"))
        {
            gitUrl = gitUrl[..^4];
        }

        return gitUrl.Split('/')[^1];
    }

    private static void SetupGitRepository(GitRepositoryConfig gitRepositoryConfig, GitRepositoryResource gitRepositoryResource, IProcessCommands? processCommands, IFileSystem? fileSystem)
    {
        fileSystem ??= new FileSystem();

        if (!fileSystem.DirectoryExists(gitRepositoryResource.RepositoryPath))
        {
            processCommands ??= new ProcessCommands();
            processCommands.CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryResource.RepositoryPath);
        }

        if (fileSystem.FileOrDirectoryExists(gitRepositoryResource.ProjectPath))
        {
            return;
        }

        string message = string.Format("Project folder {0} not found", gitRepositoryResource.ProjectPath);
        throw new Exception(message);
    }
}