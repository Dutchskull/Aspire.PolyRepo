using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using NSubstitute;

namespace Aspire.Git.Tests.Unit;

public class ProjectResourceBuilderExtensionsTests
{
    private readonly IDistributedApplicationBuilder _builderMock;
    private readonly IFileSystem _fileSystemMock;
    private readonly IProcessCommands _processCommandsMock;

    public ProjectResourceBuilderExtensionsTests()
    {
        _builderMock = Substitute.For<IDistributedApplicationBuilder>();
        _processCommandsMock = Substitute.For<IProcessCommands>();
        _fileSystemMock = Substitute.For<IFileSystem>();
    }

    [Fact]
    public void AddGitRepository_ShouldCloneRepositoryIfNotExists()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string repositoryPath = Path.Combine(Path.GetTempPath(), "repo");

        _builderMock.CreateResourceBuilder(Arg.Any<GitRepositoryResource>())
            .Returns(Substitute.For<IResourceBuilder<GitRepositoryResource>>());

        _fileSystemMock.DirectoryExists(repositoryPath).Returns(false);
        _fileSystemMock.FileExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock.AddGitRepository(c => c.WithGitUrl(gitUrl).WithRepositoryPath(repositoryPath), _processCommandsMock, _fileSystemMock);

        // Assert
        _processCommandsMock.Received(1).CloneGitRepository(gitUrl, repositoryPath);
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
        _fileSystemMock.FileExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock.AddGitRepository(c => c.WithGitUrl(gitUrl).WithRepositoryPath(repositoryPath), _processCommandsMock, _fileSystemMock);

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
        _fileSystemMock.FileExists(Arg.Any<string>()).Returns(false);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _builderMock.AddGitRepository(c => c.WithGitUrl(gitUrl).WithRepositoryPath(repositoryPath), _processCommandsMock, _fileSystemMock));
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
        _fileSystemMock.FileExists(Arg.Any<string>()).Returns(true);

        // Act
        var result = _builderMock.AddGitRepository(c => c.WithGitUrl(gitUrl).WithName(providedName), _processCommandsMock, _fileSystemMock);

        // Assert
        _builderMock.Received(1).CreateResourceBuilder(Arg.Is<GitRepositoryResource>(r => r.Name == providedName));
    }
}