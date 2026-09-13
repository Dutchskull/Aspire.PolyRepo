using FluentAssertions;

namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

public class GitUrlUtilitiesTests
{
    private const string GitUrl = "https://github.com/example/repo.git";
    private const string ExpectedProjectName = "repo";
    private const string AzureDevOpsGitUrl = "https://dev.azure.com/example/example%20web%20site/_git/example%20web%20site";

    [Theory]
    [InlineData("https://github.com/example/repo.git", true)]
    [InlineData("https://github.com/example/repo", false)]
    [InlineData("https://example@dev.azure.com/example/example%20web%20site/_git/example%20web%20site", true)]
    [InlineData("https://dev.azure.com/example/example%20web%20site/_git/example%20web%20site", true)]
    [InlineData("https://dev.azure.com/example/example%20web%20site/_git/example%20web%20site/", true)]
    [InlineData("git@ssh.dev.azure.com:v3/example/example%20web%20site/example%20web%20site", true)]
    [InlineData("git@github.com:example/repo", false)]
    [InlineData("git://github.com/example/repo", false)]
    [InlineData("https://example.com", false)]
    [InlineData("git@example.com:example/repo.git", false)]
    [InlineData("git://example.com", false)]
    [InlineData("invalid-url", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidGitUrl_ShouldReturnExpectedResult(string? url, bool expectedResult)
    {
        // Act
        bool isValid = GitUrlUtilities.IsValidGitUrl(url!);

        // Assert
        isValid.Should().Be(expectedResult);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnProjectName()
    {
        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(GitUrl);

        // Assert
        projectName.Should().Be(ExpectedProjectName);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnProjectName_WhenUrlEndsWithGitExtension()
    {
        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(GitUrl);

        // Assert
        projectName.Should().Be(ExpectedProjectName);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnLastSegment_WhenUrlDoesNotEndWithGitExtension()
    {
        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(GitUrl);

        // Assert
        projectName.Should().Be(ExpectedProjectName);
    }

    [Theory]
    [InlineData(AzureDevOpsGitUrl)]
    [InlineData("https://dev.azure.com/example/example%20web%20site/_git/example%20web%20site/")]
    [InlineData("git@ssh.dev.azure.com:v3/example/example%20web%20site/example%20web%20site")]
    public void GetProjectNameFromGitUrl_ShouldDecodePercentEncodedRepositoryNames(string gitUrl)
    {
        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(gitUrl);

        // Assert
        projectName.Should().Be("example web site");
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldThrow_WhenDecodedRepositoryNameContainsPathSeparators()
    {
        // Act
        Action act = () => GitUrlUtilities.GetProjectNameFromGitUrl("https://dev.azure.com/example/project/_git/repository%2Fchild");

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}