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
        Action<GitRepositoryConfigBuilder> configureGitRepositoryBuilderAction,
        string? workingDirectory = null,
        string[]? args = null)
    {
        GitRepositoryConfig gitRepositoryConfig = configureGitRepositoryBuilderAction
            .InitializeGitRepository();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

        gitRepositoryConfig.ProcessCommandsExecutor.NpmInstall(gitRepositoryConfig.ProjectPath);

        return builder
            .AddNodeApp(
                gitRepositoryConfig.Name,
                gitRepositoryConfig.ProjectPath,
                workingDirectory,
                args);
    }

    public static IResourceBuilder<NodeAppResource> AddNpmGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository,
        string scriptName = "start",
        string[]? args = null)
    {
        GitRepositoryConfig gitRepositoryConfig = configureGitRepository
            .InitializeGitRepository();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

        gitRepositoryConfig.ProcessCommandsExecutor.NpmInstall(gitRepositoryConfig.ProjectPath);

        return builder
            .AddNpmApp(
                gitRepositoryConfig.Name,
                gitRepositoryConfig.ProjectPath,
                scriptName,
                args);
    }

    public static IResourceBuilder<ProjectResource> AddProjectGitRepository(
        this IDistributedApplicationBuilder builder,
        Action<GitRepositoryConfigBuilder> configureGitRepository)
    {
        GitRepositoryConfig gitRepositoryConfig = configureGitRepository
            .InitializeGitRepository();

        gitRepositoryConfig.ProcessCommandsExecutor.BuildDotNetProject(gitRepositoryConfig.ProjectPath);

        return builder
            .AddProject(
                gitRepositoryConfig.Name,
                gitRepositoryConfig.ProjectPath);
    }
}
