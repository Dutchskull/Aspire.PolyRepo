using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspire.Git;

public class GitRepositoryResource(string name, string repositoryPath, string projectPath, string? branch = null) : Resource(name), IResourceWithEnvironment, IResourceWithArgs, IResourceWithServiceDiscovery
{
    public string? Branch { get; } = branch;

    public string ProjectPath { get; } = projectPath;

    public string RepositoryPath { get; } = repositoryPath;
}