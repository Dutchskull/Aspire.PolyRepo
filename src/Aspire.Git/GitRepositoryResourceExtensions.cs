namespace Aspire.Git;

public static class GitRepositoryResourceExtensions
{
    public static IResourceBuilder<ProjectResource> AddProject(this IResourceBuilder<GitRepositoryResource> resource, string? name = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        ProcessCommands.BuildDotNetProject(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddProject(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath);
    }

    public static IResourceBuilder<NodeAppResource> AddNodeApp(this IResourceBuilder<GitRepositoryResource> resource, string? name = null, string? workingDirectory = null, string[]? args = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        ProcessCommands.NpmInstall(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddNodeApp(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath, workingDirectory, args);
    }

    public static IResourceBuilder<NodeAppResource> AddNpmApp(this IResourceBuilder<GitRepositoryResource> resource, string? name = null, string scriptName = "start", string[]? args = null)
    {
        GitRepositoryResource gitRepositoryResource = resource.Resource;

        ProcessCommands.NpmInstall(gitRepositoryResource.ProjectPath);

        return resource.ApplicationBuilder.AddNpmApp(name ?? gitRepositoryResource.Name, resource.Resource.ProjectPath, scriptName, args);
    }
}