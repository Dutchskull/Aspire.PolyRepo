using Aspire.Hosting.ApplicationModel;

namespace Dutchskull.Aspire.PolyRepo;

public class RepositoryResource(string name, RepositoryConfig repositoryConfig) : Resource(name)
{
    public RepositoryConfig RepositoryConfig { get; set; } = repositoryConfig;

    internal string Resolve(string relativeProjectPath) =>
        Path.GetFullPath(Path.Combine(RepositoryConfig.RepositoryPath, relativeProjectPath));
}
