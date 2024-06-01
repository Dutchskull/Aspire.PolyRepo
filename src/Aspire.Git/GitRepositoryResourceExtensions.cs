using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspire.Git;

public static class GitRepositoryResourceExtensions
{
    private static readonly ProcessCommands _processCommands = new();

    public static IResourceBuilder<NodeAppResource> AddNodeApp(
        this IResourceBuilder<GitRepositoryResource> resource,
        string? name = null,
        string? workingDirectory = null,
        string[]? args = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        _processCommands.NpmInstall(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddNodeApp(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath, workingDirectory, args);
    }

    public static IResourceBuilder<NodeAppResource> AddNpmApp(
        this IResourceBuilder<GitRepositoryResource> resource,
        string? name = null,
        string scriptName = "start",
        string[]? args = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        _processCommands.NpmInstall(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddNpmApp(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath, scriptName, args);
    }

    public static IResourceBuilder<ProjectResource> AddProject(
        this IResourceBuilder<GitRepositoryResource> resource,
        string? name = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        _processCommands.BuildDotNetProject(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddProject(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath);
    }
}