using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static GitRepositoryConfigBuilder GitRepositoryConfigBuilder = new();

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
        GitRepositoryConfigBuilder = new();

        configureGitRepository.Invoke(GitRepositoryConfigBuilder);

        return GitRepositoryConfigBuilder.Build();
    }

    private static GitRepositoryResource CreateGitRepositoryResource(GitRepositoryConfig gitRepositoryConfig)
    {
        string gitProjectName = GetProjectNameFromGitUrl(gitRepositoryConfig.GitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(gitRepositoryConfig.RepositoryPath), gitProjectName);

        string projectName = gitRepositoryConfig.Name ?? gitProjectName;

        string resolvedProjectPath = Path.Join(resolvedRepositoryPath, gitRepositoryConfig.RelativeProjectPath);

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

        if (!fileSystem.FileExists(gitRepositoryResource.ProjectPath))
        {
            string message = string.Format("Project folder {0} not found", gitRepositoryResource.ProjectPath);
            throw new Exception(message);
        }
    }
}

public class GitRepositoryConfigBuilder
{
    private string _gitUrl;
    private string? _name;
    private string _relativeProjectPath = ".";
    private string _repositoryPath = ".";

    public GitRepositoryConfig Build()
    {
        if (string.IsNullOrEmpty(_gitUrl))
        {
            throw new InvalidOperationException("GitUrl must be provided");
        }

        return new GitRepositoryConfig
        {
            GitUrl = _gitUrl,
            Name = _name,
            RepositoryPath = _repositoryPath,
            RelativeProjectPath = _relativeProjectPath
        };
    }

    public GitRepositoryConfigBuilder WithGitUrl(string gitUrl)
    {
        _gitUrl = gitUrl;
        return this;
    }

    public GitRepositoryConfigBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public GitRepositoryConfigBuilder WithRelativeProjectPath(string relativeProjectPath)
    {
        _relativeProjectPath = relativeProjectPath;
        return this;
    }

    public GitRepositoryConfigBuilder WithRepositoryPath(string repositoryPath)
    {
        _repositoryPath = repositoryPath;
        return this;
    }
}

public class GitRepositoryConfig
{
    public string GitUrl { get; init; }
    public string? Name { get; init; }
    public string RelativeProjectPath { get; init; }
    public string RepositoryPath { get; init; }
}