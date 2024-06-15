using FluentAssertions;

namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

public class GitUrlUtilitiesTests
{
    private const string GitUrl = "https://github.com/example/repo.git";
    private const string ExpectedProjectName = "repo";

    [Theory]
    [InlineData("https://github.com/example/repo.git", true)]
    [InlineData("https://github.com/example/repo", false)]
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
}