namespace Aspire.Git;

public class GitRepositoryResource(string name, string repositoryPath, string projectPath) : Resource(name), IResourceWithEnvironment, IResourceWithServiceDiscovery
{
    public string RepositoryPath { get; } = repositoryPath;

    public string ProjectPath { get; } = projectPath;
}