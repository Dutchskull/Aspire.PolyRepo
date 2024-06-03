using Dutchskull.Aspire.Git.Interfaces;

namespace Dutchskull.Aspire.Git;

public class GitRepositoryConfig
{
    public required string Branch { get; init; }

    public required string CloneTargetPath { get; init; }

    public required IFileSystem FileSystem { get; init; }

    public string GitUrl { get; init; } = string.Empty;

    public string? Name { get; init; }

    public required IProcessCommandsExecutor ProcessCommandsExecutor { get; init; }

    public required string ProjectPath { get; init; }
}