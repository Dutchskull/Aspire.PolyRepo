using System.Text.RegularExpressions;

namespace Dutchskull.Aspire.PolyRepo;

internal static partial class GitUrlUtilities
{
    internal static string GetProjectNameFromGitUrl(string gitUrl)
    {
        ReadOnlySpan<char> gitUrlSpan = gitUrl.AsSpan().TrimEnd('/');
        int projectNameStart = gitUrlSpan.LastIndexOf('/');

        if (projectNameStart < 0 || projectNameStart == gitUrlSpan.Length - 1)
        {
            throw new ArgumentException("Git URL could not be parsed.", nameof(gitUrl));
        }

        ReadOnlySpan<char> encodedProjectName = gitUrlSpan[(projectNameStart + 1)..];

        if (encodedProjectName.EndsWith(".git", StringComparison.Ordinal))
        {
            encodedProjectName = encodedProjectName[..^4];
        }

        string projectName = encodedProjectName.Contains('%')
            ? Uri.UnescapeDataString(encodedProjectName.ToString())
            : encodedProjectName.ToString();

        if (string.IsNullOrWhiteSpace(projectName) ||
            projectName is "." or ".." ||
            projectName.Contains('/') ||
            projectName.Contains('\\') ||
            projectName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Git URL resolved to an unsafe repository name.", nameof(gitUrl));
        }

        return projectName;
    }

    internal static bool IsValidGitUrl(string url) =>
        !string.IsNullOrEmpty(url) && GitUrlRegex().IsMatch(url);

    [GeneratedRegex(@"^(?:(?:git|https?):\/\/[^\s]+\.git(?:\/)?|https:\/\/(?:[\w.-]+@)?dev\.azure\.com\/[^\/\s]+\/[^\/\s]+\/_git\/[^\/\s]+(?:\/)?|git@ssh\.dev\.azure\.com:v3\/[^\/\s]+\/[^\/\s]+\/[^\/\s]+(?:\/)?)$")]
    private static partial Regex GitUrlRegex();
}
