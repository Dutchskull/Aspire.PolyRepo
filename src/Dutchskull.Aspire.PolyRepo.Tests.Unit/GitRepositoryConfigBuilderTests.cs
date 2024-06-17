using Dutchskull.Aspire.PolyRepo.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

public class GitRepositoryConfigBuilderTests
{
    [Fact]
    public void BuildConfig_ShouldSetDefaultValues_WhenNoValuesAreProvided()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        RepositoryConfigBuilder builder = new RepositoryConfigBuilder().WithGitUrl(gitUrl);

        // Act
        RepositoryConfig config = builder.Build();

        // Assert
        config.GitUrl.Should().Be(gitUrl);
        config.Branch.Should().Be(null);
        config.RepositoryPath.Should().Be(Path.Combine(Path.GetFullPath("."), "repo"));
        config.ProcessCommandsExecutor.Should().BeOfType<ProcessCommandExecutor>();
        config.FileSystem.Should().BeOfType<FileSystem>();
    }

    [Fact]
    public void BuildConfig_ShouldThrowException_WhenGitUrlIsNotProvided()
    {
        // Arrange
        RepositoryConfigBuilder builder = new();

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().Throw<NoRepositoryUrlException>();
    }

    [Fact]
    public void BuildConfig_ShouldUseProvidedValues_WhenValuesAreProvided()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        const string cloneTargetPath = "/custom/path";
        const string branch = "develop";
        IFileSystem? fileSystem = Substitute.For<IFileSystem>();
        IProcessCommandExecutor? processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

        RepositoryConfigBuilder builder = new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .WithTargetPath(cloneTargetPath)
            .WithDefaultBranch(branch)
            .WithFileSystem(fileSystem)
            .WithProcessCommandExecutor(processCommandsExecutor);

        // Act
        RepositoryConfig config = builder.Build();

        // Assert
        config.GitUrl.Should().Be(gitUrl);
        config.Branch.Should().Be(branch);
        config.RepositoryPath.Should().Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
        config.ProcessCommandsExecutor.Should().Be(processCommandsExecutor);
        config.FileSystem.Should().Be(fileSystem);
    }

    [Fact]
    public void WithCloneTargetPath_ShouldSetCloneTargetPath()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        const string cloneTargetPath = "/custom/path";

        new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .WithTargetPath(cloneTargetPath)
            .Build()
            .RepositoryPath
            .Should()
            .Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
    }

    [Fact]
    public void WithDefaultBranch_ShouldSetBranch()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        const string branch = "develop";

        new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .WithDefaultBranch(branch)
            .Build()
            .Branch
            .Should()
            .Be(branch);
    }

    [Fact]
    public void WithFileSystem_ShouldSetFileSystem()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        IFileSystem? fileSystem = Substitute.For<IFileSystem>();

        new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .WithFileSystem(fileSystem)
            .Build()
            .FileSystem
            .Should()
            .Be(fileSystem);
    }

    [Fact]
    public void WithGitUrl_ShouldSetGitUrl()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";

        new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .Build()
            .GitUrl
            .Should()
            .Be(gitUrl);
    }

    [Fact]
    public void WithProcessCommandExecutor_ShouldSetProcessCommandsExecutor()
    {
        // Arrange
        const string gitUrl = "https://github.com/example/repo.git";
        IProcessCommandExecutor? processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

        new RepositoryConfigBuilder()
            .WithGitUrl(gitUrl)
            .WithProcessCommandExecutor(processCommandsExecutor)
            .Build()
            .ProcessCommandsExecutor
            .Should()
            .Be(processCommandsExecutor);
    }
}