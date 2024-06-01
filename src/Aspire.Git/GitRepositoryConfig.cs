namespace Aspire.Git;

public class GitRepositoryConfig
{
    public required string Branch { get; init; }

    public string GitUrl { get; init; } = string.Empty;

    public string? Name { get; init; }

    public required string RelativeProjectPath { get; init; }

    public required string RepositoryPath { get; init; }
}