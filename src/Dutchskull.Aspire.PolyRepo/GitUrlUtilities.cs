using System.Text.RegularExpressions;

namespace Dutchskull.Aspire.PolyRepo;

internal static partial class GitUrlUtilities
{
    internal static string GetProjectNameFromGitUrl(string gitUrl)
    {
        string normalizedGitUrl = gitUrl.TrimEnd('/').RemovePostfix(".git");
        string encodedProjectName = normalizedGitUrl[(normalizedGitUrl.LastIndexOf('/') + 1)..];
        string projectName = Uri.UnescapeDataString(encodedProjectName);

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
