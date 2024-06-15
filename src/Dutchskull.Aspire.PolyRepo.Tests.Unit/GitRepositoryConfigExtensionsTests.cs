//using Dutchskull.Aspire.PolyRepo.Interfaces;
//using FluentAssertions;
//using NSubstitute;

//namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

//public class GitRepositoryConfigExtensionsTests
//{
//    private readonly GitRepositoryConfigBuilder _builder = new GitRepositoryConfigBuilder()
//        .WithGitUrl("https://github.com/example/repo.git")
//        .WithFileSystem(Substitute.For<IFileSystem>())
//        .WithProcessCommandExecutor(Substitute.For<IProcessCommandExecutor>())
//        .WithName("");

//    [Theory]
//    [InlineData(true, 0)]
//    [InlineData(false, 1)]
//    public void Should_check_clone_is_called(bool pathIsCorrect, int callAmount)
//    {
//        // Arrange
//        var gitRepositoryConfig = _builder
//            .WithTargetPath("/custom/path")
//            .Build();

//        gitRepositoryConfig.FileSystem
//            .DirectoryExists(gitRepositoryConfig.RepositoryPath)
//            .Returns(pathIsCorrect);

//        // Act
//        gitRepositoryConfig.CloneGitRepository();

//        // Assert
//        gitRepositoryConfig.ProcessCommandsExecutor
//            .Received(callAmount)
//            .CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryConfig.RepositoryPath, gitRepositoryConfig.Branch);
//    }

//    [Fact]
//    public void CloneGitRepository_ShouldNotClone_WhenCloneTargetExists()
//    {
//        // Arrange
//        var gitRepositoryConfig = new RepositoryConfig
//        {
//            Name = "",
//            GitUrl = "",
//            Branch = "main",
//            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
//            ProjectPath = "",
//            RepositoryPath = "/custom/path",
//            FileSystem = Substitute.For<IFileSystem>(),
//        };

//        gitRepositoryConfig.FileSystem.DirectoryExists(gitRepositoryConfig.RepositoryPath).Returns(true);

//        // Act
//        gitRepositoryConfig.CloneGitRepository();

//        // Assert
//        gitRepositoryConfig.ProcessCommandsExecutor.DidNotReceiveWithAnyArgs().CloneGitRepository(default, default, default);
//    }

//    [Fact]
//    public void InitializeGitRepository_ShouldReturnInitializedConfig()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string cloneTargetPath = "/custom/path";
//        string branch = "develop";
//        string projectPath = "src";
//        string name = "CustomName";
//        var fileSystem = Substitute.For<IFileSystem>();
//        fileSystem.FileOrDirectoryExists(Arg.Is(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"), projectPath)))).Returns(true);
//        var processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

//        var configureGitRepository = new Action<GitRepositoryConfigBuilder>(builder =>
//        {
//            builder
//                .WithGitUrl(gitUrl)
//                .WithTargetPath(cloneTargetPath)
//                .WithDefaultBranch(branch)
//                .WithProjectPath(projectPath)
//                .WithName(name)
//                .WithFileSystem(fileSystem)
//                .WithProcessCommandExecutor(processCommandsExecutor);
//        });

//        // Act
//        var config = configureGitRepository.InitializeGitRepository();

//        // Assert
//        config.GitUrl.Should().Be(gitUrl);
//        config.Name.Should().Be(name);
//        config.Branch.Should().Be(branch);
//        config.RepositoryPath.Should().Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
//        config.ProjectPath.Should().Be(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"), projectPath)));
//        config.ProcessCommandsExecutor.Should().Be(processCommandsExecutor);
//        config.FileSystem.Should().Be(fileSystem);
//    }

//    [Fact]
//    public void SetupGitRepository_ShouldReturnConfig_WhenProjectPathExists()
//    {
//        // Arrange
//        var gitRepositoryConfig = new RepositoryConfig
//        {
//            Name = "",
//            GitUrl = "",
//            Branch = "main",
//            RepositoryPath = "",
//            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
//            ProjectPath = "/custom/path/src",
//            FileSystem = Substitute.For<IFileSystem>(),
//        };

//        gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(true);

//        // Act
//        var result = gitRepositoryConfig.SetupGitRepository();

//        // Assert
//        gitRepositoryConfig.ProcessCommandsExecutor.Received(1).CloneGitRepository(gitRepositoryConfig.GitUrl, gitRepositoryConfig.RepositoryPath, gitRepositoryConfig.Branch);
//        result.Should().Be(gitRepositoryConfig);
//    }

//    [Fact]
//    public void SetupGitRepository_ShouldThrowException_WhenProjectPathDoesNotExist()
//    {
//        // Arrange
//        var gitRepositoryConfig = new RepositoryConfig
//        {
//            Name = "",
//            GitUrl = "",
//            Branch = "main",
//            RepositoryPath = "",
//            ProcessCommandsExecutor = Substitute.For<IProcessCommandExecutor>(),
//            ProjectPath = "/custom/path/src",
//            FileSystem = Substitute.For<IFileSystem>(),
//        };

//        gitRepositoryConfig.FileSystem.FileOrDirectoryExists(gitRepositoryConfig.ProjectPath).Returns(false);

//        // Act
//        Action action = () => gitRepositoryConfig.SetupGitRepository();

//        // Assert
//        action.Should().Throw<Exception>().WithMessage($"Project folder {gitRepositoryConfig.ProjectPath} not found");
//    }
//}