using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Dutchskull.Aspire.PolyRepo;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<RepositoryResource> AddRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        string repositoryUrl,
        Action<RepositoryConfigBuilder> configureGitRepositoryBuilderAction = default!)
    {
        RepositoryConfig gitRepositoryConfig = configureGitRepositoryBuilderAction.InitializeRepository(repositoryUrl);

        RepositoryResource resource = new(name, gitRepositoryConfig);

        return builder.CreateResourceBuilder(resource);
    }

    public static IResourceBuilder<ProjectResource> AddProjectFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        repository.Resource.RepositoryConfig?.ProcessCommandsExecutor.BuildDotNetProject(projectPath);

        return builder.AddProject(name, projectPath);
    }

    public static IResourceBuilder<NodeAppResource> AddNpmAppFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string scriptName = "start",
        string[]? args = null)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        repository.Resource.RepositoryConfig?.ProcessCommandsExecutor.NpmInstall(projectPath);

        return builder.AddNpmApp(name, projectPath, scriptName, args);
    }

    public static IResourceBuilder<NodeAppResource> AddNodeAppFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string? workingDirectory = null,
        string[]? args = null)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        repository.Resource.RepositoryConfig?.ProcessCommandsExecutor.NpmInstall(projectPath);

        return builder.AddNodeApp(name, projectPath, workingDirectory, args);
    }

    public static IResourceBuilder<ContainerResource> AddDockerFileFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string? dockerFilePath = null,
        string? stage = null)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);

        return builder.AddDockerfile(name, projectPath, dockerFilePath, stage);
    }
}