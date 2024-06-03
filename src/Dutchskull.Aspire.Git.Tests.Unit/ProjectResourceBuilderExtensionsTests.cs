using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Dutchskull.Aspire.Git.Interfaces;
using NSubstitute;

namespace Dutchskull.Aspire.Git.Tests.Unit;

public class ProjectResourceBuilderExtensionsTests
{
    private readonly IDistributedApplicationBuilder _builderMock;
    private readonly IFileSystem _fileSystemMock;
    private readonly ProcessCommandExecutor _processCommandsMock;

    public ProjectResourceBuilderExtensionsTests()
    {
        _builderMock = Substitute.For<IDistributedApplicationBuilder>();
        _processCommandsMock = Substitute.For<ProcessCommandExecutor>();
        _fileSystemMock = Substitute.For<IFileSystem>();
    }

    [Fact]
    public void AddGitRepository_ShouldCloneRepositoryIfNotExists()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string repositoryPath = Path.Combine(Path.GetTempPath(), "repo");
        string expectedPath = Path.Combine(repositoryPath, "repo");

        _builderMock.CreateResourceBuilder(Arg.Any<GitRepositoryResource>())
            .Returns(Substitute.For<IResourceBuilder<GitRepositoryResource>>());

        _fileSystemMock.DirectoryExists(repositoryPath).Returns(false);
        _fileSystemMock.FileOrDirectoryExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock
            .AddProjectGitRepository(c => c
                    .WithGitUrl(gitUrl)
                    .WithCloneTargetPath(repositoryPath),
                processCommands: _processCommandsMock,
                fileSystem: _fileSystemMock);

        // Assert
        _processCommandsMock.Received(1).CloneGitRepository(gitUrl, expectedPath);
        _builderMock.Received(1).CreateResourceBuilder(Arg.Any<GitRepositoryResource>());
    }

    [Fact]
    public void AddGitRepository_ShouldResolveRepositoryPathCorrectly()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string repositoryPath = "base/path";
        string expectedResolvedPath = Path.Combine(Path.GetFullPath(repositoryPath), "repo");

        _builderMock.CreateResourceBuilder(Arg.Any<GitRepositoryResource>())
            .Returns(Substitute.For<IResourceBuilder<GitRepositoryResource>>());

        _fileSystemMock.DirectoryExists(Arg.Any<string>()).Returns(true);
        _fileSystemMock.FileOrDirectoryExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock
            .AddProjectGitRepository(c => c
                    .WithGitUrl(gitUrl)
                    .WithCloneTargetPath(repositoryPath),
                processCommands: _processCommandsMock,
                fileSystem: _fileSystemMock);

        // Assert
        _builderMock.Received(1).CreateResourceBuilder(Arg.Is<GitRepositoryResource>(r => r.RepositoryPath == expectedResolvedPath));
    }

    [Fact]
    public void AddGitRepository_ShouldThrowExceptionIfProjectPathNotFound()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string repositoryPath = Path.Combine(Path.GetTempPath(), "repo");

        _builderMock.CreateResourceBuilder(Arg.Any<GitRepositoryResource>())
            .Returns(Substitute.For<IResourceBuilder<GitRepositoryResource>>());

        _fileSystemMock.DirectoryExists(Arg.Any<string>()).Returns(true);
        _fileSystemMock.FileOrDirectoryExists(Arg.Any<string>()).Returns(false);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _builderMock
            .AddProjectGitRepository(c => c
                    .WithGitUrl(gitUrl)
                    .WithCloneTargetPath(repositoryPath),
                processCommands: _processCommandsMock,
                fileSystem: _fileSystemMock));

        Assert.Contains("Project folder", exception.Message);
    }

    [Fact]
    public void AddGitRepository_ShouldUseProvidedName()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string providedName = "CustomName";

        _builderMock.CreateResourceBuilder(Arg.Any<GitRepositoryResource>())
            .Returns(Substitute.For<IResourceBuilder<GitRepositoryResource>>());

        _fileSystemMock.DirectoryExists(Arg.Any<string>()).Returns(true);
        _fileSystemMock.FileOrDirectoryExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock
            .AddProjectGitRepository(c => c
                    .WithGitUrl(gitUrl)
                    .WithName(providedName),
                processCommands: _processCommandsMock,
                fileSystem: _fileSystemMock);

        // Assert
        _builderMock.Received(1).CreateResourceBuilder(Arg.Is<GitRepositoryResource>(r => r.Name == providedName));
    }
}