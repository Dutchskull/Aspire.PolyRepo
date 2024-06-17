using Dutchskull.Aspire.PolyRepo.Interfaces;

namespace Dutchskull.Aspire.PolyRepo;

public record RepositoryConfig
{
    public required string? Branch { get; init; }

    public required string RepositoryPath { get; init; }

    public required IFileSystem FileSystem { get; init; }

    public required string GitUrl { get; init; }

    public required IProcessCommandExecutor ProcessCommandsExecutor { get; init; }
    
    public required bool KeepUpToDate { get; init; }
}