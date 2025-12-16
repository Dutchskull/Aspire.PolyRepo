using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.JavaScript;

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
        string relativeProjectPath,
        string[]? buildArgs = null)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        repository.Resource.RepositoryConfig?.ProcessCommandsExecutor.BuildDotNetProject(projectPath, buildArgs);

        return builder.AddProject(name, projectPath);
    }

    public static IResourceBuilder<JavaScriptAppResource> AddNpmAppFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string scriptName = "start")
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        return builder.AddJavaScriptApp(name, projectPath, scriptName);
    }

    public static IResourceBuilder<ViteAppResource> AddViteAppFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string scriptName = "dev"
        )
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);
        return builder.AddViteApp(name, projectPath, scriptName);
    }

    public static IResourceBuilder<NodeAppResource> AddNodeAppFromRepository(
        this IDistributedApplicationBuilder builder,
        string name,
        IResourceBuilder<RepositoryResource> repository,
        string relativeProjectPath,
        string? workingDirectory = null)
    {
        string projectPath = repository.Resource.Resolve(relativeProjectPath);

        return builder.AddNodeApp(name, projectPath, workingDirectory ?? "app.js");
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