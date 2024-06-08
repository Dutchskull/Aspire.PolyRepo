using Dutchskull.Aspire.Git.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Dutchskull.Aspire.Git.Tests.Unit;

public class GitRepositoryConfigExtensionsTests
{
    [Fact]
    public void CloneGitRepository_ShouldClone_WhenCloneTargetDoesNotExist()
    {
        // Arrange
        var gitRepositoryConfig = new GitRepositoryConfig
        {
            ProjectPath = "",
            GitUrl = "https://github.com/example/repo.git",
            CloneTargetPath = "/custom/path",
            Branch = "main",
            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
            FileSystem = Substitute.For<IFileSystem>(),
        };

        gitRepositoryConfig.FileSystem.DirectoryExists(gitRepositoryConfig.CloneTargetPath).Returns(false);

        // Act
        gitRepositoryConfig.CloneGitRepository();

        // Assert
        gitRepositoryConfig.ProcessCommandsExecutor.Received(1).CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryConfig.CloneTargetPath, gitRepositoryConfig.Branch);
    }

    [Fact]
    public void CloneGitRepository_ShouldNotClone_WhenCloneTargetExists()
    {
        // Arrange
        var gitRepositoryConfig = new GitRepositoryConfig
        {
            Branch = "main",
            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
            ProjectPath = "",
            CloneTargetPath = "/custom/path",
            FileSystem = Substitute.For<IFileSystem>(),
        };

        gitRepositoryConfig.FileSystem.DirectoryExists(gitRepositoryConfig.CloneTargetPath).Returns(true);

        // Act
        gitRepositoryConfig.CloneGitRepository();

        // Assert
        gitRepositoryConfig.ProcessCommandsExecutor.DidNotReceiveWithAnyArgs().CloneGitRepository(default, default, default);
    }

    [Fact]
    public void InitializeGitRepository_ShouldReturnInitializedConfig()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string cloneTargetPath = "/custom/path";
        string branch = "develop";
        string projectPath = "src";
        string name = "CustomName";
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileOrDirectoryExists(Arg.Is(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"), projectPath)))).Returns(true);
        var processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

        var configureGitRepository = new Action<GitRepositoryConfigBuilder>(builder =>
        {
            builder
                .WithGitUrl(gitUrl)
                .WithCloneTargetPath(cloneTargetPath)
                .WithDefaultBranch(branch)
                .WithProjectPath(projectPath)
                .WithName(name)
                .WithFileSystem(fileSystem)
                .WithProcessCommandExecutor(processCommandsExecutor);
        });

        // Act
        var config = configureGitRepository.InitializeGitRepository();

        // Assert
        config.GitUrl.Should().Be(gitUrl);
        config.Name.Should().Be(name);
        config.Branch.Should().Be(branch);
        config.CloneTargetPath.Should().Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
        config.ProjectPath.Should().Be(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"), projectPath)));
        config.ProcessCommandsExecutor.Should().Be(processCommandsExecutor);
        config.FileSystem.Should().Be(fileSystem);
    }

    [Fact]
    public void SetupGitRepository_ShouldReturnConfig_WhenProjectPathExists()
    {
        // Arrange
        var gitRepositoryConfig = new GitRepositoryConfig
        {
            Branch = "main",
            CloneTargetPath = "",
            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
            ProjectPath = "/custom/path/src",
            FileSystem = Substitute.For<IFileSystem>(),
        };

        gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(true);

        // Act
        var result = gitRepositoryConfig.SetupGitRepository();

        // Assert
        result.Should().Be(gitRepositoryConfig);
    }

    [Fact]
    public void SetupGitRepository_ShouldThrowException_WhenProjectPathDoesNotExist()
    {
        // Arrange
        var gitRepositoryConfig = new GitRepositoryConfig
        {
            Branch = "main",
            CloneTargetPath = "",
            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
            ProjectPath = "/custom/path/src",
            FileSystem = Substitute.For<IFileSystem>(),
        };

        gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(false);

        // Act
        Action action = () => gitRepositoryConfig.SetupGitRepository();

        // Assert
        action.Should().Throw<Exception>().WithMessage($"Project folder {gitRepositoryConfig.ProjectPath} not found");
    }
}