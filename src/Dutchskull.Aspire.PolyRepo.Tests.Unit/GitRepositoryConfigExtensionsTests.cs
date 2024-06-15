using Dutchskull.Aspire.PolyRepo.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

public class GitRepositoryConfigExtensionsTests
{
    private readonly RepositoryConfigBuilder _builder = new RepositoryConfigBuilder()
        .WithGitUrl("https://github.com/example/repo.git")
        .WithFileSystem(Substitute.For<IFileSystem>())
        .WithProcessCommandExecutor(Substitute.For<IProcessCommandExecutor>());

    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    public void Should_check_clone_is_called(bool pathIsCorrect, int callAmount)
    {
        // Arrange
        RepositoryConfig gitRepositoryConfig = _builder
            .WithTargetPath("/custom/path")
            .Build();

        gitRepositoryConfig.FileSystem
            .DirectoryExists(gitRepositoryConfig.RepositoryPath)
            .Returns(pathIsCorrect);

        // Act
        gitRepositoryConfig.CloneRepository();

        // Assert
        gitRepositoryConfig.ProcessCommandsExecutor
            .Received(callAmount)
            .CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryConfig.RepositoryPath,
                gitRepositoryConfig.Branch);
    }

    [Fact]
    public void InitializeGitRepository_ShouldReturnInitializedConfig()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string cloneTargetPath = "/custom/path";
        string branch = "develop";
        string projectPath = "src";
        IFileSystem fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileOrDirectoryExists(Arg.Is(
                Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"), projectPath))))
            .Returns(true);
        IProcessCommandExecutor processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

        var configureGitRepository = new Action<RepositoryConfigBuilder>(builder =>
        {
            builder
                .WithTargetPath(cloneTargetPath)
                .WithDefaultBranch(branch)
                .WithFileSystem(fileSystem)
                .WithProcessCommandExecutor(processCommandsExecutor);
        });

        // Act
        RepositoryConfig config = configureGitRepository.InitializeRepository(gitUrl);

        // Assert
        config.GitUrl.Should().Be(gitUrl);
        config.Branch.Should().Be(branch);
        config.RepositoryPath.Should().Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
        config.ProcessCommandsExecutor.Should().Be(processCommandsExecutor);
        config.FileSystem.Should().Be(fileSystem);
    }

    // [Fact]
    // public void SetupGitRepository_ShouldReturnConfig_WhenProjectPathExists()
    // {
    //     // Arrange
    //     RepositoryConfig gitRepositoryConfig = _builder
    //         .WithTargetPath("/custom/path")
    //         .Build();
    //
    //     gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(true);
    //
    //     // Act
    //     RepositoryConfig result = gitRepositoryConfig.SetupRepository();
    //
    //     // Assert
    //     gitRepositoryConfig.ProcessCommandsExecutor.Received(1).CloneGitRepository(gitRepositoryConfig.GitUrl,
    //         gitRepositoryConfig.RepositoryPath, gitRepositoryConfig.Branch);
    //     result.Should().Be(gitRepositoryConfig);
    // }
    //
    // [Fact]
    // public void SetupGitRepository_ShouldThrowException_WhenProjectPathDoesNotExist()
    // {
    //     // Arrange
    //     RepositoryConfig gitRepositoryConfig = _builder
    //         .WithTargetPath("/custom/path")
    //         .Build();
    //
    //     gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(false);
    //
    //     // Act
    //     Action action = () => gitRepositoryConfig.SetupRepository();
    //
    //     // Assert
    //     action.Should().Throw<Exception>().WithMessage($"Project folder {gitRepositoryConfig.ProjectPath} not found");
    // }
}