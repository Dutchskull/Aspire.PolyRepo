//using Dutchskull.Aspire.PolyRepo.Interfaces;
//using FluentAssertions;
//using NSubstitute;

//namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

//public class GitRepositoryConfigBuilderTests
//{
//    [Fact]
//    public void BuildConfig_ShouldSetDefaultValues_WhenNoValuesAreProvided()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        var builder = new GitRepositoryConfigBuilder().WithGitUrl(gitUrl);

//        // Act
//        var config = builder.Build();

//        // Assert
//        config.GitUrl.Should().Be(gitUrl);
//        config.Name.Should().Be("repo");
//        config.Branch.Should().Be("main");
//        config.RepositoryPath.Should().Be(Path.Combine(Path.GetFullPath("."), "repo"));
//        config.ProjectPath.Should().Be(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath("."), "repo"), ".")));
//        config.ProcessCommandsExecutor.Should().BeOfType<ProcessCommandExecutor>();
//        config.FileSystem.Should().BeOfType<FileSystem>();
//    }

//    [Fact]
//    public void BuildConfig_ShouldThrowException_WhenGitUrlIsNotProvided()
//    {
//        // Arrange
//        var builder = new GitRepositoryConfigBuilder();

//        // Act
//        Action act = () => builder.Build();

//        // Assert
//        act.Should().Throw<InvalidOperationException>().WithMessage("GitUrl must be provided");
//    }

//    [Fact]
//    public void BuildConfig_ShouldUseProvidedValues_WhenValuesAreProvided()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string cloneTargetPath = "/custom/path";
//        string branch = "develop";
//        string projectPath = "src";
//        string name = "CustomName";
//        var fileSystem = Substitute.For<IFileSystem>();
//        var processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

//        var builder = new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithTargetPath(cloneTargetPath)
//            .WithDefaultBranch(branch)
//            .WithProjectPath(projectPath)
//            .WithName(name)
//            .WithFileSystem(fileSystem)
//            .WithProcessCommandExecutor(processCommandsExecutor);

//        // Act
//        var config = builder.Build();

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
//    public void WithCloneTargetPath_ShouldSetCloneTargetPath()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string cloneTargetPath = "/custom/path";

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithTargetPath(cloneTargetPath)
//            .Build()
//            .RepositoryPath
//            .Should()
//            .Be(Path.Combine(Path.GetFullPath(cloneTargetPath), "repo"));
//    }

//    [Fact]
//    public void WithDefaultBranch_ShouldSetBranch()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string branch = "develop";

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithDefaultBranch(branch)
//            .Build()
//            .Branch
//            .Should()
//            .Be(branch);
//    }

//    [Fact]
//    public void WithFileSystem_ShouldSetFileSystem()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        var fileSystem = Substitute.For<IFileSystem>();

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithFileSystem(fileSystem)
//            .Build()
//            .FileSystem
//            .Should()
//            .Be(fileSystem);
//    }

//    [Fact]
//    public void WithGitUrl_ShouldSetGitUrl()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .Build()
//            .GitUrl
//            .Should()
//            .Be(gitUrl);
//    }

//    [Fact]
//    public void WithName_ShouldSetName()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string name = "CustomName";

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithName(name)
//            .Build()
//            .Name
//            .Should()
//            .Be(name);
//    }

//    [Fact]
//    public void WithProcessCommandExecutor_ShouldSetProcessCommandsExecutor()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        var processCommandsExecutor = Substitute.For<IProcessCommandExecutor>();

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithProcessCommandExecutor(processCommandsExecutor)
//            .Build()
//            .ProcessCommandsExecutor
//            .Should()
//            .Be(processCommandsExecutor);
//    }

//    [Fact]
//    public void WithProjectPath_ShouldSetProjectPath()
//    {
//        // Arrange
//        string gitUrl = "https://github.com/example/repo.git";
//        string projectPath = "src";

//        new GitRepositoryConfigBuilder()
//            .WithGitUrl(gitUrl)
//            .WithProjectPath(projectPath)
//            .Build()
//            .ProjectPath
//            .Should()
//            .Be(Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath("."), "repo"), projectPath)));
//    }
//}
