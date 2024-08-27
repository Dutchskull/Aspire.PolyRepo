using Dutchskull.Aspire.PolyRepo.Interfaces;

namespace Dutchskull.Aspire.PolyRepo;

public record RepositoryConfig
{
    internal RepositoryConfig()
    {
    }

    public required string? Branch { get; init; }

    public required string RepositoryPath { get; init; }

    public required IFileSystem FileSystem { get; init; }

    public required GitConfig GitConfig { get; init; }

    public required IProcessCommandExecutor ProcessCommandsExecutor { get; init; }

    public required bool KeepUpToDate { get; init; }

    public required string RepositoryUrl { get; init; }
}