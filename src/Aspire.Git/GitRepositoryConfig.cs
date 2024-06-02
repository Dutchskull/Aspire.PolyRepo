namespace Aspire.Git;

public class GitRepositoryConfig
{
    public required string Branch { get; init; }

    public string GitUrl { get; init; } = string.Empty;

    public string? Name { get; init; }

    public required string ProjectPath { get; init; }

    public required string CloneTargetPath { get; init; }
}