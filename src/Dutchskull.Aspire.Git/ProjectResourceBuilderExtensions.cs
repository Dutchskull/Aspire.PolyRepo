using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dutchskull.Aspire.Git;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<NodeAppResource> AddNodeGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository,
        string? name = null,
        string? workingDirectory = null,
        string[]? args = null)
    {
        GitRepositoryConfig gitRepositoryConfig = BuildGitRepositoryConfig(configureGitRepository);

        GitRepositoryResource gitRepositoryResource = CreateGitRepositoryResource(gitRepositoryConfig);

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

        gitRepositoryConfig.ProcessCommandsExecutor.NpmInstall(gitRepositoryResource.ProjectPath);

        return builder
            .CreateResourceBuilder(gitRepositoryResource)
            .ApplicationBuilder
            .AddNodeApp(
                name ?? gitRepositoryResource.Name,
                gitRepositoryResource.ProjectPath,
                workingDirectory,
                args);
    }

    public static IResourceBuilder<NodeAppResource> AddNpmGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository,
        string? name = null,
        string scriptName = "start",
        string[]? args = null)
    {
        GitRepositoryConfig gitRepositoryConfig = BuildGitRepositoryConfig(configureGitRepository);

        GitRepositoryResource gitRepositoryResource = CreateGitRepositoryResource(gitRepositoryConfig);

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

        gitRepositoryConfig.ProcessCommandsExecutor.NpmInstall(gitRepositoryResource.ProjectPath);

        return builder
            .CreateResourceBuilder(gitRepositoryResource)
            .ApplicationBuilder
            .AddNpmApp(
                name ?? gitRepositoryResource.Name,
                gitRepositoryResource.ProjectPath,
                scriptName,
                args);
    }

    public static IResourceBuilder<ProjectResource> AddProjectGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository,
        string? name = null)
    {
        GitRepositoryConfig gitRepositoryConfig = BuildGitRepositoryConfig(configureGitRepository);

        GitRepositoryResource gitRepositoryResource = CreateGitRepositoryResource(gitRepositoryConfig);

        gitRepositoryConfig.ProcessCommandsExecutor.BuildDotNetProject(gitRepositoryResource.ProjectPath);

        return builder
            .CreateResourceBuilder(gitRepositoryResource)
            .ApplicationBuilder
            .AddProject(
                name ?? gitRepositoryResource.Name,
                gitRepositoryResource.ProjectPath);
    }

    private static GitRepositoryConfig BuildGitRepositoryConfig(Action<GitRepositoryConfigBuilder> configureGitRepository)
    {
        GitRepositoryConfigBuilder _gitRepositoryConfigBuilder = new();

        configureGitRepository.Invoke(_gitRepositoryConfigBuilder);

        return _gitRepositoryConfigBuilder.Build();
    }

    private static void CloneGitRepository(GitRepositoryConfig gitRepositoryConfig, GitRepositoryResource gitRepositoryResource)
    {
        if (gitRepositoryConfig.FileSystem.DirectoryExists(gitRepositoryResource.RepositoryPath))
        {
            return;
        }

        gitRepositoryConfig.ProcessCommandsExecutor.CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryResource.RepositoryPath, gitRepositoryConfig.Branch);
    }

    private static GitRepositoryResource CreateGitRepositoryResource(GitRepositoryConfig gitRepositoryConfig)
    {
        string gitProjectName = GitUrlUtilities.GetProjectNameFromGitUrl(gitRepositoryConfig.GitUrl);
        string resolvedRepositoryPath = Path.Combine(Path.GetFullPath(gitRepositoryConfig.CloneTargetPath), gitProjectName);

        string projectName = gitRepositoryConfig.Name ?? gitProjectName;

        string resolvedProjectPath = Path.GetFullPath(Path.Join(resolvedRepositoryPath, gitRepositoryConfig.ProjectPath));

        GitRepositoryResource gitRepositoryResource = new(projectName, resolvedRepositoryPath, resolvedProjectPath);

        SetupGitRepository(gitRepositoryConfig, gitRepositoryResource);

        return gitRepositoryResource;
    }

    private static void SetupGitRepository(GitRepositoryConfig gitRepositoryConfig, GitRepositoryResource gitRepositoryResource)
    {
        CloneGitRepository(gitRepositoryConfig, gitRepositoryResource);

        if (gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryResource.ProjectPath))
        {
            return;
        }

        string message = string.Format("Project folder {0} not found", gitRepositoryResource.ProjectPath);
        throw new Exception(message);
    }
}