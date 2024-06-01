using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspire.Git;

public class GitRepositoryResource(string name, string repositoryPath, string projectPath) : Resource(name), IResourceWithEnvironment, IResourceWithServiceDiscovery
{
    public string ProjectPath { get; } = projectPath;
    public string RepositoryPath { get; } = repositoryPath;
}