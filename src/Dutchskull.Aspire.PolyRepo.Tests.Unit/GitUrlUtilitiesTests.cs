using Dutchskull.Aspire.PolyRepo;
using FluentAssertions;
using NSubstitute;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Dutchskull.Aspire.PolyRepo.Tests.Unit;

public class GitUrlUtilitiesTests
{
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
    public void IsValidGitUrl_ShouldReturnExpectedResult(string url, bool expectedResult)
    {
        // Act
        bool isValid = GitUrlUtilities.IsValidGitUrl(url);

        // Assert
        isValid.Should().Be(expectedResult);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnProjectName()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string expectedProjectName = "repo";

        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(gitUrl);

        // Assert
        projectName.Should().Be(expectedProjectName);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnProjectName_WhenUrlEndsWithGitExtension()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo.git";
        string expectedProjectName = "repo";

        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(gitUrl);

        // Assert
        projectName.Should().Be(expectedProjectName);
    }

    [Fact]
    public void GetProjectNameFromGitUrl_ShouldReturnLastSegment_WhenUrlDoesNotEndWithGitExtension()
    {
        // Arrange
        string gitUrl = "https://github.com/example/repo";
        string expectedProjectName = "repo";

        // Act
        string projectName = GitUrlUtilities.GetProjectNameFromGitUrl(gitUrl);

        // Assert
        projectName.Should().Be(expectedProjectName);
    }
}
